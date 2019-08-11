using UnityEngine;
using UnityEngine.EventSystems;
using System;

public abstract class DragInterface : MonoBehaviour
{
    public GameObject dragPrefab_UI;
    public GameObject dragPrefab_Spawn;

    public GameObject dragParent;

    public GameObject currentTower;
    public SoundManager soundManager;

    [Header("UI_SoundEffect onclick")]
    public SoundObject soundEffect;

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

        currentTower = Instantiate(dragPrefab_UI);
        if (soundManager != null)
        {
            soundManager.PlayOnUIClick(soundEffect, 0f);
        }
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

            //if (true)//Checktile
            //{
            //   //Process
            //}
            //else
            //{
            //    print("Destroy Tower");
            //    //Destroy(currentTower);
            //}

        }
        else
        {
            print("Destroy Tower");
            //Destroy(currentTower);
        }
    }
}

