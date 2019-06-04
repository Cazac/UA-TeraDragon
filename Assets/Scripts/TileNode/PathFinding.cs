using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class PathFinding {

    static List<WorldTile> endTiles;
    static List<List<WorldTile>> paths;
    static List<WorldTile> startingTiles;

    /// <summary>
    /// Returns object contianing a list of paths, form shorst to longest, and 2 dictionaries of these paths
    /// with starting and ending tiles as keys
    /// </summary>
    /// <param name="map">table of nodes with their neighbours set</param>
    /// <param name="constSpawn">list of tiles that spawns enemies irrespective of location</param>
    /// <returns></returns>
    public static PathsData GetPaths(GameObject[,] map, List<WorldTile> constSpawn)
    {


        /*  Steps:
         *  Find End tiles (all paths tiles on most leftward column)
         *  Find starts (all paths tiles on most rightward column
         *  Use breath first serach to find all paths 
         *      knowing that there is a limit to how far right they can go
         *      and to previously visited node
         */
        endTiles = new List<WorldTile>();
        paths = new List<List<WorldTile>>();
        startingTiles = new List<WorldTile>();

        // finds all rightmost paths tiles
        for (int i = 0; i < map.GetLength(1); i++)
        {
            if (map[map.GetLength(0) - 1, i] != null)
            {
                if (map[map.GetLength(0) - 1, i].GetComponent<WorldTile>().walkable)
                {
                    //FIXME: Hard-coded right most tile
                    startingTiles.Add(map[map.GetLength(0) - 1, i].GetComponent<WorldTile>());
                }
            }
        }

        // finds all leftmost paths tile
        for (int i = 0; i < map.GetLength(1); i++)
        {
            if (map[0, i] != null)
            {
                if (map[0, i].GetComponent<WorldTile>().walkable)
                {
                    endTiles.Add(map[0, i].GetComponent<WorldTile>());
                }
            }
        }
        
        startingTiles.AddRange(constSpawn);

        foreach (WorldTile wt in startingTiles)
        {
            DFS(wt);
        }

        PathsData PathData = new PathsData(paths);
        
        return PathData;
    }

    /// <summary>
    /// Use depth first search to find all paths from start tiles to end tiles with limited backtrack.
    /// </summary>
    /// <param name="tile"></param>
    static void DFS(WorldTile tile) {
        List<WorldTile> list = new List<WorldTile>();
        DFS_Util(tile, list, tile.gridX);
    }

    static void DFS_Util(WorldTile nextTile, List<WorldTile> worldTiles, int furthest) {
        worldTiles.Add(nextTile);
        int nextfurthest;
        if (!endTiles.Contains(nextTile)) {
            foreach (WorldTile tile in nextTile.myNeighbours) {

                nextfurthest = furthest;
                if (worldTiles.Contains(tile))
                    continue;
                // prevents to much backtracking
                if (tile.gridX > furthest + 2 )
                    continue;

                if(tile.gridX < nextfurthest) {
                    nextfurthest = tile.gridX;
                }
                // need to find way to reduce num of lists
                DFS_Util(tile, DeepClone(worldTiles), nextfurthest);
                
            }
        }
        else {
            paths.Add(DeepClone(worldTiles));
        }

    }
    
    static List<T> DeepClone<T>(this List<T> items) {
        return new List<T>(items);
    }
    
}

/// <summary>
/// Contains all paths for each starting tiles to each ending tile
/// Paths can be retrived in 3 ways:
///  - paths: all paths from shortest to longest
///  - PathsByStarts: starting tiles as keys, retrieves all paths associated with key 
///  - PathsByEnd: ending tiles as keys, retrieves all paths associated with key 
/// </summary>
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


        foreach (List<WorldTile> wt in paths)
        {
            if (!PathsByStart.ContainsKey(wt[0]))
            {
                PathsByStart.Add(wt[0], new List<List<WorldTile>>() { wt });
            }
            else
            {
                PathsByStart[wt[0]].Add(wt);
            }

            if (!PathsByEnd.ContainsKey(wt[wt.Count - 1]))
            {
                PathsByEnd.Add(wt[wt.Count - 1], new List<List<WorldTile>>() { wt });
            }
            else
            {
                PathsByEnd[wt[wt.Count - 1]].Add(wt);
            }
        }
    }


}

// Comparor that sorts lists by size
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
