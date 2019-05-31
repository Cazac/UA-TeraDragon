using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TD_NodeDrop : MonoBehaviour, IDropHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //print(Input.mousePosition);
    }


    public void OnPointerEnter(PointerEventData data)
    {

        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();

        sprite.color = Color.black;


        print("YUP");

    }

    public void OnPointerExit(PointerEventData data)
    {
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();

        sprite.color = Color.green;
    }


    public void OnDrag(PointerEventData eventData)
    {
    
    }

    public void OnDrop(PointerEventData eventData)
    {

        print("Add");

        //Check Money
        //Charge PLyaer
        //Spawn Tower UI
        //Get Reff To Tower
        //Attach to Cursor
        //

    }

}
