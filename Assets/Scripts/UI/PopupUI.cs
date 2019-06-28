using UnityEngine;
using UnityEngine.UI;


public class PopupUI : MonoBehaviour
{
    public GameObject uiPrefab;

    private InputDetection inputDetection = new InputDetection();
    private GameObject currentUIPrefab;

    private bool exitCondition;
    private TileNodes tileNodes;
    private void Start()
    {
        tileNodes = GameObject.FindObjectOfType<TileNodes>();
    }

    private void Update()
    {
        //Excetue 
        if (inputDetection.BeginClickEvent() == "Clicked")
        {
            ShowPopUpPanel(uiPrefab);
        }
    }
    public void ShowPopUpPanel(GameObject uiPrefab)
    {
        Ray raycastMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(raycastMouse, out hit, Mathf.Infinity))
        {
            //Check for tileNode or check for tile?
            if(hit.collider.gameObject.name.Contains("NODE"))
            {
                Vector3Int nodePosition = Vector3Int.FloorToInt(hit.collider.gameObject.transform.position);
                if (tileNodes.uniqueTilemap.GetTile(tileNodes.uniqueTilemap.WorldToCell(nodePosition)).name.Contains("Hidden"))
                {
                    GameObject canvas = GameObject.Find("Canvas");

                    //if(GameObject.Find("/"+canvas.name+""))
                    Vector2 uiOffset = new Vector2(canvas.GetComponent<RectTransform>().sizeDelta.x / 2f, canvas.GetComponent<RectTransform>().sizeDelta.y / 2f);
                    Vector2 viewportPoint = Camera.main.WorldToViewportPoint(uiPrefab.transform.position);
                    Vector2 proportionalPosition = new Vector2(viewportPoint.x * canvas.GetComponent<RectTransform>().sizeDelta.x, viewportPoint.y
                                                                                                                                   * canvas.GetComponent<RectTransform>().sizeDelta.y);
                    uiPrefab = Instantiate(uiPrefab, canvas.transform);
                    currentUIPrefab = uiPrefab;

                    uiPrefab.GetComponent<RectTransform>().anchoredPosition = proportionalPosition - uiOffset;
                    uiPrefab.AddComponent<PopupUI>();
                    uiPrefab.GetComponentInChildren<Button>().onClick.AddListener(delegate { ClosePopUpPanel(uiPrefab, hit.collider.gameObject.transform); });
                }
            }
        }

        //TODO: Play aniamtion
        
        //if (inputDetection.RaycastDetectionWorldTileBreakable(true))
    }

    void ClosePopUpPanel(GameObject uiPrefab, Transform breakableBlock)
    {
        //TODO: Close aniamtion before exit
        tileNodes.ShowTiles(breakableBlock.transform);
        DestroyImmediate(uiPrefab);
    }

    public void ButtonListener_ShowTiles()
    {
    }

}
