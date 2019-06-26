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
        if(inputDetection.BeginClickEvent() =="Clicked")
        {

        }
    }
    public void ShowPopUpPanel(GameObject uiPrefab)
    {
        //Play aniamtion
        currentUI = Instantiate(uiPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
    }

    public void ClosePopUpPanel()
    {
        //Close aniamtion before exit
    }

}
