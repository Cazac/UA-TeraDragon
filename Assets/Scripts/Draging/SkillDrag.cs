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
/// SkillDrag is used to drag and drop all skills into the game.
/// A UI version of the tower is attached to the mouse to drag and when dropped if the tile is valid,
/// a real skill that will activate will be placed.
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

    [Header("Sound Effects")]
    public SoundObject skillDrag_SFX;
    public SoundObject skillError_SFX;

    [Header("Player Stats")]
    public PlayerStats playerStats;

    [Header("Color")]
    public string skillColor;

    //Managers
    private TileNodes tileNodes;
    private SoundManager soundManager;
    private CameraPanningCursor cameraPanningCursor;

    //Current dragged skill
    private GameObject currentSkill;

    //TO DO HARD CODED COST
    private int skillCost = 5;

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
    /// Dragging a skill will reset the skill timer and create a drag version of the Gameobject attached to the cursor.
    /// </summary>
    ///////////////
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Check if the button is usable
        if (gameObject.GetComponent<Button>().interactable)
        {
            //Disable skill bool
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

            //Spawn Skill Drag
            currentSkill = Instantiate(skillPrefab_UI);

            //Skill SFX
            soundManager.PlayOnUIClick(skillDrag_SFX, 0);
        }
        else
        {
            //Error SFX
            //soundManager.PlayOnUIClick(skillError_SFX, );
        }
    }


    ///////////////
    /// <summary>
    /// Every frame the curosr moves make the current dragged skill follow it.
    /// </summary>
    ///////////////
    public void OnDrag(PointerEventData eventData)
    {
        //Check for a skill
        if (currentSkill != null)
        {
            //Get cursor position
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = 0;

            //Move skill
            currentSkill.transform.position = cursorPosition;
        }

        //usefull ???
        cameraPanningCursor.IsUIDragging = true;
    }


    ///////////////
    /// <summary>
    /// Raycast the tilemap looking for a node under the mouse when the tower is dropped onto the map,
    /// validation check the tile then add it to the map. If not valid remove the skill from the cursor.
    /// </summary>
    ///////////////
    public void OnEndDrag(PointerEventData eventData)
    {
        //Get current mouse raycast
        Ray raycastMouse = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Useful ???
        cameraPanningCursor.IsUIDragging = false;

        //Dragging an empty skill
        if (currentSkill == null)
        {
            return;
        }

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
            //No Raycast
            Destroy(currentSkill);
            playerStats.skillReady_Red = true;
            //Debug.Log("BOUND ERROR, NO COLLIDER");
            return;
        }
    }

    /////////////////////////////////////////////////////////////////
}