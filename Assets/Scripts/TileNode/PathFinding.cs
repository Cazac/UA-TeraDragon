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
        return GetPaths( map, constSpawn, map.GetLength(0) - 1);
    }

    /// <summary>
    /// Returns object contianing a list of paths, form shorst to longest, and 2 dictionaries of these paths
    /// with starting and ending tiles as keys
    /// </summary>
    /// <param name="map">table of nodes with their neighbours set</param>
    /// <param name="constSpawn">list of tiles that spawns enemies irrespective of location</param>
    /// <param name="startingColumn">column where we search fo starting tiles. First colum is 0</param>
    /// <returns></returns>
    public static PathsData GetPaths(GameObject[,] map, List<WorldTile> constSpawn, int startingColumn)
    {
        if(startingColumn <= 0 || map.GetLength(0) <= startingColumn){
            return new PathsData(new List<List<WorldTile>>() );
        }
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

        startingTiles.AddRange(constSpawn);
        // finds all rightmost paths tiles
        for (int i = 0; i < map.GetLength(1); i++)
        {
            if (map[startingColumn, i] != null)
            {
                if (map[startingColumn, i].GetComponent<WorldTile>().walkable)
                {
                    startingTiles.Add(map[startingColumn, i].GetComponent<WorldTile>());
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

        int dfsLimit = Mathf.Max(0, startingColumn - 2);
        foreach (WorldTile wt in startingTiles)
        {
            DFS(wt, dfsLimit);
        }
        PathsData PathData = new PathsData(paths);
        
        return PathData;
    }


    /// <summary>
    /// Use depth first search to find all paths from start tiles to end tiles with limited ability to go right.
    /// </summary>
    /// <param name="tile"></param>
    static void DFS(WorldTile tile, int limit) {
        List<WorldTile> list = new List<WorldTile>();
        DFS_Util(tile, list, limit);
    }

    // Actual function, is recursive
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

