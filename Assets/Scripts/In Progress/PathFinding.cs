using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class PathFinding {

    static List<WorldTile> endTiles;


    // rules: start alwats last column and end always last
    public static PathsData GetPaths( GameObject[,] map, List<WorldTile> constSpawn) {


        /*  Steps:
         *  Find End(there can only be one
         *  Find starts (there can be multiples)
         *  Use breath first serach to find all paths
         *  knowing that you cannot move to the right 
         *  and to previously visited node
         * 
         */
        endTiles = new List<WorldTile>();
        paths = new List<List<WorldTile>>();
        List<WorldTile> startingTiles = new List<WorldTile>();
        for (int i = 0; i < map.GetLength(1); i++) {
            if (map[map.GetLength(0) -1, i] != null) {
            //    Debug.Log("y:" + i + "  " + map[map.GetLength(0) - 1, i].GetComponent<WorldTile>().walkable);
                if (map[map.GetLength(0)-1, i].GetComponent<WorldTile>().walkable) {
                    startingTiles.Add(map[map.GetLength(0)-1, i].GetComponent<WorldTile>());
                }
            }
        }
        for (int i = 0; i < map.GetLength(1); i++) {
            if (map[0, i] != null) {
            //    Debug.Log("y:" + i + "  " + map[0, i].GetComponent<WorldTile>().walkable);
                if (map[0, i].GetComponent<WorldTile>().walkable) {
                    endTiles.Add(map[0, i].GetComponent<WorldTile>());
                }
            }
        }

        startingTiles.AddRange(constSpawn);

        Debug.Log("Count: " + startingTiles.Count);
        foreach(WorldTile wt in startingTiles) {
            DFS(wt);
        }
        

        PathsData PathData = new PathsData(paths);

        return PathData;
    }

    static void DFS(WorldTile tile) {
        List<WorldTile> list = new List<WorldTile>();
       // list.Add(tile);
        DFS_Util(tile, list);

        string s = "";
        for (int i = 0; i < list.Count; i++) {
            s = s + "->(" + list[i].gridX + "," + list[i].gridY + ")";
        }
      //  Debug.Log(s);

    }
    static List<List<WorldTile>> paths;
    static void DFS_Util(WorldTile nextTile, List<WorldTile> worldTiles) {
        worldTiles.Add(nextTile);

        string s = "";
        for (int i = 0; i < worldTiles.Count; i++) {
            s = s + "->(" + worldTiles[i].gridX + "," + worldTiles[i].gridY + ")";
        }
     //   Debug.Log(s);

        if (!endTiles.Contains(nextTile)) {
            foreach (WorldTile tile in nextTile.myNeighbours) {
                //if (tile.gridX > nextTile.gridX)
                //    continue;
                if (worldTiles.Contains(tile))
                    continue;

                // need to find way to reduce num of lists
                DFS_Util(tile, DeepClone<WorldTile>(worldTiles));
                

            }
        }
        else {
            paths.Add(DeepClone<WorldTile>(worldTiles));
        }

    }






     
    public static List<T> DeepClone<T>(this List<T> items) {
        return new List<T>(items);
    }
    
}
class ListSize : IComparer<List<WorldTile>> {
    public int Compare(List<WorldTile> l1, List<WorldTile> l2) {
        if(l1.Count > l2.Count) {
            return 1;
        }
        else {
            return l1.Count.CompareTo(l2.Count);
        }
    }
}

public class PathsData {

    public List<List<WorldTile>> paths;
    public Dictionary<WorldTile, List<List<WorldTile>>> PathsByStart;
    public Dictionary<WorldTile, List<List<WorldTile>>> PathsByEnd;


    public PathsData(List<List<WorldTile>> paths) {
        ListSize comparator = new ListSize();
        this.paths = paths;
        PathsByStart = new Dictionary<WorldTile, List<List<WorldTile>>>();
        PathsByEnd = new Dictionary<WorldTile, List<List<WorldTile>>>();

        paths.Sort(comparator);

        foreach (List<WorldTile> wt in paths) {
            if (!PathsByStart.ContainsKey(wt[0])) {
            PathsByStart.Add(wt[0], new List<List<WorldTile>>());
            }
            else {
                PathsByStart[wt[0]].Add(wt);
            }
            if (!PathsByEnd.ContainsKey(wt[wt.Count-1])) {
                PathsByEnd.Add(wt[wt.Count-1], new List<List<WorldTile>>());
            }
            else {
                PathsByEnd[wt[wt.Count - 1]].Add(wt);
            }
        }
        
    }
}
