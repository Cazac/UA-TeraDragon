using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///////////////
/// <summary>
///     
/// Undocumented - Useful ?????
/// 
/// </summary>
///////////////

public class TowerSelector : MonoBehaviour
{
    //  TO DO   // - Used for ???
    [Header("Selected Nodes")]
    [SerializeField]
    private List<GameObject> selectedNodes = new List<GameObject>();
    public List<GameObject> SelectedNodes { get => selectedNodes; set => selectedNodes = value; }


    public GameObject selectedNode;
    public GameObject TowerWindowPrefab;

    public TowerScript SelectedTower;

    public GameObject CurrentTowerWindow;

    //////////////////////////////////////////////////////////

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit2D = Physics2D.Raycast(mousePos, Vector2.zero, 10, LayerMask.GetMask("Tower"));

            //if anything is collided
            if (hit2D.collider != null)
            {
                print("2D hit:" + hit2D.collider.name);
                SelectedTower = hit2D.collider.gameObject.GetComponentInChildren<TowerRange>().parentTowerScript;
            }

        }
        else if (Input.GetMouseButtonDown(1))
        {
            SelectedTower = null;
        }

        TowerUI();
    }


    private void TowerUI()
    {
        if (SelectedTower != null)
        {
            if (CurrentTowerWindow == null)
            {
                CurrentTowerWindow = Instantiate(TowerWindowPrefab, SelectedTower.transform.position, new Quaternion());
                //CurrentTowerWindow.GetComponent<TowerNodeUIScript>().changeNodeText(
                    //SelectedTower.towerName + " \n" + 
                    //"Damage: " + SelectedTower.towerRange.currentProjectileData.projectileDamage + "  \n" + 
                    //"Attack Speed: " + SelectedTower.towerRange.timeToReload);
                //Debug.Log(SelectedTower.projectileData.name + " \n" + SelectedTower.projectileData.projectileDamage + "  \n" + SelectedTower.timeToReload);
            }
        }
        if (SelectedTower == null)
        {
            Destroy(CurrentTowerWindow);
        }
    }

    //////////////////////////////////////////////////////////
}
