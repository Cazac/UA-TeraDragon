using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class PathFinding
{
    // rules: start alwats last column and end always last
    public static List<List<WorldTile>> GetPaths( GameObject[,] map, List<WorldTile> constSpawn) {


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

        return paths;
    }

    static List<WorldTile> endTiles;
    static Queue<WorldTile> enqueuedTiles = new Queue<WorldTile>();
    static void DFS(WorldTile tile) {
        List<WorldTile> list = new List<WorldTile>();
        list.Add(tile);
        DFS_Util(tile, list);

        string s = "";
        for (int i = 0; i < list.Count; i++) {
            s = s + "->(" + list[i].gridX + "," + list[i].gridY + ")";
        }
        Debug.Log(s);

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
        //if (nextTile.myNeighbours.Count == 0) {
        //    string s = "";
        //    foreach (WorldTile tile in worldTiles) {
        //        s = s + tile.ToString() + "  ";
        //    }
        //    Debug.Log(s);
        //}

    }

    public static List<T> DeepClone<T>(this List<T> items) {
        return new List<T>(items);
    }



}
