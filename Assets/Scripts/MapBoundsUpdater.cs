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
    private GameObject[,] nodes;

    TileBase[,] restoreTile;

    int minY = -12;
    int maxY = 10;
    List<int> yValues;

    public TileBase barrierTile;

    public int WaveCounter = 0;

    public List<TileBase> restorationTile;

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

        //Set new wave line TESTING THIS
        StartCoroutine(WaitTime());






        //TileData tileData = new TileData();
        //tileData.sprite = newSprite;
        //    Change Sprite
        //tileBase.GetTileData(testINT, uniqueTilemap, tileData);
       // uniqueTilemap.SetTile(testINT, testbase);
        //uniqueTilemap.SwapTile(tileBase, testbase);



        //Refresh the sprites
        uniqueTilemap.RefreshAllTiles();
    }

    //////////////////////////////////////////////////////////

    public void Setup()
    {
        //Get Script
        tileNodes = TileMap.GetComponent<TileNodes>();

        // Is this bad form to use without getter / setter?         ???
        uniqueTilemap = tileNodes.uniqueTilemap;
        nodes = tileNodes.nodes;

        yValues = new List<int>();

        //Set height values
        while (minY <= maxY)
        {
            yValues.Add(minY);
            minY++;
        }
    }

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

        if (mapBounds.waveID.Length != mapBounds.positionX.Length)
        {
            Debug.LogError("Map Bounds Not Equal");
        }
    }

    //////////////////////////////////////////////////////////

    public IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(5);


        print("New Test Wave");
        WaveCounter++;

        MoveMapBounds();

        yield break;
    }

    public void MoveMapBounds()
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
            restorationTile.Add(tileBase);

            //Set tile at position
            uniqueTilemap.SetTile(tilePosition, barrierTile);
        }
    }

    public void MoveCameraBounds()
    {

    }

    public void RestoreTiles()
    {
        foreach (TileBase tilebase in restorationTile)
        {
            
        }

        if (WaveCounter - 1 <= 0)
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

                //Get tile at position
                TileBase tileBase = uniqueTilemap.GetTile(tilePosition);

                //Save for later
                //restoreTile[pos_X, pos_Y] = tileBase;

                //Set tile at position
                uniqueTilemap.SetTile(tilePosition, barrierTile);
            }
        }

        //reset for next addition
        restorationTile.Clear();
    }

    //////////////////////////////////////////////////////////
}
