using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelector : MonoBehaviour
{
    //  TO DO   // - Used for ???
    [Header("Selected Nodes")]
    [SerializeField]
    private List<GameObject> selectedNodes = new List<GameObject>();
    public List<GameObject> SelectedNodes { get => selectedNodes; set => selectedNodes = value; }

    public GameObject selectedNode;

    public TowerScript SelectedTower;

    public GameObject TowerWindowPrefab;
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
                SelectedTower = hit2D.collider.gameObject.GetComponent<TowerScript>();
            }
            else
            {
              //  SelectedTower = null;
            }

        }
        TowerUI();

    }

    public GameObject CurrentTowerWindow;
    void TowerUI()
    {
        if(SelectedTower != null)
        {

            if(CurrentTowerWindow == null)
                CurrentTowerWindow = Instantiate(TowerWindowPrefab, SelectedTower.transform.position, new Quaternion());
        }
        if(SelectedTower == null)
        {
          //  Destroy(CurrentTowerWindow);
        }
    }

}
