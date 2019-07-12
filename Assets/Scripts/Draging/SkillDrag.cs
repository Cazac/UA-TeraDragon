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
/// SkillDrag is used to drag and drop all skills into the game
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

    [Header("Click Sound Effect")]
    public SoundObject soundEffect;

    [Header("Player Stats")]
    public PlayerStats playerStats;

    [Header("Color")]
    public string skillColor;

    private GameObject currentSkill;

    [Header("Managers")]
    private TileNodes tileNodes;
    private SoundManager soundManager;
    private WaveManager waveManager;
    private CameraPanningCursor cameraPanningCursor;

    //TO DO HARD CODED COST
    private int skillCost = 5;

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
                playerStats.skillReady_Blue = false;
            }
            if (skillColor == "Green")
            {
                playerStats.skillReady_Green = false;
            }
            if (skillColor == "Yellow")
            {
                playerStats.skillReady_Yellow = false;
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

        //???
        cameraPanningCursor.IsUIDragging = false;

        //Check Raycast for any hit with COLLIDERS
        if (Physics.Raycast(raycastMouse, out RaycastHit hit, Mathf.Infinity))
        {
            //Spawn Skill
            GameObject newSkill = Instantiate(skillPrefab_Spawn, currentSkill.transform.position, Quaternion.identity, skillParent.transform);

            //Destory old UI Skill
            Destroy(currentSkill);
            return;
        }
        else
        {
            Destroy(currentSkill);
            playerStats.skillReady_Red = true;
            Debug.Log("BOUND ERROR, NO COLLIDER");
            return;
        }
    }

    /////////////////////////////////////////////////////////////////
}