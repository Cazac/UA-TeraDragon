using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TD_TowerDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ///////////////
    /// <summary>
    ///     
    /// TD_TowerDrag is used to drag and drop all towers into the game onto TD_TowerDrop sockets
    /// 
    /// </summary>
    ///////////////

    public GameObject towerPrefab;
    public GameObject currentTower;

    public GameObject cursor;

    ////////////////////////////////


    public int tileLayer;



    /////////////////////////////////////////////////////////////////

    void Start()
    {

    }

    /////////////////////////////////////////////////////////////////

    public void OnBeginDrag(PointerEventData eventData)
    {
        print("Thing");

        currentTower = Instantiate(towerPrefab);

        print(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public void OnDrag(PointerEventData eventData)
    {
        //print("THOG");


        //currentTower.transform.position = cursor.transform.position;
        Vector3 test = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        test.z = 0;

        currentTower.transform.position = test;



        //Check Money
        //Charge PLyaer
        //Spawn Tower UI
        //Get Reff To Tower
        //Attach to Cursor
        //

    }

    public void OnEndDrag(PointerEventData eventData)
    {
 
        

        Ray raycastMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(raycastMouse, out hit, Mathf.Infinity))
        {

            string tileLayerParent = hit.collider.gameObject.transform.parent.gameObject.name;

            //  TO DO   // - HARD CODED ???
            if (tileLayerParent == "Parent_Ground")
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

    /////////////////////////////////////////////////////////////////

    public void SpawnTower()
    {
        //currentTower.transform.position
    }


    /////////////////////////////////////////////////////////////////

    internal void LogRaycasthitObject(String position, String type)
    {
        String logString = String.Format("Hit node spawing tower at position: {0}, is type of: {1}", position, type);
        Debug.Log(logString);
    }

    /////////////////////////////////////////////////////////////////
}
