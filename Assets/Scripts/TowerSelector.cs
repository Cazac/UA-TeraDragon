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

    public GameObject TowerWindowPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TowerUI();
        if (selectedNode == null)
        {
            if(CurrentTowerWindow != null)
            {
                Destroy(CurrentTowerWindow);
                CurrentTowerWindow = null;
            }
        }
    }

    public GameObject CurrentTowerWindow;
    void TowerUI()
    {
        if(selectedNode != null && selectedNode.GetComponent<TowerScript>() != null)
        {
            CurrentTowerWindow = Instantiate(TowerWindowPrefab, selectedNode.transform.position, new Quaternion());

        }
    }

}
