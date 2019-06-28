using UnityEngine;
using UnityEngine.UI;


public class PopupUI : MonoBehaviour
{
    public GameObject uiPrefab;

    private InputDetection inputDetection = new InputDetection();
    private GameObject currentUI;
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


                    Vector2 uiOffset = new Vector2(canvas.GetComponent<RectTransform>().sizeDelta.x / 2f, canvas.GetComponent<RectTransform>().sizeDelta.y / 2f);
                    Vector2 viewportPoint = Camera.main.WorldToViewportPoint(uiPrefab.transform.position);
                    Vector2 proportionalPosition = new Vector2(viewportPoint.x * canvas.GetComponent<RectTransform>().sizeDelta.x, viewportPoint.y
                                                                                                                                   * canvas.GetComponent<RectTransform>().sizeDelta.y);
                    uiPrefab = Instantiate(uiPrefab, canvas.transform);

                    uiPrefab.GetComponent<RectTransform>().localPosition = proportionalPosition - uiOffset;
                    tileNodes.ShowTiles(hit.collider.gameObject.transform);
                }
            }
        }

        //Play aniamtion
        
        //if (inputDetection.RaycastDetectionWorldTileBreakable(true))
    }

    public void ClosePopUpPanel()
    {
        //Close aniamtion before exit
    }

}
