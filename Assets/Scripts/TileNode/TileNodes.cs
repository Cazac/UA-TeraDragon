using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;

///////////////
/// <summary>
///     
/// TD_TileNodes is used to spawn and keep track of all nodes attached to the tilemap sprites
/// 
/// </summary>
///////////////
[ExecuteInEditMode]
[Serializable]
public class TileNodes : MonoBehaviour
{
    [Header("Main Grid / Tilemap")]
    public Grid gridBase;
    public Tilemap uniqueTilemap;
    public Tilemap hiddenTileMap;


    [Header("Monster Prefabs")]
    public GameObject enemyPrefab;

    [Header("Tile Node Prefabs")]
    public GameObject[] TileNodesPrefabs;


    //  TO DO   // - This is not the best?
    [Header("Tile Sprites For Layers")]
    public Tile[] WalkableTiles;
    public Tile[] UnwalkableTiles;
    public Tile[] SpawnTiles;
    public Tile[] HiddenTiles;
    public Tile[] TowerTiles;
    public Tile[] CrystalTiles;



    // Sorted 2D array of nodes
    public GameObject[,] nodes;

    // List of nodes before they are sorted
    private List<GameObject> unsortedNodes;
    private List<WorldTile> permanentSpawnPoints;

    // Auto set to size of the tilemap tiles
    private float mapConstant;

    [SerializeField] // necessary to have the nodes saved in and out of play
    GameObject[] parentNodes = new GameObject[0];
    GameObject parentHiddenNodes;


    [Header("Editor variables")]
    // variables used for gizmo draw and 
    public int SelectedPath = 0;

    public List<WorldTile> selectedList;
    public PathsData pathData;

    public HiddenTileManager hiddenTileManager = new HiddenTileManager();

    //////////////////////////////////////////////////////////

    private void Awake()
    {
        HideTiles();
        BuildTable();
    }

    private void Start()
    {

    }
    private void Update()
    {
        //CheckBlockedPath();
        DrawAllPath();
    }

    public bool CheckBlockedPath()
    {
        if (pathData != null)
        {
            foreach (var path in pathData.paths)
            {
                bool isModified = false;
                foreach (WorldTile tile in path)
                {
                    if (tile.isBlockedBarrier == true)
                    {
                        isModified = true;

                        // Condition for when player places more than 1 barrier in the same path, false will be returned --> deny request
                        if (pathData.blockedPaths.Contains(path))
                            return false;

                        pathData.blockedPaths.Add(path);
                        break;
                    }
                }

                if (!isModified && pathData.blockedPaths.Contains(path))
                    pathData.blockedPaths.Remove(path);
            }
        }
        return true;
    }

    public void BuildTable()
    {

        mapConstant = gridBase.cellSize.x;


        //   listWapper = new ListWapper
        //if (parentNodes.Length < 2)
        {
            for (int i = 0; i < parentNodes.Length; i++)
            {
                if (parentNodes[i] != null)
                {
                    DestroyImmediate(parentNodes[i]);
                }
            }

            parentNodes = new GameObject[TileNodesPrefabs.Length - 1]; //Remove the crystal tile

            parentNodes[0] = new GameObject("Parent_WalkableTiles");
            parentNodes[0].transform.SetParent(transform);

            parentNodes[1] = new GameObject("Parent_UnwalkableTiles");
            parentNodes[1].transform.SetParent(transform);

        }

        //else
        //{
        //    foreach (GameObject child in parentNodes[0].gameObject.transform)
        //    {
        //        DestroyImmediate(child);
        //    }

        //    foreach (GameObject child in parentNodes[1].gameObject.transform)
        //    {
        //        DestroyImmediate(child);
        //    }
        //}


        permanentSpawnPoints = new List<WorldTile>();
        unsortedNodes = new List<GameObject>();
        generateNodes();

        pathData = PathFinding.GetPaths(nodes, permanentSpawnPoints, maxGridX);
    }


    ///////////////
    /// <summary>
    /// Allows you to see the list that is currently selected
    /// </summary>
    ///////////////
    public void Editor_SelectList()
    {
        if (pathData != null && pathData.paths != null)
        {
            if (SelectedPath >= 0 && SelectedPath < pathData.paths.Count)
            {
                selectedList = pathData.paths[SelectedPath];
            }
        }
    }

    public Dictionary<string, GameObject> dictionaryTest;

