using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TD_TowerDrag : MonoBehaviour, IDragHandler, IEndDragHandler
{
    ///////////////
    /// <summary>
    ///     
    /// TD_TowerDrag is used to drag and drop all towers into the game
    /// 
    /// </summary>
    ///////////////

    ////////////////////////////////


    /////////////////////////////////////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /////////////////////////////////////////////////////////////////

    public void OnDrag(PointerEventData eventData)
    {


        //Check Money
        //Charge PLyaer
        //Spawn Tower UI
        //Get Reff To Tower
        //Attach to Cursor
        //

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        //Find Nearest Node

        //No Node Then Refund


        //if Node Call Tower Spawner on Node

    }

    /////////////////////////////////////////////////////////////////
}
