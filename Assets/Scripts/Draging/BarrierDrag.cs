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
/// BarrierDrag is used to drag and drop all skills into the game.
/// A UI version of the tower is attached to the mouse to drag and when dropped if the tile is valid,
/// a real barrier that will activate will be placed.
///
/// </summary>
///////////////

public class BarrierDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Prefab Barriers")]
    public GameObject barrierPrefab_UI;
    public GameObject barrierPrefab_Spawn;

    [Header("Parent Gameobject")]
    public GameObject barrierParent;

    [Header("Sound Effects")]
    public SoundObject barrierDrag_SFX;
    public SoundObject barrierError_SFX;

    [Header("Player Stats")]
    public PlayerStats playerStats;

    //Current dragged barrier
    private GameObject currentBarrier;

    //Managers
    private TileNodes tileNodes;
    private WaveManager waveManager;
    private SoundManager soundManager;
    private CameraPanningCursor cameraPanningCursor;

    //TO DO HARD CODED COST
    private int barrierCost = 0;

    /////////////////////////////////////////////////////////////////

    private void Start()
    {
        //Setup Managers
        waveManager = GameObject.FindObjectOfType<WaveManager>();
        tileNodes = GameObject.FindObjectOfType<TileNodes>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        cameraPanningCursor = GameObject.FindObjectOfType<CameraPanningCursor>();
    }

    /////////////////////////////////////////////////////////////////

    ///////////////
    /// <summary>
    /// Dragging a barrier will charge the player the cost then create a drag version of the Gameobject attached to the cursor.
    /// </summary>
    ///////////////
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Check if the button is usable
        if (gameObject.GetComponent<Button>().interactable)
        {
            //Charge Player
            //if (towerColor == "Red")
            {
                playerStats.crystalsOwned_Red -= barrierCost;
            }
            //if (towerColor == "Blue")
            {
                playerStats.crystalsOwned_Blue -= barrierCost;
            }
            //if (towerColor == "Green")
            {
                playerStats.crystalsOwned_Green -= barrierCost;
            }
            //if (towerColor == "Yellow")
            {
                playerStats.crystalsOwned_Yellow -= barrierCost;
            }

            //Spawn Tower Drag
            currentBarrier = Instantiate(barrierPrefab_UI);

            //Spawn SFX
            soundManager.PlayOnUIClick(barrierDrag_SFX, 0);
        }
        else
        {
            //Error SFX
            //soundManager.PlayOnUIClick(barrierError_SFX);
        }
    }


    ///////////////
    /// <summary>
    /// Every frame the cursor moves make the current dragged barrier follow it.
    /// </summary>
    ///////////////
    public void OnDrag(PointerEventData eventData)
    {
        //Check for a barrier
        if (currentBarrier != null)
        {
            //Get cursor position
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = 0;

            //Move barrier
            currentBarrier.transform.position = cursorPosition;
        }

        //usefull ???
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
            //Check hit node name
            string tileLayer = hit.collider.gameObject.transform.parent.gameObject.name;

            // ????
            if (tileLayer == null)
            {
                print("called");
                Destroy(currentBarrier);
            }

            //Condition for barrier ??????????
            if (!hit.collider.GetComponent<WorldTile>().isBlockedBarrier && hit.collider.GetComponent<WorldTile>().walkable && currentBarrier.name.Contains("Barrier"))
            {
                //Condition for barrier ??????????
                if (waveManager.CurrentWave.TimeUntilSpawn >= 0 && waveManager.EnableSpawning == false && waveManager.waveParent.transform.childCount <= 0)
                {
                    // ???
                    hit.collider.GetComponent<WorldTile>().isBlockedBarrier = true;

                    //DrawBlockedPath(tileNodes.pathData);

                    // ?????
                    if (tileNodes.CheckBlockedPath() && tileNodes.pathData.blockedPaths.Count <= tileNodes.pathData.paths.Count)
                    {
                        GameObject newBarrier = Instantiate(barrierPrefab_Spawn, hit.collider.gameObject.transform.position, Quaternion.identity, barrierParent.transform);
                        newBarrier.transform.position = new Vector3(newBarrier.transform.position.x, newBarrier.transform.position.y, -10 + hit.collider.transform.position.y * 0.01f);
                    }
                    else
                    {
                        Destroy(currentBarrier);
                    }
                }
            }
        }

        //Refund the barrier
        //RefundDragTower();

        //Remove it?????
        Destroy(currentBarrier);

        //usefull ???
        cameraPanningCursor.IsUIDragging = true;
    }


    ///////////////
    /// <summary>
    /// UNDOCUMENTED
    /// </summary>
    ///////////////
    private void DrawBlockedPath(PathsData blockedPath)
    {
        foreach (var path in blockedPath.blockedPaths)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawRay(path[i].transform.position, path[i + 1].transform.position - path[i].transform.position, Color.red, 100f, false);
            }
        }
    }


    ///////////////
    /// <summary>
    /// UNDOCUMENTED
    /// </summary>
    ///////////////
    public void RefundDragTower()
    {
        if (currentBarrier != null)
        {
            print("Refund Tower");

            //Refund Player
           // if (towerColor == "Red")
            {
                playerStats.crystalsOwned_Red += barrierCost;
            }
            //if (towerColor == "Blue")
            {
                playerStats.crystalsOwned_Blue += barrierCost;
            }
            //if (towerColor == "Green")
            {
                playerStats.crystalsOwned_Green += barrierCost;
            }
            //if (towerColor == "Yellow")
            {
                playerStats.crystalsOwned_Yellow += barrierCost;
            }

            playerStats.UpdateCrystalUI();
            Destroy(currentBarrier);
        }
    }

    /////////////////////////////////////////////////////////////////
}
