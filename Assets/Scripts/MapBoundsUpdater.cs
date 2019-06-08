using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBoundsUpdater : MonoBehaviour
{



    [Header("Main Grid / Tilemap")]



    public Sprite newSprite;


    private Tilemap uniqueTilemap;
    private TileNodes tileNodes;
    private GameObject[,] nodes;



    public TileBase testbase;



    //////////////////////////////////////////////////////////


    private void Start()
    {
        tileNodes = GetComponent<TileNodes>();

        //                                      Is this bad form to use without getter / setter?
        uniqueTilemap = tileNodes.uniqueTilemap;
        nodes = tileNodes.nodes;

        uniqueTilemap.CompressBounds();
        BoundsInt bounds = uniqueTilemap.cellBounds;


        print("Test Start");

        //GameObject worldTile_GO = nodes[0, 0];
        string name = uniqueTilemap.GetTile(new Vector3Int(20, -10, 0)).name;

        TileBase tileBase = uniqueTilemap.GetTile(new Vector3Int(20, -10, 0));

        print(" null? " + tileBase == null);

        //Tile

        print(" Name? " + tileBase.name);




        //TileData tileData = new TileData();


        //tileData.sprite = newSprite;


        Vector3Int testINT = new Vector3Int(20, -10, 0);

        //    Change Sprite
        //tileBase.GetTileData(testINT, uniqueTilemap, tileData);



        uniqueTilemap.SetTile(testINT, testbase);


        //uniqueTilemap.SwapTile(tileBase, testbase);



        //.sprite = newSprite;







        //Refresh the sprites
        uniqueTilemap.RefreshAllTiles();

        //RefreshMap();
    }

    //////////////////////////////////////////////////////////


    public void RefreshMap()
    {
        if (uniqueTilemap != null)
        {
            uniqueTilemap.RefreshAllTiles();
        }
    }




    //////////////////////////////////////////////////////////

    
}
