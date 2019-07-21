using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///////////////
/// <summary>
///     
/// TowerScript Controllers the Towers on the game.
/// 
/// </summary>
///////////////

public class TowerScript : MonoBehaviour
{
    [Header("Tower Info")]
    public string towerName;
    public int currentTowerTier;

    [Header("Refferences")]
    public TowerData towerData;
    public TowerRange towerRange;
    public GameObject firingPoint;
    public GameObject towerUIPanel;
    public SpriteRenderer towerSpriteRenderer;

    [Header("Tower UI")]
    public Text towerUpgradeText;
    public Text towerSellText;
    public GameObject RangeVisualizer;

    //////////////////////////////////////////////////////////
    private void Start()
    {
        RangeVisualizer.SetActive(false);
    }

    public void UpgradeTower()
    {
        if (currentTowerTier < 3)
        {
            //Update Values
            currentTowerTier++;
            towerRange.GetTowerData();


            switch (currentTowerTier)
            {
                case 0:

                    Debug.Log("Error");
                    break;

                case 1:

                    towerSpriteRenderer.sprite = towerData.towerSprite_T1;
                    break;

                case 2:

                    towerSpriteRenderer.sprite = towerData.towerSprite_T2;
                    break;

                case 3:

                    towerSpriteRenderer.sprite = towerData.towerSprite_T3;
                    break;
            }
        }
    }

    //////////////////////////////////////////////////////////



    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //Raycast Mouse
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(mousePos, Vector2.zero, 10, LayerMask.GetMask("Tower"));

            //if anything is collided
            if (hit2D.collider != null)
            {
                //if correct tower hit.
                if (hit2D.collider == gameObject.GetComponent<Collider2D>())
                {
                    OpenTowerUI();
                    RangeVisualizer.SetActive(true);
                }
                else
                {
                    CloseTowerUI();
                    RangeVisualizer.SetActive(false);
                }
            }

        }
        else if (Input.GetMouseButton(1))
        {
            CloseTowerUI();
            RangeVisualizer.SetActive(false);
        }

    }


    private void CloseTowerUI()
    {
        towerUIPanel.SetActive(false);
    }


    private void OpenTowerUI()
    {
        towerUIPanel.SetActive(true);


        LoadTowerDataUI();

    }

    private void LoadTowerDataUI()
    {

    }


    //////////////////////////////////////////////////////////

    /*


private void TowerUI()
{
    if (SelectedTower != null)
    {
        if (CurrentTowerWindow == null)
        {
            CurrentTowerWindow = Instantiate(TowerWindowPrefab, SelectedTower.transform.position, new Quaternion());
            CurrentTowerWindow.GetComponent<TowerNodeUIScript>().changeNodeText(
                SelectedTower.towerName + " \n" +
                "Damage: " + SelectedTower.towerRange.currentProjectileData.projectileDamage + "  \n" +
                "Attack Speed: " + SelectedTower.towerRange.timeToReload);
            //Debug.Log(SelectedTower.projectileData.name + " \n" + SelectedTower.projectileData.projectileDamage + "  \n" + SelectedTower.timeToReload);
        }
    }
    if (SelectedTower == null)
    {
        Destroy(CurrentTowerWindow);
    }
}


//////////////////////////////////////////////////////////





public float timer = 5f;

// Update is called once per frame
void Update()
{
    timer -= Time.deltaTime;
    if(timer < 0)
    {
        ShootTarget();
        timer = 5f;
    }
}

void ShootTarget()
{
    print(closestPosition);
}

Vector3 closestPosition;
private void OnTriggerEnter2D(Collider2D collision)
{
    Debug.Log(collision.transform.position);
    if(Vector3.Distance(transform.position, collision.transform.position) < Vector3.Distance(transform.position, closestPosition))
    {
        closestPosition = collision.transform.position;
    }
}

private void OnTriggerStay2D(Collider2D collision)
{
    Debug.Log(collision.transform.position);
    if (Vector3.Distance(transform.position, collision.transform.position) < Vector3.Distance(transform.position, closestPosition))
    {
        closestPosition = collision.transform.position;
    }
}

private void OnTriggerExit2D(Collider2D collision)
{
    Debug.Log(collision.transform.position);
}

private void OnCollisionStay2D(Collision2D collision)
{
    Debug.Log("Collider stay:" + collision.transform.position);
}

private void OnTriggerStay(Collider other)
{
    Debug.Log("Trigger 3d stay:" + other.transform.position);
}

private void OnCollisionStay(Collision collision)
{
    Debug.Log("Collision 3d stay:" + collision.transform.position);
}


*/
}
