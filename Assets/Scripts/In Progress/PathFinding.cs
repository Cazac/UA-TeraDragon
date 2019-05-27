using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class PathFinding
{

    public static void GetPaths(Tilemap tilemap, GameObject[,] map) {

        BoundsInt bounds = tilemap.cellBounds;
        Debug.Log("Bounds: " + bounds.size.x + " " + bounds.size.y);
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        for (int x = 0; x < bounds.size.x; x++) {
            for (int y = 0; y < bounds.size.y; y++) {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null) {
                  //  Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);

                }
                else {
                    //  Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }

        /*  Steps:
         *  Find End(there can only be one
         *  Find starts (there can be multiples)
         *  Use breath first serach to find all paths
         *  knowing that you cannot move to the right 
         *  and to previously visited node
         * 
         */

        List<WorldTile> startingTiles = new List<WorldTile>();
        for (int i = 0; i < map.GetLength(1); i++) {
            if (map[map.GetLength(0) - 2, i] != null) {
                
                if (map[map.GetLength(0) - 2, i].GetComponent<WorldTile>().walkable) {
                    startingTiles.Add(map[map.GetLength(0) - 2, i].GetComponent<WorldTile>());
                }
            }
        }
        for (int i = 0; i < map.GetLength(1); i++) {
            if (map[0, i] != null) {
            Debug.Log("y:" + i + "  " + map[0, i].GetComponent<WorldTile>().walkable);
                if (map[0, i].GetComponent<WorldTile>().walkable) {
                    endTile = map[0, i].GetComponent<WorldTile>();
                    break;
                }
            }
        }
        Debug.Log("Count: " + startingTiles.Count);
        if (startingTiles.Count != 0) {

            DFS(startingTiles[0]);
        }
    }

    static WorldTile endTile;
    static Queue<WorldTile> enqueuedTiles = new Queue<WorldTile>();
    static void DFS(WorldTile tile) {
        List<WorldTile> list = new List<WorldTile>();
        DFS_Util(tile, list);

    }

    static void DFS_Util(WorldTile nextTile, List<WorldTile> worldTiles) {
        worldTiles.Add(nextTile);
        bool first = true;

        if (!nextTile.Equals(endTile)) {
            foreach (WorldTile tile in nextTile.myNeighbours) {
                //if (tile.gridX > nextTile.gridX)
                //    continue;
                if (worldTiles.Contains(tile))
                    continue;
                if (first) {
                    DFS_Util(tile, worldTiles);
                    first = false;
                }
                else {
                    DFS_Util(tile, DeepClone<WorldTile>(worldTiles));
                }

            }
        }
        if (nextTile.myNeighbours.Count == 0) {
            string s = "";
            foreach (WorldTile tile in worldTiles) {
                s = s + tile.ToString() + "  ";
            }
            Debug.Log(s);
        }

    }

    public static List<T> DeepClone<T>(this List<T> items) {
        return new List<T>(items);
    }



}