    ///////////////
    /// <summary>
    /// Creates the nodes, places them in a table and sets each nodes neighbours
    /// </summary>
    ///////////////
    private void generateNodes()
    {
        uniqueTilemap.CompressBounds();
        BoundsInt bounds = uniqueTilemap.cellBounds;
        int tableX = uniqueTilemap.cellBounds.size.x;
        int tableY = uniqueTilemap.cellBounds.size.y;


        nodes = new GameObject[tableX, tableY];

        // create nodes
        LoopThroughTileset();
        // places these nodes in a table
        FillNodeTable();
        // give each node their neighbours
        SetNeigbours();
    }

    private void DrawAllPath()
    {
        //stfu stop placing errors in the log
        return;

        foreach (var path in pathData.paths)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawRay(path[i].transform.position, path[i + 1].transform.position - path[i].transform.position, Color.cyan);
            }
        }

        foreach (var path in pathData.blockedPaths)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawRay(path[i].transform.position, path[i + 1].transform.position - path[i].transform.position, Color.red);
            }
        }
    }

    public void BuildHiddenNodes()
    {
        parentHiddenNodes = new GameObject("Parent_HiddenNodes");
        parentHiddenNodes.transform.SetParent(transform);
        hiddenTileMap.CompressBounds();

        BoundsInt bounds = hiddenTileMap.cellBounds;
        int tableX = hiddenTileMap.cellBounds.size.x;
        int tableY = hiddenTileMap.cellBounds.size.y;


        nodes = new GameObject[tableX, tableY];

        WorldTile wt; GameObject node = null;

        int GridX = 0; int GirdY = 0;
        for (int x = -(nodes.GetLength(0)) - 1; x < nodes.GetLength(0) + 1; x++)
        {
            for (int y = -(nodes.GetLength(1)) - 1; y < nodes.GetLength(1) + 1; y++)
            {
                TileBase tb = hiddenTileMap.GetTile(new Vector3Int(x, y, 0)); //check if we have a floor tile at that world coords

                if (tb != null)
                {
                    Vector3 nodePosition = new Vector3(mapConstant / 2 + ((x + gridBase.transform.position.x) * mapConstant), ((y + 0.5f + gridBase.transform.position.y) * mapConstant), 0);

                    string name = hiddenTileMap.GetTile(hiddenTileMap.WorldToCell(nodePosition)).name;

                    foreach (Tile tile in HiddenTiles)
                    {
                        if (name == tile.name)
                        {
                            node = Instantiate(TileNodesPrefabs[0], nodePosition, Quaternion.identity, parentHiddenNodes.transform);
                        }
                    }

                    wt = node.GetComponent<WorldTile>();
                    if (wt == null)
                        wt = node.GetComponent<CrystalTile>();
                    wt.gridX = GridX;
                    wt.gridY = GirdY;
                }
                GirdY++;
            }
            GirdY = 0; ;
            GridX++;
        }
    }



    ///////////////
    /// <summary>
    /// Scans tileset for tiles and places the corresponding tile node when it enconters one.
    /// </summary>
    ///////////////
    private void LoopThroughTileset()
    {
        WorldTile wt; GameObject node;
        //parentNodes = new GameObject[TileNodesPrefabs.Length];

        //parentNodes[0] = new GameObject("Parent_WalkableTiles");
        //parentNodes[0].transform.SetParent(transform);

        //parentNodes[1] = new GameObject("Parent_UnwalkableTiles");
        //parentNodes[1].transform.SetParent(transform);

        int GridX = 0; int GirdY = 0;
        for (int x = -(nodes.GetLength(0)) - 1; x < nodes.GetLength(0) + 1; x++)
        {
            for (int y = -(nodes.GetLength(1)) - 1; y < nodes.GetLength(1) + 1; y++)
            {
                TileBase tb = uniqueTilemap.GetTile(new Vector3Int(x, y, 0)); //check if we have a floor tile at that world coords


                if (tb != null)
                {
                    Vector3 nodePosition = new Vector3(mapConstant / 2 + ((x + gridBase.transform.position.x) * mapConstant), ((y + 0.5f + gridBase.transform.position.y) * mapConstant), 0);

                    node = null;

                    string name = uniqueTilemap.GetTile(uniqueTilemap.WorldToCell(nodePosition)).name;

                    //print(name);

                    // checks if tile is found in walkable
                    foreach (Tile tile in WalkableTiles)
                    {
                        if (name == tile.name)
                        {
                            node = Instantiate(TileNodesPrefabs[0], nodePosition, Quaternion.identity, parentNodes[0].transform);
                        }
                    }
                    // checks if walkable tile is a spawning tile
                    foreach (Tile spTile in SpawnTiles)
                    {
                        if (name == spTile.name)
                        {
                            node = Instantiate(TileNodesPrefabs[0], nodePosition, Quaternion.identity, parentNodes[0].transform);
                            permanentSpawnPoints.Add(node.GetComponent<WorldTile>());
                        }
                    }
                    // checks if tile is found in unwalkable
                    foreach (Tile tile in UnwalkableTiles)
                    {
                        if (name == tile.name)
                        {
                            node = Instantiate(TileNodesPrefabs[1], nodePosition, Quaternion.identity, parentNodes[1].transform);
                            if (name == "Rock Tile")
                            {
                                //Debug.Log("Name: " + name);
                             //   node.name = "test";
                                  node.GetComponent<WorldTile>().towering = false;
                            }
                        }
                    }
                    // checks if tile is found in Cystal
                    foreach (Tile tile in CrystalTiles)
                    {
                        if (name == tile.name)
                        {
                            node = Instantiate(TileNodesPrefabs[2], nodePosition, Quaternion.identity, parentNodes[1].transform);
                        }
                    }

                    // checks if tile is found in unwalkable
                    //foreach (Tile tile in TowerTiles)
                    //{
                    //if (name == tile.name)
                    //{
                    //node = Instantiate(TileNodesPrefabs[1], nodePosition, Quaternion.identity, parentNodes[1].transform);
                    //}
                    //}



                    if (node == null)
                    {
                        //    Debug.LogError(name + " is not registered.");
                    }
                    else
                    {
                        unsortedNodes.Add(node);
                        wt = node.GetComponent<WorldTile>();
                        if (wt == null)
                            wt = node.GetComponent<CrystalTile>();
                        wt.gridX = GridX;
                        wt.gridY = GirdY;
                    }
                }
                GirdY++;
            }
            GirdY = 0; ;
            GridX++;
        }
    }


    ///////////////
    /// <summary>
    /// Checks tilemap for size of node array then places existing nodes in their corresponding place in the table.
    /// </summary>
    ///////////////
    private void FillNodeTable()
    {
        int minX = nodes.GetLength(0);
        int minY = nodes.GetLength(1);
        WorldTile wt;

        // temp variable

        // makes sure grid is correctly alligned by finding
        // the lowest value for x and y and making sure it is zero
        foreach (GameObject g in unsortedNodes)
        {
            wt = g.GetComponent<WorldTile>();
            if (wt.gridX < minX)
                minX = wt.gridX;
            if (wt.gridY < minY)
                minY = wt.gridY;

        }
        foreach (GameObject g in unsortedNodes)
        {
            wt = g.GetComponent<WorldTile>();
            wt.gridX -= minX;
            wt.gridY -= minY;
            wt.name = "NODE " + wt.gridX.ToString() + " : " + wt.gridY.ToString();
            nodes[wt.gridX, wt.gridY] = g;
            if (wt.gridX > maxGridX)
            {
                maxGridX = wt.gridX;

            }

        }
        //print("MaxGridx:" + maxGridX);
        unsortedNodes.Clear();
    }
    int maxGridX = 0;


    ///////////////
    /// <summary>
    /// For each tile in nodes[] checks the 4 surrounding tiles for neighbours
    /// </summary>
    //////////////////
    private void SetNeigbours()
    {
        WorldTile wt;
        for (int x = 0; x < nodes.GetLength(0); x++)
        {
            for (int y = 0; y < nodes.GetLength(1); y++)
            {
                if (nodes[x, y] != null)
                {
                    wt = nodes[x, y].GetComponent<WorldTile>();
                    wt.myNeighbours = SetNeigbour(x, y, nodes.GetLength(0), nodes.GetLength(1), wt.walkable);
                }
            }
        }
    }


    ///////////////
    /// <summary>
    /// Grab 4 surrounding tiles from all tilemaps and checks if they are neigbours
    /// </summary>
    ///////////////
    private List<WorldTile> SetNeigbour(int x, int y, int width, int height, bool walkable)
    {
        List<WorldTile> myNeighbours = new List<WorldTile>();
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return myNeighbours;
        }

        if (x > 0)
        {
            AddNodeToList(myNeighbours, x - 1, y, walkable);
        }
        if (x < width - 1)
        {
            AddNodeToList(myNeighbours, x + 1, y, walkable);
        }
        if (y > 0)
        {
            AddNodeToList(myNeighbours, x, y - 1, walkable);
        }
        if (y < height - 1)
        {
            AddNodeToList(myNeighbours, x, y + 1, walkable);
        }

        return myNeighbours;
    }


    ///////////////
    /// <summary>
    /// Error check each node and walkable status before adding to the WorldTile neighbours list
    /// </summary>
    ///////////////
    private void AddNodeToList(List<WorldTile> list, int x, int y, bool currentWalkableState)
    {
        if (nodes[x, y] != null)
        {
            WorldTile wt = nodes[x, y].GetComponent<WorldTile>();
            if (wt != null && wt.walkable == currentWalkableState)
            {
                list.Add(wt);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // draws the lines that represent the path currently selected
        Gizmos.color = Color.blue;
        if (pathData != null && pathData.paths != null)
        {
            if (SelectedPath >= 0 && SelectedPath < pathData.paths.Count)
            {
                for (int i = 0; i < pathData.paths[SelectedPath].Count - 1; i++)
                {
                    Gizmos.DrawLine(pathData.paths[SelectedPath][i].transform.position, pathData.paths[SelectedPath][i + 1].transform.position);
                }
            }
            if (SelectedPath < 0)
                SelectedPath = 0;
            else if (SelectedPath >= (pathData.paths.Count))
                SelectedPath = pathData.paths.Count - 1;
        }
    }

    public void SetSpawnStartPosFromSelected(ref List<WorldTile> permanentSpawnPoints, ref List<GameObject> selectedNodes)
    {
        foreach (GameObject selectedNode in selectedNodes)
        {
            permanentSpawnPoints.Add(selectedNode.GetComponent<WorldTile>());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tileList">HiddenTileMananger object</param>
    /// <param name="tileMap">Tilemap that contains hidden tiles</param>
    public void HideTiles()
    {
        foreach (TransformList tileTransformListObject in hiddenTileManager.list)
        {
            foreach (Transform transformPos in tileTransformListObject.listOfNodes)
            {
                //Remove blocked options for tile, default is Lock Colour
                //transformPos.gameObject.SetActive(false);

                Vector3Int temp = new Vector3Int((int)transformPos.position.x, (int)transformPos.position.y, (int)transformPos.position.z);

                SetTileColor(hiddenTileMap.WorldToCell(temp), new Color(0, 0, 0, 1), hiddenTileMap);
                transformPos.gameObject.SetActive(false);

            }
        }
        //BuildTable();
    }

    /// <summary>
    /// Reveal hidden area
    /// <para>
    /// Loop through each TransformList class to get required properties
    /// </para>
    /// </summary>
    public void ShowTiles()
    {
        foreach (TransformList tileTransformListObject in hiddenTileManager.list)
        {
            foreach (Transform transformPos in tileTransformListObject.listOfNodes)
            {
                //transformPos.gameObject.SetActive(true);

                Vector3Int temp = new Vector3Int((int)transformPos.position.x, (int)transformPos.position.y, (int)transformPos.position.z);
                SetTileColor(hiddenTileMap.WorldToCell(temp), new Color(0, 0, 0, 0), hiddenTileMap);
                transformPos.gameObject.SetActive(true);
            }
        }
        //BuildTable();
    }

    /// <summary>
    /// Reveal hidden area
    /// <para>
    /// Loop through each TransformList class to get required properties
    /// </para>
    /// </summary>
    /// <param name="destructableTile">Node (associates with tile) that will reveal hidden area</param>

    public void ShowTiles(Transform destructableTile)
    {
        foreach (TransformList tileTransformListObject in hiddenTileManager.list)
        {
            if (tileTransformListObject.breakableBlockPos == destructableTile)
            {
                foreach (Transform transformPos in tileTransformListObject.listOfNodes)
                {
                    Vector3Int temp = new Vector3Int((int)transformPos.position.x, (int)transformPos.position.y, (int)transformPos.position.z);
                    SetTileColor(hiddenTileMap.WorldToCell(temp), new Color(0, 0, 0, 0), hiddenTileMap);
                    //transformPos.gameObject.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Set tile color as black to hide tiles and white to show tile
    /// </summary>
    /// <param name="tileCellPosition">Position of tile in tile map</param>
    /// <param name="color">Color of tile to be set</param>
    /// <param name="tileMap">Tilemap that contains hidden tiles</param>
    private void SetTileColor(Vector3Int tileCellPosition, Color color, Tilemap tileMap)
    {
        tileMap.SetTileFlags(tileCellPosition, TileFlags.None);
        tileMap.SetColor(tileCellPosition, color);
    }
}
