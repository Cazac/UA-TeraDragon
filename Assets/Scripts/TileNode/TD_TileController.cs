using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

///////////////
/// <summary>
///     
/// TD_TileSpawner is used to spawn an keep track of all nodes attached to the tilemap layers
/// 
/// </summary>
///////////////

public class TD_TileController : MonoBehaviour
{

    [Header("Main Grid")]
    public Grid gridBase;

    [Header("List of Nodes Types")]
    public GameObject[] nodePrefabs;
    public GameObject nodePrefab_Unwalkable;

    [Header("Monster Prefabs")]
    public GameObject enemyPrefab;

    //  TO DO   // - Legacy?
    [Header("Selected Nodes")]
    [SerializeField]
    private List<GameObject> selectedNode = new List<GameObject>();
    public List<GameObject> SelectedNode { get => selectedNode; set => selectedNode = value; }

    //  TO DO   // - Set as private?
    [Header("Scanning coordinates for tilemap")]
    public int scanStartingPoint_X = -100;
    public int scanStartingPoint_Y = -100;
    public int scanFinishPoint_X = 100;
    public int scanFinishPoint_Y = 100;

    //Auto set to size of the tilemap tiles
    private float mapConstant;

    //  TO DO   // - Remove?
    //Auto calculated grid bound found after searching
    private int gridBoundX;
    private int gridBoundY;

    //  TO DO   // - Remove?
    // ????
    public int arrayOffsetSize_X = 10;
    public int arrayOffsetSize_Y = 6;

    public PathsData pathData;

    public List<Tilemap> tileMapFloorList;
    public List<GameObject> unsortedNodes;
    public List<WorldTile> permanentSpawnPoints;

    private float timer = 0;
    private bool testBool = false;

    //Sorted 2D array of nodes, world is permenant storage
    public GameObject[,] nodes;
    public GameObject[,] worldNodes;

    //////////////////////////////////////////////////////////

    private void Awake()
    {
        tileMapFloorList = new List<Tilemap>();
        unsortedNodes = new List<GameObject>();
        mapConstant = gridBase.cellSize.x;
        //worldNodes = new GameObject[300, 300];

        foreach (Transform transform in gridBase.transform)
        {
            tileMapFloorList.Add(transform.GetComponent<Tilemap>());
        }

        generateNodes();
    }

    private void Start()
    {
        generatePathing();
    }

    private void Update()
    {
        ///////////  for testing  //////////////////////
        timer += Time.deltaTime;
        if (timer > 1f && !testBool)
        {
            foreach (WorldTile wt2 in pathData.PathsByEnd.Keys)
            {

                GameObject go = Instantiate(enemyPrefab, pathData.PathsByEnd[wt2][0][0].transform.position, new Quaternion());
                EnemyScript enemy = go.GetComponent<EnemyScript>();
                enemy.waypoints = pathData.PathsByEnd[wt2][0];

            }

            testBool = true;
        }
        /////////////////////////////////////////////////
    }


    ///////////////
    /// <summary>
    /// Compress the bounds of the map then generate all of the nodes attached to each layer
    /// </summary>
    ///////////////
    public void generateNodes()
    {
        int tableX = 0, tableY = 0;

        for (int i = 0; i < tileMapFloorList.Count; i++)
        {
            tileMapFloorList[i].CompressBounds();

            if (tileMapFloorList[i].cellBounds.size.x > tableX)
            {
                tableX = tileMapFloorList[i].cellBounds.size.x;
            }
            if (tileMapFloorList[i].cellBounds.size.y > tableY)
            {
                tableY = tileMapFloorList[i].cellBounds.size.y;
            }
        }

        nodes = new GameObject[tableX, tableY];
        LoopThroughFloorList(tileMapFloorList, nodePrefabs);

        print("Here");
        return;
    }


