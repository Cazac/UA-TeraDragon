using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBoundsUpdater : MonoBehaviour
{

    [Header("Map Bounds Data")]
    public MapBound mapBounds;

    [Header("Main Grid / Tilemap")]
    public GameObject TileMap;
    private TileNodes tileNodes;
    private Tilemap uniqueTilemap;

    [Header("Tile Used As a Border")]
    public TileBase barrierTile;

    [Header("Wave System")]
    public GameObject SpawnManager;
    private WaveSystem.WaveManager waveManger;

    [Header("Camera Bounds")]
    public GameObject Camera;
    private CameraPanningCursor cameraPanningCursor;

    //  SET FROM OUTSIDE SOURCE???????
    [Header("Current Wave")]
    public int WaveCounter = 0;

    [Header("Time Per Move")]
    public int time = 0;

    //Map height list
    private List<int> yValues;
    private int minY;
    private int maxY;

    //For reversing the tiles
    private List<TileBase> restorationTiles;

    //////////////////////////////////////////////////////////

    private void Start()
    {
        print("Test Start");

        //Error check
        CheckForMatchingBounds();

        //Setup connectionns
        Setup();

        //Set first pass bounds
        MoveMapBounds();

        //Set new wave line TESTING PURPOSES
        StartCoroutine(WaitTime());

        //Refresh the sprites
        uniqueTilemap.RefreshAllTiles();
    }

    //////////////////////////////////////////////////////////

    ///////////////
    /// <summary>
    /// Setups up all of the lists, refferences and y value sizing.
    /// </summary>
    ///////////////
    public void Setup()
    {
        //Get Scripts       Maybe SKip Getting Gameobjects ???
        tileNodes = TileMap.GetComponent<TileNodes>();
        waveManger = SpawnManager.GetComponent<WaveSystem.WaveManager>();
        cameraPanningCursor = Camera.GetComponent<CameraPanningCursor>();

        // Is this bad form to use without getter / setter?         ???
        uniqueTilemap = tileNodes.uniqueTilemap;

        yValues = new List<int>();
        restorationTiles = new List<TileBase>();

        minY = uniqueTilemap.cellBounds.yMin;
        maxY = uniqueTilemap.cellBounds.yMax - 1;

        //Set height values
        while (minY <= maxY)
        {
            yValues.Add(minY);
            minY++;
        }
    }

    ///////////////
    /// <summary>
    /// Error checking for invalid setup.
    /// </summary>
    ///////////////
    public void CheckForMatchingBounds()
    {
        if (mapBounds == null)
        {
            Debug.LogError("No Map Bounds Gameobject");
        }

        if (mapBounds == null)
        {
            Debug.LogError("No Map Bounds Gameobject");
        }
    }

    //////////////////////////////////////////////////////////

    ///////////////
    /// <summary>
    /// UNDOCUMENTED
    /// </summary>
    ///////////////
    public IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(time);


        WaveCounter++;

        MoveMapBounds();

        if (mapBounds.positionX.Length > WaveCounter)
        {

            StartCoroutine(WaitTime());
        }

        yield break;
    }

    ///////////////
    /// <summary>
    /// Move the map boudaires over to the next X position provided by the MapBound Scriptable Object
    /// </summary>
    ///////////////
    public void MoveMapBounds()
    {
        //Restore old tiles before adding new
        RestoreTiles();

        if (mapBounds.positionX.Length > WaveCounter)
        {
            //Set base X
            int pos_X = mapBounds.positionX[WaveCounter];

            foreach (int pos_Y in yValues)
            {
                //Create position
                Vector3Int tilePosition = new Vector3Int(pos_X, pos_Y, 0);

                //Get tile at position
                TileBase tileBase = uniqueTilemap.GetTile(tilePosition);

                //Save for later
                restorationTiles.Add(tileBase);

                //Set tile at position
                uniqueTilemap.SetTile(tilePosition, barrierTile);
            }

            //Update Camera Bounds
            cameraPanningCursor.SetCameraBounds(mapBounds.positionX[WaveCounter]);
        }
        else
        {
            print("No More Waves Bounds!");
        }
    }

    ///////////////
    /// <summary>
    /// UNDOCUMENTED
    /// </summary>
    ///////////////
    public void MoveCameraBounds()
    {

        //Send Bounds Position X + 1 right


    }

    ///////////////
    /// <summary>
    /// UNDOCUMENTED
    /// </summary>
    ///////////////
    public void RestoreTiles()
    {
        if (WaveCounter - 1 < 0)
        {
            print("No Restoration to be made!");
        }
        else
        {
            //Set base X
            int pos_X = mapBounds.positionX[WaveCounter - 1];

            foreach (int pos_Y in yValues)
            {
                //Create position
                Vector3Int tilePosition = new Vector3Int(pos_X, pos_Y, 0);

                //Set tile at position
                uniqueTilemap.SetTile(tilePosition, restorationTiles[0]);

                //clear last used tile form list
                restorationTiles.RemoveAt(0);
            }
        }

        //reset for next addition
        restorationTiles.Clear();
    }

    //////////////////////////////////////////////////////////
}
