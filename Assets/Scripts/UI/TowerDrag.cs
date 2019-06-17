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

public class TowerDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
   
    public GameObject towerPrefab;
    private GameObject currentTower;

    // TO DO ???
    public int tileLayer;

    /////////////////////////////////////////////////////////////////


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

        currentTower = Instantiate(towerPrefab);
    }


    ///////////////
    /// <summary>
    /// Undocumented
    /// </summary>
    ///////////////
    public void OnDrag(PointerEventData eventData)
    {
        //currentTower.transform.position = cursor.transform.position;
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0;

        currentTower.transform.position = cursorPosition;
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
            //Check node name
            string tileLayer = hit.collider.gameObject.transform.parent.gameObject.name;


            print(tileLayer);
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
                currentTower.transform.position = hit.collider.gameObject.transform.position;
                LogRaycasthitObject(hit.collider.gameObject.transform.position.ToString(), hit.collider.gameObject.transform.parent.gameObject.name);
                hit.collider.GetComponent<WorldTile>().towering = false;

            }
            else
            {
                print("Destroy Tower");
                Destroy(currentTower);
            }
        }
        else
        {
            print("Destroy Tower");
            Destroy(currentTower);
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
