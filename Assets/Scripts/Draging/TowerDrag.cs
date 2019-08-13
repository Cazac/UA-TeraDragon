using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

///////////////
/// <summary>
///
/// TowerDrag is used to drag and drop all towers into the game. 
/// A UI version of the tower is attached to the mouse to drag and when dropped if the tile is valid,
/// a real tower that can fire is placed.
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

    [Header("Sound Effects")]
    public SoundObject towerDrag_SFX;
    public SoundObject towerPlacement_SFX;
    public SoundObject towerError_SFX;

    [Header("Player Stats")]
    public PlayerStats playerStats;

    [Header("Color")]
    public string towerColor;

    //Managers
    private TileNodes tileNodes;
    private SoundManager soundManager;
    private CameraPanningCursor cameraPanningCursor;

    //Current dragged tower
    private GameObject currentTower;

    //TO DO HARD CODED COST
    private int towerCost = 5;

    /////////////////////////////////////////////////////////////////

    private void Start()
    {
        //Setup Managers
        tileNodes = GameObject.FindObjectOfType<TileNodes>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        cameraPanningCursor = GameObject.FindObjectOfType<CameraPanningCursor>();
    }

    /////////////////////////////////////////////////////////////////

    ///////////////
    /// <summary>
    /// Dragging a tower will charge the player the cost then create a drag version of the Gameobject attached to the cursor.
    /// </summary>
    ///////////////
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Check if the button is usable
        if (gameObject.GetComponent<Button>().interactable)
        {
            //Charge player for the tower
            if (towerColor == "Red")
            {
                playerStats.crystalsOwned_Red -= towerCost;
            }
            if (towerColor == "Blue")
            {
                playerStats.crystalsOwned_Blue -= towerCost;
            }
            if (towerColor == "Green")
            {
                playerStats.crystalsOwned_Green -= towerCost;
            }
            if (towerColor == "Yellow")
            {
                playerStats.crystalsOwned_Yellow -= towerCost;
            }

            //Spawn Drag Tower 
            currentTower = Instantiate(towerPrefab_UI);

            //Spawn SFX
            soundManager.PlayOnUIClick(towerDrag_SFX, 0);
        }
        else
        {
            //Error SFX
            //soundManager.PlayOnUIClick(towerError_SFX);
        }
    }


    ///////////////
    /// <summary>
    /// Every frame the curosr moves make the current dragged tower follow it.
    /// </summary>
    ///////////////
    public void OnDrag(PointerEventData eventData)
    {

        //Check for a tower
        if (currentTower != null)
        {
            //Get cursor position
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = 0;

            //Move tower
            currentTower.transform.position = cursorPosition;
        }

        //usefull ???
        cameraPanningCursor.IsUIDragging = true;
    }


    ///////////////
    /// <summary>
    /// Raycast the tilemap looking for a node under the mouse when the tower is dropped onto the map, 
    /// validation check the tile then add it to the map. If not valid remove the tower from the cursor.
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

            //Dragging an empty tower
            if (currentTower == null)
            {
                return;
            }


            if (hit.collider.GetComponent<WorldTile>() != null && hit.collider.GetComponent<WorldTile>().towering)
            {
                //Create real tower on node
                GameObject newTower = Instantiate(towerPrefab_Spawn, hit.collider.gameObject.transform.position, Quaternion.identity, towerParent.transform);

                //Placement SFX
                soundManager.PlayOnUIClick(towerPlacement_SFX, 0.1f);

                //Remove old tower
                Destroy(currentTower);

                // Place new tower
                newTower.transform.position = new Vector3 (newTower.transform.position.x, newTower.transform.position.y, -10 + hit.collider.transform.position.y * 0.01f);
                hit.collider.GetComponent<WorldTile>().towering = false;
                return;
            }
            else
            {
                //No Match
                //print("No Matching Value, Destroy Miner");
                Destroy(currentTower);
                RefundDragTower();
                return;
            }
        }
        else
        {
            //No Raycast
            //print("No Raycast Hit, Destroy Tower");
            Destroy(currentTower);
            RefundDragTower();
            return;
        }
    }


    ///////////////
    /// <summary>
    /// Give the player the money back and remove the dragged tower.
    /// </summary>
    ///////////////
    public void RefundDragTower()
    {
        //Check for a tower
        if (currentTower != null)
        {
            //Refund Player
            if (towerColor == "Red")
            {
                playerStats.crystalsOwned_Red += towerCost;
            }
            if (towerColor == "Blue")
            {
                playerStats.crystalsOwned_Blue += towerCost;
            }
            if (towerColor == "Green")
            {
                playerStats.crystalsOwned_Green += towerCost;
            }
            if (towerColor == "Yellow")
            {
                playerStats.crystalsOwned_Yellow += towerCost;
            }
        }
    }

    /////////////////////////////////////////////////////////////////
}