    ///////////////
    /// <summary>
    /// Grab the pathing route for enemy spawns and spawn creatures
    /// </summary>
    ///////////////
    public void generatePathing()
    {
        //Grab the enemy pathing route
        pathData = PathFinding.GetPaths(nodes, permanentSpawnPoints);

        Debug.Log("Paths numbers: " + pathData.paths.Count);
        Debug.Log("Nodes length:" + nodes.GetLength(0) + " " + nodes.GetLength(1));

        print("Here");
        return;

        foreach (WorldTile wt in pathData.PathsByStart.Keys)
        {
            GameObject enemyGO = Instantiate(enemyPrefab, wt.transform.position, Quaternion.identity);
            EnemyScript enemy = enemyGO.GetComponent<EnemyScript>();
            enemy.waypoints = pathData.PathsByStart[wt][0];
        }

        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                if (tileMapFloorList[0].GetTile(new Vector3Int(i, j, 0)) != null)
                {
                    Debug.Log(tileMapFloorList[0].GetTile(new Vector3Int(i, j, 0)).name);
                }
            }
        }



        /*
         
        ///
        /// This is code that couls allow us to use only 1 tilemap instead of multiple ones. 
        ///
        if (nodes != null)
        {

            for (int i = -(nodes.GetLength(0)) - 1; i < nodes.GetLength(0) + 1; i++)
            {
                for (int j = -(nodes.GetLength(1)) - 1; j < nodes.GetLength(1) + 1; j++)
                {
                    Vector3 nodePosition = new Vector3(mapConstant / 2 + ((i + gridBase.transform.position.x) * mapConstant),
                        (j + 0.5f + gridBase.transform.position.y) * mapConstant, 0);
                    //    Debug.Log(mapConstant / 2 + ((i + gridBase.transform.position.x) * mapConstant) + ", " + (j + 0.5f + gridBase.transform.position.y) * mapConstant);

                    //Debug.Log("Tile:" + nodePosition.x + " " + nodePosition.y + ", There is tile:" +
                    //    (tileMapFloorList[1].GetTile(tileMapFloorList[1].WorldToCell(nodePosition)) != null ));
                    //;

                    if (tileMapFloorList[1].GetTile(tileMapFloorList[1].WorldToCell(nodePosition)) != null)
                    {
                        Debug.Log("Tile:" + tileMapFloorList[1].GetTile(tileMapFloorList[1].WorldToCell(nodePosition)).name);
                    }
                }
            }
        }

        */
    }


    ///////////////
    /// <summary>
    /// Loop through all available floor in list, which will then instantiate node accordingly
    /// </summary>
    ///     <param name="floorList"> List of tilemap to iterate through</param>
    ///     <param name="nodePrefabs"> List of node correspond to suitable floor</param>
    ///////////////
    private void LoopThroughFloorList(List<Tilemap> floorList, GameObject[] nodePrefabs)
    {
        if (floorList.Count != nodePrefabs.Length)
        {
            Debug.LogError("Number of node does not match number of floor");
            return;
        }

        for (int i = 0; i < floorList.Count; i++)
        {
            createNodes2(floorList[i], nodePrefabs[i], i);
        }

        FillNodeTable();
        SetNeigbours();
    }


    ///////////////
    /// <summary>
    /// Undocumented
    /// </summary>
    ///////////////
    private void FillNodeTable()
    {
        int minX = nodes.GetLength(0);
        int minY = nodes.GetLength(1);
        WorldTile wt;

        foreach (GameObject g in unsortedNodes)
        {
            wt = g.GetComponent<WorldTile>();

            if (wt.gridX < minX)
            {
                minX = wt.gridX;
            }
            if (wt.gridY < minY)
            {
                minY = wt.gridY;
            }
        }

        //Makes sure grid is correctly alligned
        foreach (GameObject g in unsortedNodes)
        {
            wt = g.GetComponent<WorldTile>();
            wt.gridX -= minX;
            wt.gridY -= minY;
            wt.name = "NODE " + wt.gridX.ToString() + " : " + wt.gridY.ToString();
            nodes[wt.gridX, wt.gridY] = g;
        }

        unsortedNodes.Clear();
    }

    ///////////////
    /// <summary>
    /// Main method to handle node creation and placement
    /// </summary>
    ///     <param name="tileMapFloor"> Tilemap that will spawn in game</param>
    ///     <param name="nodePrefab"> Node that corresponds to the tileMapFloor</param>
    ///////////////
    private void createNodes(Tilemap tileMapFloor, GameObject nodePrefab)
    {
        //use these to work out the size and where each node should be in the 2d array we'll use to store our nodes so we can work out neighbours and get paths
        int gridX = 0;
        int gridY = 0;

        //Bool for finding a tile so that you may increment the X grid size
        bool foundTileOnLastPass = false;

        //Create new parent for currrnt tilemap nodes
        GameObject parentNode = new GameObject("TileParent_" + tileMapFloor.name);

        ////////////////////////////////

        //Scan though tiles on X coordinates
        for (int x = scanStartingPoint_X; x < scanFinishPoint_X; x++)
        {
            //Scan though tiles on Y coordinates
            for (int y = scanStartingPoint_Y; y < scanFinishPoint_Y; y++)
            {
                //Get the tilebase for the current iteration
                TileBase tile = tileMapFloor.GetTile(new Vector3Int(x, y, 0));

                //Check if current tile exists
                if (tile == null)
                {
                    //No tile, skip
                    continue;
                }

                ////////////////////////////////

                //Reset obstacle bool
                bool foundObstacle = false;

       

                ////////////////////////////////

                //if we have found an obstacle then we do the same but make the node unwalkable
                if (foundObstacle)
                {
                    Debug.Log("Found Unwalkable - Obstacle Layer?");

                    //Generate new node position and rotation
                    Vector3 nodePosition = new Vector3(mapConstant + ((x + gridBase.transform.position.x) * mapConstant), (mapConstant / 2) + ((y + 0.5f + gridBase.transform.position.y) * mapConstant), 0);

                    //Instatiate new node
                    GameObject node = Instantiate(nodePrefab, nodePosition, Quaternion.identity, parentNode.transform);

                    //Get WorldTile script for tile
                    WorldTile wt = node.GetComponent<WorldTile>();

                    //Set array values
                    wt.posX = x + arrayOffsetSize_X;
                    wt.posY = y + arrayOffsetSize_Y;

                    //Set positional values
                    wt.gridX = gridX;
                    wt.gridY = gridY;

                    //Set walkablke value
                    wt.walkable = false;

                    //Set bool so we know to increment the index counters
                    foundTileOnLastPass = true;

                    //What to do with list ???
                    unsortedNodes.Add(node);

                    //Set new name with coordinates
                    node.name = "UNWALKABLE NODE " + x.ToString() + " : " + y.ToString();

                    //Add to array
                    worldNodes[wt.posX, wt.posY] = node;
                }
                else
                {
                    //Generate new node position and rotation
                    Vector3 nodePosition = new Vector3(mapConstant + ((x + gridBase.transform.position.x) * mapConstant), (mapConstant / 2) + ((y + 0.5f + gridBase.transform.position.y) * mapConstant), 0);

                    //Instatiate new node
                    GameObject node = Instantiate(nodePrefab, nodePosition, Quaternion.identity, parentNode.transform);

                    //Get WorldTile script for tile
                    WorldTile wt = node.GetComponent<WorldTile>();

                    //Set array values
                    wt.posX = x + arrayOffsetSize_X;
                    wt.posY = y + arrayOffsetSize_Y;

                    //Set positional values
                    wt.gridX = gridX;
                    wt.gridY = gridY;

                    //Set bool so we know to increment the index counters
                    foundTileOnLastPass = true;

                    //What to do with list ???
                    unsortedNodes.Add(node);

                    //Set new name with coordinates
                    node.name = "NODE " + wt.posX.ToString() + " : " + wt.posY.ToString();

                    //Add to array
                    worldNodes[wt.posX, wt.posY] = node;
                }

                ////////////////////////////////

                //Increment Y grid size
                gridY++; 

                //Check for new max X size
                if (gridX > gridBoundX)
                { 
                    gridBoundX = gridX;
                }

                //Check for new max Y size
                if (gridY > gridBoundY)
                {
                    gridBoundY = gridY;
                }
            }

            //since the grid is going from bottom to top on the Y axis on each iteration of the inside loop, if we have found tiles on this iteration we increment the gridX value and reset the y value
            if (foundTileOnLastPass == true)
            {
                //Increment X grid size and reset Y grid size
                gridX++;
                gridY = 0;

                //Reset bool
                foundTileOnLastPass = false;
            }
        }

        ////////////////////////////////

        //Initialise with the new found bounds the 2D array that will store our nodes in their tilemap oriented position
        nodes = new GameObject[gridBoundX + 1, gridBoundY + 1];

        //Go through the unsorted list of nodes and put them into the 2d array sorted
        foreach (GameObject node_GO in unsortedNodes)
        { 
            //Get WorldTile script from gameobject
            WorldTile wt = node_GO.GetComponent<WorldTile>();

            //Set gameobject to array
            nodes[wt.gridX, wt.gridY] = node_GO;
        }

        //Assign pathing neighbours to nodes
        for (int x = 0; x < gridBoundX; x++)
        { 
            //Go through the 2d array and assign the neighbours of each node
            for (int y = 0; y < gridBoundY; y++)
            {
                //Check if the coords in the array contain a node
                if (nodes[x, y] != null)
                {
                    //if they do then assign the neighbours
                    WorldTile wt = nodes[x, y].GetComponent<WorldTile>();

                    //if (wt.walkable == true) {
                    wt.myNeighbours = getNeighbours(x, y, gridBoundX, gridBoundY, true);
                    //}
                }
            }
        }
    }


    ///<summary>
    ///Main method to handle node creation
    ///</summary>
    /// <param name="tileMapFloor">Tilemap that will spawn in game</param>
    /// <param name="nodePrefab">Node that corresponds to the tileMapFloor</param>
    private void createNodes2(Tilemap tileMapFloor, GameObject nodePrefab, int i)
    {
        //use these to work out the size and where each node should be in the 2d array we'll use to store our nodes so we can work out neighbours and get paths

   

        GameObject parentNode = new GameObject("Parent_" + tileMapFloor.name);

        tileMapFloorList[i].CompressBounds();
        BoundsInt bounds = tileMapFloorList[i].cellBounds;


        //scan tiles and create nodes based on where they are
        int GridX = 0;
        int GirdY = 0;


        for (int x = -(nodes.GetLength(0)) - 1; x < nodes.GetLength(0) + 1; x++)
        {
            for (int y = -(nodes.GetLength(1)) - 1; y < nodes.GetLength(1) + 1; y++)
            {
                TileBase tileBase = tileMapFloor.GetTile(new Vector3Int(x, y, 0));

                if (tileBase != null)
                {

                    float mapConstant = tileMapFloorList[0].cellSize.x;

                    print(mapConstant);

                    Vector3 nodePosition = new Vector3(mapConstant / 2 + ((x + gridBase.transform.position.x) * mapConstant), ((y + 0.5f + gridBase.transform.position.y) * mapConstant), 0);
                    Quaternion nodeRotation = Quaternion.Euler(0, 0, 0);

                    GameObject node = Instantiate(nodePrefab, nodePosition, Quaternion.identity, parentNode.transform);

                    WorldTile wt = node.GetComponent<WorldTile>();
                    wt.gridX = GridX;
                    wt.gridY = GirdY;
                    unsortedNodes.Add(node);

                    if (tileMapFloor.name == "Spawns")
                    {
                        permanentSpawnPoints.Add(wt);
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
    /// For each tile in nodes[] add the 4 surrounding tiles to WorldTile myNeighbours
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
                    wt.myNeighbours = getNeighbours(x, y, nodes.GetLength(0), nodes.GetLength(1), wt.walkable);
                }
            }
        }
    }


    ///////////////
    /// <summary>
    /// Grab 4 surrounding tiles (N, E, S, W) from all tilemaps and return them
    /// </summary>
    ///////////////
    private List<WorldTile> getNeighbours(int x, int y, int width, int height, bool walkable)
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
    /// Error check each node before adding to the WorldTile neighbours list
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

    //////////////////////////////////////////////////////////
}
