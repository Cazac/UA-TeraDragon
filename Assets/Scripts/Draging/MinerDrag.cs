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

    //Permitted Tiles
    public TileBase[] validTileTypes;

    /////////////////////////////////////////////////////////////////


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


        currentMiner.transform.position = cursorPosition;
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
        RaycastHit hit;


        //Check Raycast for any hit with COLLIDERS
        if (Physics.Raycast(raycastMouse, out hit, Mathf.Infinity))
        {
            //Check hit tile name

            string tileTypeName = hit.collider.gameObject.name;
            print("Is not null:" + hit.collider.GetComponent<CrystalTile>() != null);
            if (hit.collider.GetComponent<CrystalTile>() != null && hit.collider.GetComponent<CrystalTile>().towering)
            {
                //Leave the Miner on the node, Call spawner later for init
                currentMiner.transform.position = hit.collider.gameObject.transform.position;
                LogRaycasthitObject(hit.collider.gameObject.transform.position.ToString(), hit.collider.gameObject.transform.parent.gameObject.name);

                currentMiner.GetComponent<MinerScript>().level = 1;
                currentMiner.GetComponent<MinerScript>().crystalTile = hit.collider.GetComponent<CrystalTile>();
                hit.collider.GetComponent<WorldTile>().towering = false;
                

                return;

            }
            else
            {
                //No Match
                print("Destroy Miner");
                Destroy(currentMiner);

            }

        }
        else
        {
            //No Raycast
            print("Destroy Miner");
            Destroy(currentMiner);
        }
    }


    ///////////////
    /// <summary>
    /// Undocumented
    /// </summary>
    ///////////////
    private void LogRaycasthitObject(String position, String type)
    {
        String logString = String.Format("Hit node spawing tower at position: {0}, is type of: {1}", position, type);
        Debug.Log(logString);
    }

    /////////////////////////////////////////////////////////////////
}
