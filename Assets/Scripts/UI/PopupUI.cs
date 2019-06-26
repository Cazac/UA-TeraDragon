using UnityEngine;
using UnityEngine.UI;


public class PopupUI : MonoBehaviour
{
    private InputDetection inputDetection = new InputDetection();
    private GameObject currentUI;
    private bool exitCondition;

    private void Update()
    {
        //Excetue 
        if (inputDetection.BeginClickEvent() == "Clicked")
        {

        }
    }
    public void ShowPopUpPanel(GameObject uiPrefab)
    {
        //Play aniamtion
        Vector2 localUiPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("Canvas").GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out localUiPoint);

        //if (inputDetection.RaycastDetectionWorldTileBreakable(true))
            currentUI = Instantiate(uiPrefab, localUiPoint, Quaternion.identity, GameObject.Find("Canvas").transform);
    }

    public void ClosePopUpPanel()
    {
        //Close aniamtion before exit
    }

}
