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

public class BarrierDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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
        RaycastHit hit;


        //Check Raycast for any hit with COLLIDERS
        if (Physics.Raycast(raycastMouse, out hit, Mathf.Infinity))
        {

            //Check node name
            string tileLayer = hit.collider.gameObject.transform.parent.gameObject.name;
            if (tileLayer == null)
            {
                print("called");
                Destroy(currentBarrier);
            }

            //Condition for barrier
            if (!hit.collider.GetComponent<WorldTile>().isBlockedBarrier && hit.collider.GetComponent<WorldTile>().walkable && currentBarrier.name.Contains("Barrier")
                && waveManager.CurrentWave.TimeUntilSpawn >= 0 && waveManager.EnableSpawning == false && waveManager.waveParent.transform.childCount <= 0)
            {
                hit.collider.GetComponent<WorldTile>().isBlockedBarrier = true;
                GameObject newTower = null;

                //DrawBlockedPath(tileNodes.pathData);

                if (tileNodes.CheckBlockedPath() && tileNodes.pathData.blockedPaths.Count <= tileNodes.pathData.paths.Count)
                {
                    newTower = Instantiate(towerPrefab_Spawn, hit.collider.gameObject.transform.position, Quaternion.identity, towerParent.transform);
                }
                else
                {
                    Destroy(currentBarrier);
                }
            }
        }

        //Refund The Tower
        RefundDragTower();

        cameraPanningCursor.IsUIDragging = false;

    }

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
