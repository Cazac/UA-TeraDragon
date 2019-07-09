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

    [Header("UI_SoundEffect onclick")]
    public SoundObject soundEffect;

    [Header("Player Stats")]
    public PlayerStats playerStats;

    [Header("Color")]
    public string towerColor;

    private GameObject currentBarrier;
    private SoundManager soundManager;

    private TileNodes tileNodes;
    private WaveManager waveManager;
    private CameraPanningCursor cameraPanningCursor;

    //TO DO HARD CODED COST
    private int barrierCost = 0;

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
    /// Undocumented
    /// </summary>
    ///////////////
    public void OnBeginDrag(PointerEventData eventData)
    {

        if (gameObject.GetComponent<Button>().interactable)
        {
            //Charge Player
            if (towerColor == "Red")
            {
                playerStats.crystalsOwned_Red -= barrierCost;
            }
            if (towerColor == "Blue")
            {
                playerStats.crystalsOwned_Blue -= barrierCost;
            }
            if (towerColor == "Green")
            {
                playerStats.crystalsOwned_Green -= barrierCost;
            }
            if (towerColor == "Yellow")
            {
                playerStats.crystalsOwned_Yellow -= barrierCost;
            }

            playerStats.UpdateCrystalUI();

            //Spawn Tower Drag
            currentBarrier = Instantiate(towerPrefab_UI);
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
        if (currentBarrier != null)
        {
            //currentTower.transform.position = cursor.transform.position;
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = 0;

            currentBarrier.transform.position = cursorPosition;
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
            Debug.Log(tileTypeName);

            if (hit.collider.GetComponent<WorldTile>() != null && hit.collider.GetComponent<WorldTile>().towering)
            {
                //Create Spawn Tower
                GameObject newTower = Instantiate(towerPrefab_Spawn, hit.collider.gameObject.transform.position, Quaternion.identity, towerParent.transform);

                //Remove old tower
                Destroy(currentBarrier);
                hit.collider.GetComponent<WorldTile>().towering = false;
                return;
            }
            else
            {
                //No Match
                print("No Matching Value, Destroy Miner");
                Destroy(currentBarrier);
                RefundDragTower();
                return;
            }

        }
        else
        {
            //No Raycast
            print("No Raycast Hit, Destroy Tower");
            Destroy(currentBarrier);
            RefundDragTower();
            return;
        }




        // ???
        cameraPanningCursor.IsUIDragging = false;
    }



    public void RefundDragTower()
    {
        if (currentBarrier != null)
        {
            print("Refund Tower");

            //Refund Player
            if (towerColor == "Red")
            {
                playerStats.crystalsOwned_Red += barrierCost;
            }
            if (towerColor == "Blue")
            {
                playerStats.crystalsOwned_Blue += barrierCost;
            }
            if (towerColor == "Green")
            {
                playerStats.crystalsOwned_Green += barrierCost;
            }
            if (towerColor == "Yellow")
            {
                playerStats.crystalsOwned_Yellow += barrierCost;
            }

            playerStats.UpdateCrystalUI();
            Destroy(currentBarrier);
        }
    }

    /////////////////////////////////////////////////////////////////
}


