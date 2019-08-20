using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

///////////////
/// <summary>
///     
/// MinerDrag is used to drag and drop all skills into the game.
/// A UI version of the tower is attached to the mouse to drag and when dropped if the tile is valid,
/// a real miner that will activate will be placed.
/// 
/// </summary>
///////////////

public class MinerDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Prefab Miner")]
    public GameObject minerPrefab_UI;
    public GameObject minerPrefab_Spawn;

    [Header("Parent Gameobject")]
    public GameObject minerParent;

    [Header("Sound Effects")]
    public SoundObject minerDrag_SFX;
    public SoundObject minerPlacement_SFX;
    public SoundObject minerError_SFX;

    [Header("Player Stats")]
    public PlayerStats playerStats;

    //Managers
    private TileNodes tileNodes;
    private SoundManager soundManager;
    private CameraPanningCursor cameraPanningCursor;
    
    //Current dragged miner
    private GameObject currentMiner;

    //TO DO HARD CODED COST ???
    private int minerCost = 5;

    //Permitted Tiles To Place a Miner On
    public TileBase[] validTileTypes;

    /////////////////////////////////////////////////////////////////

    private void Start()
    {
        //Setup Managers
        tileNodes = GameObject.FindObjectOfType<TileNodes>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        cameraPanningCursor = GameObject.FindObjectOfType<CameraPanningCursor>();
    }

    ///////////////
    /// <summary>
    /// Dragging a miner will charge the player the cost then create a drag version of the Gameobject attached to the cursor.
    /// </summary>
    ///////////////
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Check if the button is usable
        if (gameObject.GetComponent<Button>().interactable)
        {
            //Charge player for the tower
            playerStats.minersOwned -= 1;
           
            //Spawn Drag Miner 
            currentMiner = Instantiate(minerPrefab_UI);

            //Spawn SFX
            soundManager.PlayOnUIClick(minerDrag_SFX, 0);
        }
        else
        {
            //Error SFX
            //soundManager.PlayOnUIClick(minerError_SFX);
        }
    }


    ///////////////
    /// <summary>
    /// Every time the mouse moves, match the new dragged miner to the mouse position
    /// </summary>
    ///////////////
    public void OnDrag(PointerEventData eventData)
    {
        //Check for a miner
        if (currentMiner != null)
        {
            //Get cursor position
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = 0;

            //Move tower
            currentMiner.transform.position = cursorPosition;
        }

        //usefull ???
        cameraPanningCursor.IsUIDragging = true;
    }


    ///////////////
    /// <summary>
    /// Raycast the tilemap looking for a node under the mouse when the tower is dropped onto the map,
    /// validation check the tile then add it to the map. If not valid remove the miner from the cursor.
    /// </summary>
    ///////////////
    public void OnEndDrag(PointerEventData eventData)
    {
        //Get current mouse raycast
        Ray raycastMouse = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Useful?
        cameraPanningCursor.IsUIDragging = false;

        //Dragging an empty Miner
        if (currentMiner == null)
        {
            return;
        }

        //Check Raycast for any hit with COLLIDERS
        if (Physics.Raycast(raycastMouse, out RaycastHit hit, Mathf.Infinity))
        {
            //Check hit tile name
            string tileTypeName = hit.collider.gameObject.name;

            if (hit.collider.GetComponent<CrystalTile>() != null && hit.collider.GetComponent<CrystalTile>().towering)
            {
                //Create real miner on node
                GameObject newMiner = Instantiate(minerPrefab_Spawn, hit.collider.gameObject.transform.position, Quaternion.identity, minerParent.transform);

                //Placement SFX
                soundManager.PlayOnUIClick(minerPlacement_SFX, 0.1f);

                //Raise Miner 
                newMiner.transform.position = new Vector2(newMiner.transform.position.x, newMiner.transform.position.y + 3.5f);

                //Set new miner values to tile
                newMiner.GetComponent<MinerScript>().level = 1;
                newMiner.GetComponent<MinerScript>().crystalTile = hit.collider.GetComponent<CrystalTile>();


                //Remove old miner
                Destroy(currentMiner);
                hit.collider.GetComponent<WorldTile>().towering = false;
                return;
            }
            else
            {
                //No Match
                print("No Matching Value, Destroy Miner");
                ManualDeselect();
            }
        }
        else
        {
            //No Raycast
            print("No Raycast Hit, Destroy Miner");
            ManualDeselect();
        }
    }

    public void ManualDeselect()
    {
        Destroy(currentMiner);
        RefundDragMiner();
        return;
    }

    ///////////////
    /// <summary>
    /// Give the player the money back and remove the dragged miner.
    /// </summary>
    ///////////////
    public void RefundDragMiner()
    {
        //Check for a miner
        if (currentMiner != null)
        {
            //Refund Player
            playerStats.minersOwned += 1;
        }
    }

    /////////////////////////////////////////////////////////////////
}
