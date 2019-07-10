using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

///////////////
/// <summary>
///     
/// TD_TowerDrag is used to drag and drop all towers into the game onto TD_TowerDrop sockets
/// 
/// </summary>
///////////////

public class MinerDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public GameObject minerPrefab;
    private GameObject currentMiner;
    private CameraPanningCursor cameraPanningCursor;

    //Permitted Tiles To Place a Miner On
    public TileBase[] validTileTypes;

    /////////////////////////////////////////////////////////////////

    private void Start()
    {
        cameraPanningCursor = GameObject.FindObjectOfType<CameraPanningCursor>();
    }

    ///////////////
    /// <summary>
    /// Undocumented
    /// </summary>
    ///////////////
    public void OnBeginDrag(PointerEventData eventData)
    {
        currentMiner = Instantiate(minerPrefab);
    }


    ///////////////
    /// <summary>
    /// Every time the mouse moves, match the new dragged miner to the mouse position
    /// </summary>
    ///////////////
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0;

        //Set position to mouse
        currentMiner.transform.position = cursorPosition;

        //???
        cameraPanningCursor.IsUIDragging = true;
    }


    ///////////////
    /// <summary>
    /// Raycast the tilemap looking for a node under the mouse when the tower is dropped onto the map, validation check the tile then add it to the map. If not valid remove the miner from the cursor.
    /// </summary>
    ///////////////
    public void OnEndDrag(PointerEventData eventData)
    {
        //Get current mouse raycast
        Ray raycastMouse = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Useful?
        cameraPanningCursor.IsUIDragging = false;


        //Check Raycast for any hit with COLLIDERS
        if (Physics.Raycast(raycastMouse, out RaycastHit hit, Mathf.Infinity))
        {
            //Check hit tile name
            string tileTypeName = hit.collider.gameObject.name;

            if (hit.collider.GetComponent<CrystalTile>() != null && hit.collider.GetComponent<CrystalTile>().towering)
            {
                //Leave the Miner on the node, Call spawner later for init
                currentMiner.transform.position = hit.collider.gameObject.transform.position;
                
                currentMiner.GetComponent<MinerScript>().level = 1;
                currentMiner.GetComponent<MinerScript>().crystalTile = hit.collider.GetComponent<CrystalTile>();
                hit.collider.GetComponent<WorldTile>().towering = false;
                return;
            }
            else
            {
                //No Match
                print("No Matching Value, Destroy Miner");
                Destroy(currentMiner);
                return;
            }

        }
        else
        {
            //No Raycast
            print("No Raycast Hit, Destroy Miner");
            Destroy(currentMiner);
            return;
        }
    }




    /////////////////////////////////////////////////////////////////
}
