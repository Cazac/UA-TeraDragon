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

    //////////////////////////////////////////////////////////


    private void Start()
    {
        tileNodes = GetComponent<TileNodes>();

        //                                      Is this bad form to use with getter / setter?
        uniqueTilemap = tileNodes.uniqueTilemap;
        nodes = tileNodes.nodes;


        GameObject worldTile_GO = nodes[0, 0];


        //string name = uniqueTilemap.GetTile(uniqueTilemap.WorldToCell(nodePosition)).name;

        //TileBase tileBase = uniqueTilemap.GetTile(new Vector3(worldTile_GO.transform.position));



        //    Change Sprite
        //tileData.sprite = newSprite;


        RefreshMap();
    }

    //////////////////////////////////////////////////////////


    public void RefreshMap()
    {
        if (uniqueTilemap != null)
        {
            uniqueTilemap.RefreshAllTiles();
        }
    }


    public Sprite Spring;
    public Sprite Summer;
    public Sprite Fall;
    public Sprite Winter;

    //////////////////////////////////////////////////////////

    
}
