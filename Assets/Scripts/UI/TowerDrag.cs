using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

///////////////
/// <summary>
///     
/// TD_TowerDrag is used to drag and drop all towers into the game onto TD_TowerDrop sockets
/// 
/// </summary>
///////////////

public class TowerDragOLD : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
   
    public GameObject towerPrefab;
    private GameObject currentTowerLOCAL;
    private TileNodes tileNodes;

    // TO DO ???
    public int tileLayer;

    /////////////////////////////////////////////////////////////////

    private void Start()
    {
        tileNodes = GameObject.FindObjectOfType<TileNodes>();
    }

    ///////////////
    /// <summary>
    /// Undocumented
    /// </summary>
    ///////////////
    public void OnBeginDrag(PointerEventData eventData)
    {
        // TO DO ???
        //Check Money
        //Charge PLyaer
        //Spawn Tower UI
        //Get Reff To Tower
        //Attach to Cursor

        currentTowerLOCAL = Instantiate(towerPrefab);
    }


    ///////////////
    /// <summary>
    /// Undocumented
    /// </summary>
    ///////////////
    public void OnDrag(PointerEventData eventData)
    {
        //currentTowerLOCAL.transform.position = cursor.transform.position;
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0;

        currentTowerLOCAL.transform.position = cursorPosition;
    }


    ///////////////
    /// <summary>
    /// Raycast the tilemap looking for a node under the mouse when the tower is dropped onto the map, validation check the tile then add it to the map. If not valid remove the tower from the cursor.
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
            string tileLayer = "";


            if (hit.collider.gameObject.transform.parent != null)
            {
                tileLayer = hit.collider.gameObject.transform.parent.gameObject.name;
            }
            else
            {
                print("Destroy Tower");
                Destroy(currentTowerLOCAL);

                return;
            }



      //      print(tileLayer);
      //      print("Name:" + hit.collider.gameObject.name);
      //      print("Name:" + hit.collider.GetComponent<WorldTile>().towering);


            //  TO DO   // - HARD CODED ???
            if (tileLayer == "Parent_Ground" || tileLayer == "Parent_UnwalkableTiles")
            {
                //Leave the Tower on the node, Call spawner later for init
            }
            if (hit.collider.GetComponent<WorldTile>().towering)
            {
                //Leave the Tower on the node, Call spawner later for init
                currentTowerLOCAL.transform.position = hit.collider.gameObject.transform.position;
                currentTowerLOCAL.transform.position += Vector3.back;
                //LogRaycasthitObject(hit.collider.gameObject.transform.position.ToString(), hit.collider.gameObject.transform.parent.gameObject.name);
                hit.collider.GetComponent<WorldTile>().towering = false;
            }
            else
            {
                print("Destroy Tower");
                Destroy(currentTowerLOCAL);
            }
        }
        else
        {
            print("Destroy Tower");
            Destroy(currentTowerLOCAL);
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
      //  Debug.Log(logString);
    }

    /////////////////////////////////////////////////////////////////
}
