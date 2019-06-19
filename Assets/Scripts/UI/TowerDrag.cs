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
    [Header("Prefab Towers")]
    public GameObject towerPrefab_UI;
    public GameObject towerPrefab_Spawn;

    [Header("Parent Gameobject")]
    public GameObject towerParent;

    private GameObject currentTower;
    private SoundManager soundManager;

    [Header("UI_SoundEffect onclick")]
    public SoundObject soundEffect;


    private void Start()
    {
        soundManager = GameObject.FindObjectOfType<SoundManager>();
    }
    /////////////////////////////////////////////////////////////////

    ///////////////
    /// <summary>
    /// Undocumented
    /// </summary>
    ///////////////
    ///
    public void OnBeginDrag(PointerEventData eventData)
    {
        // TO DO ???
        //Check Money
        //Charge PLyaer
        //Spawn Tower UI
        //Get Reff To Tower
        //Attach to Cursor

        currentTower = Instantiate(towerPrefab_UI);
        soundManager.PlayOnUIClick(soundEffect);
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



        print("Destroy Tower");
        Destroy(currentTower);

        //Check Raycast for any hit with COLLIDERS
        if (Physics.Raycast(raycastMouse, out hit, Mathf.Infinity))
        {
            //Check node name
            string tileLayer = hit.collider.gameObject.transform.parent.gameObject.name;


            //print(tileLayer);

         

            if (hit.collider.GetComponent<WorldTile>().towering)
            {
                //Leave the Tower on the node, Call spawner later for init
                //currentTower.transform.position = hit.collider.gameObject.transform.position;



                GameObject newTower = Instantiate(towerPrefab_Spawn, hit.collider.gameObject.transform.position, Quaternion.identity, towerParent.transform);

                


                hit.collider.GetComponent<WorldTile>().towering = false;

            }
            else
            {
                print("Destroy Tower");
                //Destroy(currentTower);
            }
        }
        else
        {
            print("Destroy Tower");
            //Destroy(currentTower);
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
