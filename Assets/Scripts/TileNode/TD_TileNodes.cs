using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TD_TileNodes : MonoBehaviour {
    //did some stuff to the actions in npc so they can get closer to the Nodes without the glitchyness

    //changed execution order for this and world builder
    public Grid gridBase;
    float mapConstant;
    // 0 == Road, 1 == Ground
     List<Tilemap> tileMapFloorList;
    //floor of world
    public List<Tilemap> obstacleLayers;
    //all layers that contain objects to navigate around

    [Header("List of node for tilemap")]
    public GameObject[] nodePrefabs;
    public GameObject nodePrefab_Unwalkable;

    [Header("Selected node")]
    [SerializeField]
    private List<GameObject> selectedNode = new List<GameObject>();
    public List<GameObject> SelectedNode { get => selectedNode; set => selectedNode = value; }

    [Header("Scan coordinate for tilemap")]
    //these are the bounds of where we are searching in the world for tiles, have to use world coords to check for tiles in the tile map
    public int scanStartingPoint_X = -250;
    public int scanStartingPoint_Y = -250;
    public int scanFinishPoint_X = 250;
    public int scanFinishPoint_Y = 250;

    //all the nodes in the world
    public List<GameObject> unsortedNodes;

    //sorted 2d array of nodes, may contain null entries if the map is of an odd shape e.g. gaps
    public GameObject[,] nodes;
    public List<WorldTile> permanentSpawnPoints;

    //Actual Calulated Grid Bounds Found From Searching
    public int gridBoundX = 10;
    public int gridBoundY = 10;

    public int unwalkableNodeBorder = 1;


    //////////////////////////////////////////////////////////

    private void Awake() {
        //Set List
        tileMapFloorList = new List<Tilemap>();
        foreach (Transform t in gridBase.transform) {
            Debug.Log(t.name);
            tileMapFloorList.Add(t.GetComponent<Tilemap>());
        }
        unsortedNodes = new List<GameObject>();
        mapConstant = gridBase.cellSize.x;
        generateNodes();
    }

    public List<List<WorldTile>> paths = new List<List<WorldTile>>();
    private void Start() {

        //Start it all
        paths = PathFinding.GetPaths(nodes, permanentSpawnPoints);

        Debug.Log("Paths numbers: " + paths.Count);

        foreach (List<WorldTile> list in paths) {

        string s = "";
            for (int i = 0; i < list.Count; i++) {
                s = s + "->(" + list[i].gridX + "," + list[i].gridY + ")";
            }
         Debug.Log(s);
        }

     //   Debug.Log("Num of permanent spawn pts:" + permanentSpawnPoints.Count);
    }
    
    
    public void generateNodes() {
        int tableX = 0, tableY = 0;
        for (int i = 0; i < tileMapFloorList.Count; i++) {
            tileMapFloorList[i].CompressBounds();
            if (tileMapFloorList[i].cellBounds.size.x  > tableX) {
                tableX = tileMapFloorList[i].cellBounds.size.x;
            }
            if (tileMapFloorList[i].cellBounds.size.y > tableY) {
                tableY = tileMapFloorList[i].cellBounds.size.y;
            }
        }
        Debug.Log("Table:" + tableX + " " + tableY);
        nodes = new GameObject[tableX, tableY];
        LoopThroughFloorList(tileMapFloorList, nodePrefabs);
    }
    
    private void LoopThroughFloorList(List<Tilemap> floorList, GameObject[] nodePrefabs) {
        if (floorList.Count != nodePrefabs.Length) {
            Debug.LogError("Number of node does not match number of floor");
            return;
        }
        for (int i = 0; i < floorList.Count; i++) {
            createNodes(floorList[i], nodePrefabs[i], i);
        }
        FillNodeTable();
        SetNeigbours();
    }


    ///<summary>
    ///Main method to handle node creation
    ///</summary>
    /// <param name="tileMapFloor">Tilemap that will spawn in game</param>
    /// <param name="nodePrefab">Node that corresponds to the tileMapFloor</param>
    private void createNodes(Tilemap tileMapFloor, GameObject nodePrefab, int i) {
        //use these to work out the size and where each node should be in the 2d array we'll use to store our nodes so we can work out neighbours and get paths

        WorldTile wt;

        GameObject parentNode = new GameObject("Parent_" + tileMapFloor.name);
        tileMapFloorList[i].CompressBounds();
        BoundsInt bounds = tileMapFloorList[i].cellBounds;


        //scan tiles and create nodes based on where they are
        int GridX = 0; int GirdY = 0;

        for (int x = -(nodes.GetLength(0)) / 2 - 1; x < nodes.GetLength(0) / 2 + 1; x++) {
            for (int y = -(nodes.GetLength(1)) / 2 - 1; y < nodes.GetLength(1) / 2 + 1; y++) {

                TileBase tb = tileMapFloor.GetTile(new Vector3Int(x, y, 0)); //check if we have a floor tile at that world coords

                if (tb != null) {

                    float mapConstant = tileMapFloorList[0].cellSize.x;

                    Vector3 nodePosition = new Vector3(mapConstant / 2 + ((x + gridBase.transform.position.x) * mapConstant), ((y + 0.5f + gridBase.transform.position.y) * mapConstant), 0);
                    Quaternion nodeRotation = Quaternion.Euler(0, 0, 0);


                    GameObject node = Instantiate(nodePrefab, nodePosition, Quaternion.identity, parentNode.transform);

                    wt = node.GetComponent<WorldTile>();
                    wt.gridX = GridX;
                    wt.gridY = GirdY;
                    unsortedNodes.Add(node);

                    if (tileMapFloor.name == "Spawns") {
                        permanentSpawnPoints.Add(wt);
                    }

                }
                GirdY++;
            }
            GirdY = 0; ;
            GridX++;
        }
        
    }

    void FillNodeTable() {

        int minX, minY;
        minX = nodes.GetLength(0); minY = nodes.GetLength(1);
        WorldTile wt;
        foreach (GameObject g in unsortedNodes) {
            wt = g.GetComponent<WorldTile>();
            if (wt.gridX < minX)
                minX = wt.gridX;
            if (wt.gridY < minY)
                minY = wt.gridY;
        }
        // makes sure grid is correctly alligned
        foreach (GameObject g in unsortedNodes) {
            wt = g.GetComponent<WorldTile>();
            wt.gridX -= minX;
            wt.gridY -= minY;
            wt.name = "NODE " + wt.gridX.ToString() + " : " + wt.gridY.ToString();
              nodes[wt.gridX, wt.gridY] = g;
        }
        unsortedNodes.Clear();
        
    }

    private void SetNeigbours() {

        WorldTile wt;
        for (int x = 0; x < nodes.GetLength(0); x++) {
            for (int y = 0; y < nodes.GetLength(1); y++) {
                if (nodes[x, y] != null) {
                    wt = nodes[x, y].GetComponent<WorldTile>();
                    wt.myNeighbours = getNeighbours(x, y, nodes.GetLength(0), nodes.GetLength(1), wt.walkable);
                }
            }
        }
    }

    private List<WorldTile> getNeighbours(int x, int y, int width, int height, bool walkable) {
        List<WorldTile> myNeighbours = new List<WorldTile>();
        if (x < 0 || x >= width || y < 0 || y >= height)
            return myNeighbours;

        if (x > 0) {
            AddNodeToList(myNeighbours, x - 1, y, walkable);
        }
        if (x < width - 1) {
            AddNodeToList(myNeighbours, x + 1, y, walkable);
        }
        if (y > 0) {
            AddNodeToList(myNeighbours, x, y - 1, walkable);
        }
        if (y < height - 1) {
            AddNodeToList(myNeighbours, x, y + 1, walkable);
        }

        return myNeighbours;
    }

    void AddNodeToList(List<WorldTile> list, int x, int y, bool currentWalkableState) {
        if (nodes[x, y] != null) {
            WorldTile wt = nodes[x, y].GetComponent<WorldTile>();
            if (wt != null && wt.walkable == currentWalkableState) {
                list.Add(wt);
            }
        }
    }

}
