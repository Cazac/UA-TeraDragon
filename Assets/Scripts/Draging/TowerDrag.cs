using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WaveSystem;

///////////////
/// <summary>
///
/// TD_TowerDrag is used to drag and drop all towers into the game
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

    [Header("UI_SoundEffect onclick")]
    public SoundObject soundEffect;

    [Header("Player Stats")]
    public PlayerStats playerStats;

    [Header("Color")]
    public string towerColor;

    //Dragging Tower
    private GameObject currentTower;

    //Managers
    private TileNodes tileNodes;
    private SoundManager soundManager;
    private WaveManager waveManager;
    private CameraPanningCursor cameraPanningCursor;

    //TO DO HARD CODED COST
    private int towerCost = 5;

    /////////////////////////////////////////////////////////////////

    private void Start()
    {
        waveManager = GameObject.FindObjectOfType<WaveManager>();
        tileNodes = GameObject.FindObjectOfType<TileNodes>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        cameraPanningCursor = GameObject.FindObjectOfType<CameraPanningCursor>();
    }

    /////////////////////////////////////////////////////////////////

    ///////////////
    /// <summary>
    /// Dragging a tower will charge the player the cost then create a drag version of the Gameobject.
    /// </summary>
    ///////////////
    public void OnBeginDrag(PointerEventData eventData)
    {

        if (gameObject.GetComponent<Button>().interactable)
        {
            //Charge Player
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

            playerStats.UpdateCrystalUI();

            //Spawn Tower Drag
            currentTower = Instantiate(towerPrefab_UI);
            soundManager.PlayOnUIClick(soundEffect);
        }
        else
        {
            //Error SFX
            Debug.Log("No Money");
        }

    }


    ///////////////
    /// <summary>
    /// Undocumented
    /// </summary>
    ///////////////
    public void OnDrag(PointerEventData eventData)
    {
        if (currentTower != null)
        {
            //currentTower.transform.position = cursor.transform.position;
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = 0;

            currentTower.transform.position = cursorPosition;
        }
        cameraPanningCursor.IsUIDragging = true;
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

        //Useful?
        cameraPanningCursor.IsUIDragging = false;

        //Check Raycast for any hit with COLLIDERS
        if (Physics.Raycast(raycastMouse, out RaycastHit hit, Mathf.Infinity))
        {
            //Check hit tile name
            string tileTypeName = hit.collider.gameObject.name;

            if (hit.collider.GetComponent<WorldTile>() != null && hit.collider.GetComponent<WorldTile>().towering)
            {
                //Create Spawn Tower
                GameObject newTower = Instantiate(towerPrefab_Spawn, hit.collider.gameObject.transform.position, Quaternion.identity, towerParent.transform);

                //Remove old tower
                Destroy(currentTower);
                hit.collider.GetComponent<WorldTile>().towering = false;
                return;
            }
            else
            {
                //No Match
                print("No Matching Value, Destroy Miner");
                Destroy(currentTower);
                RefundDragTower();
                return;
            }

        }
        else
        {
            //No Raycast
            print("No Raycast Hit, Destroy Tower");
            Destroy(currentTower);
            RefundDragTower();
            return;
        }




        // ???
        cameraPanningCursor.IsUIDragging = false;
    }



    public void RefundDragTower()
    {
        if (currentTower != null)
        {
            print("Refund Tower");

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

            playerStats.UpdateCrystalUI();
            Destroy(currentTower);
        }
    }

    /////////////////////////////////////////////////////////////////
}


