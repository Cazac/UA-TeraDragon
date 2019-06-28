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

    private GameObject currentTower;
    private SoundManager soundManager;

    private TileNodes tileNodes;
    private WaveManager waveManager;

    //TO DO HARD CODED COST
    private int towerCost = 5;


    private void Start()
    {
        waveManager = GameObject.FindObjectOfType<WaveManager>();
        tileNodes = GameObject.FindObjectOfType<TileNodes>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();

        
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
            if(towerColor == "Red")
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

            //print(tileLayer);

            if (hit.collider.GetComponent<WorldTile>().towering && currentTower.name.Contains("Tower"))
            {
                //Leave the Tower on the node, Call spawner later for init
                //currentTower.transform.position = hit.collider.gameObject.transform.position;

                GameObject newTower = Instantiate(towerPrefab_Spawn, hit.collider.gameObject.transform.position, Quaternion.identity, towerParent.transform);

                hit.collider.GetComponent<WorldTile>().towering = false;

                //Destory old UI Tower
                Destroy(currentTower);
                return;
            }

            //Condition for barrier
            if(!hit.collider.GetComponent<WorldTile>().isBlockedBarrier && hit.collider.GetComponent<WorldTile>().walkable && currentTower.name.Contains("Barrier")
                && waveManager.CurrentWave.TimeUntilSpawn >= 0 && waveManager.EnableSpawning == false)
            {
                hit.collider.GetComponent<WorldTile>().isBlockedBarrier = true;
                    GameObject newTower = null;
                tileNodes.CheckBlockedPath();

                if(tileNodes.pathData.blockedPaths.Count <= tileNodes.pathData.paths.Count /*&& !tileNodes.pathData.blockedPaths.Contains(waveManager.CurrentWave.Paths[0])*/)
                    newTower = Instantiate(towerPrefab_Spawn, hit.collider.gameObject.transform.position, Quaternion.identity, towerParent.transform);
                else
                {
                    hit.collider.GetComponent<WorldTile>().isBlockedBarrier = false;
                    tileNodes.CheckBlockedPath();
                }

                //Destory old UI Tower
                Destroy(currentTower);
                return;
            }
        }

        //Refund The Tower
        RefundDragTower();
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