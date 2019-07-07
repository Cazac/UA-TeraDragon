using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelector : MonoBehaviour
{
    //  TO DO   // - Used for ???
    [Header("Selected Nodes")]
    [SerializeField]
    private List<GameObject> selectedNodes = new List<GameObject>();
    public List<GameObject> SelectedNodes { get => selectedNodes; set => selectedNodes = value; }

    public GameObject selectedNode;
    public GameObject TowerWindowPrefab;

    public TowerShooting SelectedTower;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit2D = Physics2D.Raycast(mousePos, Vector2.zero, 10, LayerMask.GetMask("Tower"));

            //if anything is collided
            if (hit2D.collider != null)
            {
                print("2D hit:" + hit2D.collider.name);
                SelectedTower = hit2D.collider.gameObject.GetComponentInChildren<TowerShooting>();
            }

        }
        else if (Input.GetMouseButton(1))
        {
            SelectedTower = null;
        }
        TowerUI();
    }
    
    public GameObject CurrentTowerWindow;
    void TowerUI()
    {
        if(SelectedTower != null)
        {
            if(CurrentTowerWindow == null)
            {
                CurrentTowerWindow = Instantiate(TowerWindowPrefab, SelectedTower.transform.position, new Quaternion());
                CurrentTowerWindow.GetComponent<TowerNodeUIScript>().changeNodeText(
                    SelectedTower.TowerName + " \n" + 
                    "Damage: " + SelectedTower.projectilePresetData.projectileDamage + "  \n" + 
                    "Attack Speed: " + SelectedTower.timeToReload);
                Debug.Log(SelectedTower.projectilePresetData.name + " \n" + SelectedTower.projectilePresetData.projectileDamage + "  \n" + SelectedTower.timeToReload);
            }
        }
        if(SelectedTower == null)
        {
            Destroy(CurrentTowerWindow);
        }
    }
    

}
