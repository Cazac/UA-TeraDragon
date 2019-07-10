using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public SpriteRenderer towerSpriteRenderer;

    //////////////////////////////////////////////////////////

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


    /*

    

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
