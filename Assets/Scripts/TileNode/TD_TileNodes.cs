using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TD_TileNodes : MonoBehaviour
{
    //did some stuff to the actions in npc so they can get closer to the Nodes without the glitchyness

    //changed execution order for this and world builder
    public Grid gridBase;
    float mapConstant;

    [Header("List of node for tilemap")]


    [Header("Selected node")]
    [SerializeField]
    private List<GameObject> selectedNode = new List<GameObject>();
    public List<GameObject> SelectedNode { get => selectedNode; set => selectedNode = value; }
    
    //List of nodes before they are sorted
    private List<GameObject> unsortedNodes;

    //sorted 2d array of nodes, may contain null entries if the map is of an odd shape e.g. gaps
    public GameObject[,] nodes;
    public List<WorldTile> permanentSpawnPoints;
    
    public Tilemap uniqueTilemap;

    // nodes that can be placed;
    public GameObject[] TileNodes;
    // this is not the best
    public Tile[] WalkableTiles;
    public Tile[] UnwalkableTiles;
    public Tile[] SpawnTiles;

    public PathsData pathData;

    // Temporary variable
    public GameObject enemyPrefab;

    private void Awake()
    {
        //Set List
        unsortedNodes = new List<GameObject>();
        mapConstant = gridBase.cellSize.x;

        generateNodes();
    }

    private void Start()
    {

        //Start it all
        Debug.Log("permanentSpawnPoints: " + permanentSpawnPoints.Count);
        pathData = PathFinding.GetPaths(nodes, permanentSpawnPoints);

        Debug.Log("Paths numbers: " + pathData.paths.Count);
        Debug.Log("Nodes length:" + nodes.GetLength(0) + " " + nodes.GetLength(1));


        foreach (WorldTile wt in pathData.PathsByStart.Keys)
        {
            GameObject go = Instantiate(enemyPrefab, wt.transform.position, new Quaternion());
            EnemyScript enemy = go.GetComponent<EnemyScript>();
            enemy.waypoints = pathData.PathsByStart[wt][0];

        }
        
    }


    float timer = 0;
    bool testBool = false;
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


    public void generateNodes()
    {
        uniqueTilemap.CompressBounds();
        BoundsInt bounds = uniqueTilemap.cellBounds;
        int tableX = uniqueTilemap.cellBounds.size.x;
        int tableY = uniqueTilemap.cellBounds.size.y;
        
        nodes = new GameObject[tableX, tableY];

        LoopThroughTileset();
        FillNodeTable();
        SetNeigbours();
    }
    
    void LoopThroughTileset()
    {
        WorldTile wt;
        GameObject[] parentNodes = new GameObject[TileNodes.Length];
        parentNodes[0] = new GameObject("Parent_WalkableTiles");
        parentNodes[1] = new GameObject("Parent_UnwalkableTiles");


        int GridX = 0; int GirdY = 0;
        for (int x = -(nodes.GetLength(0)) - 1; x < nodes.GetLength(0) + 1; x++)
        {
            for (int y = -(nodes.GetLength(1)) - 1; y < nodes.GetLength(1) + 1; y++)
            {
                TileBase tb = uniqueTilemap.GetTile(new Vector3Int(x, y, 0)); //check if we have a floor tile at that world coords

                if (tb != null)
                {
                    Vector3 nodePosition = new Vector3(mapConstant / 2 + ((x + gridBase.transform.position.x) * mapConstant), ((y + 0.5f + gridBase.transform.position.y) * mapConstant), 0);
                    Quaternion nodeRotation = Quaternion.Euler(0, 0, 0);

                    GameObject node = null;
                    
                    string name = uniqueTilemap.GetTile(uniqueTilemap.WorldToCell(nodePosition)).name;

                    foreach (Tile tile in WalkableTiles)
                    {
                        if (name == tile.name)
                        {
                            node = Instantiate(TileNodes[0], nodePosition, Quaternion.identity, parentNodes[0].transform);
                            foreach(Tile spTile in SpawnTiles)
                            {
                                if (name == spTile.name)
                                {
                                    permanentSpawnPoints.Add(node.GetComponent<WorldTile>());
                                }
                            }
                        }
                    }
                    foreach (Tile tile in UnwalkableTiles)
                    {
                        if (name == tile.name)
                        {
                            node = Instantiate(TileNodes[1], nodePosition, Quaternion.identity, parentNodes[1].transform);
                        }
                    }

                    if (node == null)
                    {
                        Debug.LogError(name + " is not registered.");
                    }
                    else
                    {
                        unsortedNodes.Add(node);
                        wt = node.GetComponent<WorldTile>();
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
    
    void FillNodeTable()
    {
        int minX, minY;
        minX = nodes.GetLength(0); minY = nodes.GetLength(1);
        WorldTile wt;
        foreach (GameObject g in unsortedNodes)
        {
            wt = g.GetComponent<WorldTile>();
            if (wt.gridX < minX)
                minX = wt.gridX;
            if (wt.gridY < minY)
                minY = wt.gridY;
        }
        // makes sure grid is correctly alligned
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

    void AddNodeToList(List<WorldTile> list, int x, int y, bool currentWalkableState)
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

    
}
