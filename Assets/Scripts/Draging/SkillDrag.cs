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

public class SkillDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Prefab Skills")]
    public GameObject skillPrefab_UI;
    public GameObject skillPrefab_Spawn;

    [Header("Parent Gameobject")]
    public GameObject skillParent;

    [Header("UI_SoundEffect onclick")]
    public SoundObject soundEffect;

    [Header("Player Stats")]
    public PlayerStats playerStats;

    [Header("Color")]
    public string skillColor;

    private GameObject currentSkill;
    private SoundManager soundManager;

    private TileNodes tileNodes;
    private WaveManager waveManager;
    private CameraPanningCursor cameraPanningCursor;

    //TO DO HARD CODED COST
    private int skillCost = 5;


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
            if (skillColor == "Red")
            {
                playerStats.skillReady_Red = false;
            }
            if (skillColor == "Blue")
            {
                //playerStats.crystalsOwned_Blue -= towerCost;
            }
            if (skillColor == "Green")
            {
                //playerStats.crystalsOwned_Green -= towerCost;
            }
            if (skillColor == "Yellow")
            {
                //playerStats.crystalsOwned_Yellow -= towerCost;
            }

            playerStats.UpdateCrystalUI();

            //Spawn Skill Drag
            currentSkill = Instantiate(skillPrefab_UI);
            soundManager.PlayOnUIClick(soundEffect);
        }
        else
        {
            //Error SFX
            Debug.Log("No Cooldown");
        }
    }


    ///////////////
    /// <summary>
    /// Undocumented
    /// </summary>
    ///////////////
    public void OnDrag(PointerEventData eventData)
    {
        if (currentSkill != null)
        {
            //currentTower.transform.position = cursor.transform.position;
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = 0;

            currentSkill.transform.position = cursorPosition;
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

        cameraPanningCursor.IsUIDragging = false;

        //Check Raycast for any hit with COLLIDERS
        if (Physics.Raycast(raycastMouse, out hit, Mathf.Infinity))
        {
            //Spawn Skill
            GameObject newTower = Instantiate(skillPrefab_Spawn, currentSkill.transform.position, Quaternion.identity, skillParent.transform);

            //Destory old UI Skill
            Destroy(currentSkill);
            return;



            //start timer

            //set particules

            //set collisions


        }
        else
        {
            Destroy(currentSkill);

            Debug.Log("BOUND ERRORORORS");
            return;
        }


    }

    /////////////////////////////////////////////////////////////////
}