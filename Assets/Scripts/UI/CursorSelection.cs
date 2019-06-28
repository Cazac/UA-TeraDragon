using UnityEngine;
using System;
using System.Collections;
using WaveSystem;

///////////////
/// <summary>
///     
/// CursorSelection
/// 
/// </summary>
///////////////
[ExecuteInEditMode]
public class CursorSelection : MonoBehaviour
{
    [Header("The layer that floor tile is in")]
    public int tileLayer;
    [Header("The layer that tower tile is in")]
    public int TowerLayer;
   // int layer_mask = LayerMask.GetMask("Tower");

    private WaveManager waveManager;
    private TowerSelector selectorManager;

    /////////////////////////////////////////////////////////////////

    private void Start() 
    {
        TowerLayer = LayerMask.GetMask("Tower");
        selectorManager = GameObject.FindObjectOfType<TowerSelector>();
        

        //Caching reference
        waveManager = GameObject.FindObjectOfType<WaveManager>();
       // Debug.Log(waveManager);
    }

    private void Update() => OnClickSelect();

    ///////////////
    /// <summary>
    /// Detects mouse click and performs raycast to a tile, if detected then turn that tile to the color black
    /// </summary>
    /// <para>
    /// Uses raycast from mouse position to collider
    /// </para>
    ///////////////
    private void OnClickSelect()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
            
        Ray raycastMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(raycastMouse, out hit, Mathf.Infinity, TowerLayer))
        {
            selectorManager.selectedNode = hit.collider.gameObject;

            // waveManager.NodeSpawnPosition.Add(hit.collider.gameObject.transform.position);


            Debug.Log("TowerLayer hit:" + hit.collider.gameObject.name + " selected");
        }
        else if (Physics.Raycast(raycastMouse, out hit, Mathf.Infinity, tileLayer))
        {
            Debug.Log("hit layer:" + hit.transform.gameObject.layer);
            //TODO: Color is hardcoded to black when a tile is clicked, need to change to dynamic
            hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            //LogRaycasthitObject(hit.collider.gameObject.transform.position.ToString(),
            //hit.collider.gameObject.transform.parent.gameObject.name);

            //Store hit tile node in a list in tD_TileNodes
          //  selectorManager.SelectedNodes.Add(hit.collider.gameObject);
            selectorManager.selectedNode = hit.collider.gameObject; 

            //waveManager.NodeSpawnPosition.Add(hit.collider.gameObject.transform.position);
            Debug.Log(hit.collider.gameObject.name + " selected");
        }
        else
        {
            selectorManager.selectedNode = null;
        }
    }


    ///////////////
    /// <summary>
    /// Print position and type of object detected by raycast
    /// </summary>
    ///     <param name="position">Transform position of gameobject</param>
    ///     <param name="type">Parent name of gameobject</param>
    ///////////////
    internal void LogRaycasthitObject(String position, String type)
    {
        String logString = String.Format("Hit node at position: {0}, is type of: {1}", position, type);
        //Debug.Log(logString);
    }
}
