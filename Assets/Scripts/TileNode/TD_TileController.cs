using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

///////////////
/// <summary>
///     
/// TD_TileSpawner is used to manage all of the button inputs
/// 
/// </summary>
///////////////

public class TD_TileController : MonoBehaviour
{
    ////////////////////////////////

    //changed execution order for this and world builder
    public Grid gridBase;
    public Tilemap[] tileMapFloorList;

    //floor of world
    public List<Tilemap> obstacleLayers;

    //all layers that contain objects to navigate around
    public GameObject[] nodePrefabs;
    public GameObject nodePrefab_Unwalkable;

    ////////////////////////////////

    //these are the bounds of where we are searching in the world for tiles, have to use world coords to check for tiles in the tile map
    public int scanStartingPoint_X = -100;
    public int scanStartingPoint_Y = -100;
    public int scanFinishPoint_X = 100;
    public int scanFinishPoint_Y = 100;

    //Actual calculated grid bound found after searching
    public int gridBoundX = 0;
    public int gridBoundY = 0;

    //Tilemap scale size, used for node placement
    public float mapConstant = 11.2f;

    public int arrayOffsetSize_X = 10;
    public int arrayOffsetSize_Y = 6;

    ////////////////////////////////

    //all the nodes in the world
    public List<GameObject> unsortedNodes;

    //Sorted 2D array of nodes reset per Tilemap
    public GameObject[,] nodes;

    //Sorted 2D final array of nodes with perm storage
    public GameObject[,] worldNodes;

    ////////////////////////////////////////////////////////// - Mono

    private void Awake()
    {
        //Set a new List
        unsortedNodes = new List<GameObject>();
        worldNodes = new GameObject[100, 100];
    }

    private void Start()
    {
        //Generate all of the nodes attached to the map
        generateNodes();
    }

    //////////////////////////////////////////////////////////

    ///////////////
    /// <summary>
    /// Just call this and plug the resulting 2d array of nodes into your own A* algorithm
    /// </summary>
    ///////////////
    public void generateNodes()
    {
        //Loop each floor
        LoopThroughFloorList(tileMapFloorList, nodePrefabs);
    }


    ///////////////
    /// <summary>
    /// Loop through all available floor in list, which will then instantiate node accordingly
    /// </summary>
    ///     <param name="floorList"> List of tilemap to iterate through</param>
    ///     <param name="nodePrefabs"> List of node correspond to suitable floor</param>
    ///////////////
    private void LoopThroughFloorList(Tilemap[] floorList, GameObject[] nodePrefabs)
    {
        //Check for linked amount of floors to match node prefabs
        if (floorList.Length > nodePrefabs.Length || floorList.Length < nodePrefabs.Length)
        {
            Debug.LogError("Number of node does not match number of floor");
            return;
        }

        //Create nodes for each tilemap
        for (int i = 0; i < floorList.Length; i++)
        {
            createNodes(floorList[i], nodePrefabs[i]);
        }
    }


    ///////////////
    /// <summary>
    /// Main method to handle node creation
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

                //BETTER WAY TO DO THIS ???
                //There are no obstacle layers set so this is never called      
                foreach (Tilemap t in obstacleLayers)
                {
                    //Get the same tile but from the obstacle layer
                    TileBase tile_2 = t.GetTile(new Vector3Int(x, y, 0));

                    //Check current tile against obstacle tilemap layer
                    if (tile_2 != null)
                    {
                        foundObstacle = true;
                    }
                }

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
            WorldTile worldtile = node_GO.GetComponent<WorldTile>();

            //Set gameobject to array
            nodes[worldtile.gridX, worldtile.gridY] = node_GO;
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
                    wt.myNeighbours = getNeighbours(x, y, gridBoundX, gridBoundY);
                    //}
                }
            }
        }
    }


    ///////////////
    /// <summary>
    /// Undocumented - Gets neighbours of a tile at x/y in a specific tilemap, can also have a border
    /// </summary>
    //////////////////
    public List<TileBase> getNeighbouringTiles(int x, int y, Tilemap t)
    {
        List<TileBase> retVal = new List<TileBase>();

        for (int i = x; i < x; i++)
        {
            for (int j = y; j < y; j++)
            {
                TileBase tile = t.GetTile(new Vector3Int(i, j, 0));
                if (tile != null)
                {
                    retVal.Add(tile);
                }
            }
        }
        return retVal;
    }


    ///////////////
    /// <summary>
    /// Undocumented - useful for pathing?
    /// </summary>
    ///////////////
    public List<WorldTile> getNeighbours(int x, int y, int width, int height)
    {
        List<WorldTile> myNeighbours = new List<WorldTile>();
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return myNeighbours;
        }

        //needs the width & height to work out if a tile is not on the edge, also needs to check if the nodes is null due to the accounting for odd shapes
        if (x > 0 && x < width - 1)
        {
            //can get tiles on both left and right of the tile

            AddNodeToList(myNeighbours, x - 1, y);
            AddNodeToList(myNeighbours, x + 1, y);
            if (y > 0)
            { //just top
                AddNodeToList(myNeighbours, x, y - 1);
            }
            if (y < height - 1)
            { //just bottom
                AddNodeToList(myNeighbours, x, y + 1);
            }
        }
        else if (x == 0)
        {
            AddNodeToList(myNeighbours, x + 1, y);
            //can't get tile on left
            if (y > 0)
            { //just top
                AddNodeToList(myNeighbours, x, y - 1);
            }
            if (y < height - 1)
            { //just bottom
                AddNodeToList(myNeighbours, x, y + 1);
            }
        }
        else if (x == width - 1)
        {
            AddNodeToList(myNeighbours, x - 1, y);
            //can't get tile on right
            if (y > 0)
            { //just top
                AddNodeToList(myNeighbours, x, y - 1);
            }
            if (y < height - 1)
            { //just bottom
                AddNodeToList(myNeighbours, x, y + 1);
            }
        }

        return myNeighbours;
    }


    ///////////////
    /// <summary>
    /// Undocumented - Something to do with getNeighbours()
    /// </summary>
    ///////////////
    void AddNodeToList(List<WorldTile> list, int x, int y)
    {
        if (nodes[x, y] != null)
        {
            WorldTile wt = nodes[x, y].GetComponent<WorldTile>();
            if (wt != null)
            {
                list.Add(wt);
            }
        }
    }

    //////////////////////////////////////////////////////////
}
