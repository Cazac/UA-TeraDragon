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

    private WaveManager waveManager;

    private TD_TileNodes tD_TileNodes;


    /////////////////////////////////////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /////////////////////////////////////////////////////////////////

    public void OnBeginDrag(PointerEventData eventData)
    {
        print("Thing");

        currentTower = Instantiate(towerPrefab);


    }

    public void OnDrag(PointerEventData eventData)
    {
        //print("THOG");


        currentTower.transform.position = cursor.transform.position;



        //Check Money
        //Charge PLyaer
        //Spawn Tower UI
        //Get Reff To Tower
        //Attach to Cursor
        //

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("Destroy");
        Destroy(currentTower);





        /// <summary>
        ///Detects mouse click and performs raycast to a tile, if detected then turn that tile to the color black
        /// </summary>
        ///<para>
        ///Uses raycast from mouse position to collider
        ///</para>


        return;
        

        Ray raycastMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(raycastMouse, out hit, Mathf.Infinity, tileLayer))
        {
            //TODO: Color is hardcoded to black when a tile is clicked, need to change to dynamic
            hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            LogRaycasthitObject(hit.collider.gameObject.transform.position.ToString(),
            hit.collider.gameObject.transform.parent.gameObject.name);

            //Store hit tile node in a list in tD_TileNodes
            tD_TileNodes.SelectedNode.Add(hit.collider.gameObject);

        }


     

        //Find Nearest Node

        //No Node Then Refund


        //if Node Call Tower Spawner on Node




    }


    internal void LogRaycasthitObject(String position, String type)
    {
        String logString = String.Format("Hit node at position: {0}, is type of: {1}", position, type);
          Debug.Log(logString);
    }

    /////////////////////////////////////////////////////////////////
}
