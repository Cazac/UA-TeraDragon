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

        print("Start Dragging At: " + Camera.main.ScreenToWorldPoint(Input.mousePosition));

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
        Vector3 test = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        test.z = 0;

        currentTower.transform.position = test;
    }


    ///////////////
    /// <summary>
    /// Undocumented
    /// </summary>
    ///////////////
    public void OnEndDrag(PointerEventData eventData)
    {    
        //Get current mouse raycast
        Ray raycastMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(raycastMouse, out hit, Mathf.Infinity))
        {

            //  TO DO   // - Will break on Next Pass
            string tileLayer = hit.collider.gameObject.transform.parent.gameObject.name;


            print(tileLayer);

            //  TO DO   // - HARD CODED ???
            if (tileLayer == "Parent_Ground")
            {
                currentTower.transform.position = hit.collider.gameObject.transform.position;
                LogRaycasthitObject(hit.collider.gameObject.transform.position.ToString(), hit.collider.gameObject.transform.parent.gameObject.name);
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


        //Find Nearest Node
        //No Node Then Refund
        //if Node Call Tower Spawner on Node
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
