using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveSystem;

public class EnemyScript : MonoBehaviour
{
    public List<WorldTile> currentWaypoints;
    public List<List<WorldTile>> blockedWaypoints = new List<List<WorldTile>>();

    private int currentWaypoint = 0;
    private EnemyData enemyData;
    private TileNodes tileNodes;
    private PlayerStats playerStats;
    private WaveManager waveManager;

    private GameObject currentBarrier;

    [Header("Enemy Stats")]
    public int MaxHealth;
    public int CurrentHealth;
    [Range(0.05f, 30f)]
    public float speed = 1.0f;
    public float currentSpeed;
    public float speedWearOffTime;

    private bool isAttacking = false;

    // This seems better because allows us to change speed and reverse it
    Vector3 startPosition, endPosition, dir;

    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }


    /////////////////////////////////////////////////////////////////

    private void Awake()
    {
        tileNodes = GameObject.FindObjectOfType<TileNodes>();
        waveManager = GameObject.FindObjectOfType<WaveManager>();
        playerStats = GameObject.FindObjectOfType<PlayerStats>();  
    }
    
    private void Start()
    {
        currentSpeed = speed;
        //DynamicPathRelocation(transform.position);

        if (enemyData != null)
        {
            // TO-DO: need to add modifyier calculations
            speed = enemyData.BaseSpeed;
            Debug.Log(enemyData.name + " has spawned");
        }
        else
        {
            //Debug.Log("Spawing Monster from prefab data");
        }

    }

    private void FixedUpdate()
    {
        if (currentWaypoints.Count > 1 && isAttacking == false)
        {
            Move();
            //DynamicPathRelocation(transform.position);
        }
    }

    private void Update()
    {
        RefreshSlow();

        if (currentBarrier != null)
        {
            if (isAttacking)
            {
                StartCoroutine(AttackBarrier(currentBarrier.GetComponent<BarrierData>()));
            }
            if (!isAttacking)
            {
                StopCoroutine(AttackBarrier(currentBarrier.GetComponent<BarrierData>()));
            }
        }
    }

    /////////////////////////////////////////////////////////////////

    ///////////////
    /// <summary>
    /// UNDOCUMENTED
    /// </summary>
    ///////////////
    private void Move()
    {
        StartPosition = currentWaypoints[currentWaypoint].transform.position;
        endPosition = currentWaypoints[currentWaypoint + 1].transform.position;
        dir = endPosition - StartPosition;

        transform.position += dir.normalized * currentSpeed * Time.fixedDeltaTime;

        if (Vector3.Distance(gameObject.transform.position, endPosition) < 0.5f)
        {
            if (currentWaypoint < currentWaypoints.Count - 2)
            {
                currentWaypoint++;
                // TODO: Rotate into move direction
            }
            else
            {
                // to be replaced with more complete function

                if (enemyData != null)
                {
                    //Debug.Log(enemyData.name + " has died");
                }
                else
                {
                    //Debug.Log("Death???");
                }


  
                //Deal Damage
                playerStats.RemoveLife(1);

                //Base Damage Destruction
                Destroy(gameObject);
            }
        }

    }

    /// <summary>
    /// Recalulate path when current path has a barrier
    /// <para>Function runs by scanning for path that contains blocked tile, 
    /// and redirect to path without blocked tile </para>
    /// </summary>
    /// <param name="currentPosition">Current captured position of enemy gameobject at runtime</param>
    private void PathRelocation(Vector3 currentPosition)
    {
        //scan map for block waypoibt
      
        //Reapply waypoints
        foreach (var path in tileNodes.pathData.paths)
        {
            //If there's still a path to take
            if (!tileNodes.pathData.blockedPaths.Contains(path) && tileNodes.pathData.blockedPaths.Count >= 1 
               )
            {
                currentWaypoints = new List<WorldTile>(path);

                //StartPosition = currentPosition;
                return;
            }

            //If there's still a path to take
            else
            {

            }
        }
    }

    public void PathRelocation()
    {
        //scan map for block waypoibt

        //Reapply waypoints
        foreach (var path in tileNodes.pathData.paths)
        {
            //If there's still a path to take
            if (!tileNodes.pathData.blockedPaths.Contains(path) && tileNodes.pathData.blockedPaths.Count >= 1)
            {
                int gridXCurrentWave = waveManager.CurrentWave.Paths[0][0].gridX;
                int gridYCurrentWave = waveManager.CurrentWave.Paths[0][0].gridY;
                if (gridXCurrentWave == path[0].gridX && gridYCurrentWave == path[0].gridY)
                {
                    currentWaypoints = new List<WorldTile>(path);
                    return;
                }
               
            }

            //If there's still a path to take
            //else
            //{

            //}
        }
    }

    ///////////////
    /// <summary>
    /// UNDOCUMENTED
    /// </summary>
    ///////////////
    private void OnTriggerEnter2D(Collider2D collision)
    {


        //Check for collision with the ending tile - USELESS IN CURRENT VERSION????????
        if (collision.transform.GetComponent<BaseNode>() != null)
        {
            collision.transform.GetComponent<BaseNode>().BaseIsHit(1);
            Destroy(gameObject);
        }

        if(collision.gameObject.tag == "Barrier")
        {
            currentBarrier = collision.gameObject;
            isAttacking = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Barrier")
        {
            currentBarrier = collision.gameObject;
            isAttacking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Barrier")
        {
            isAttacking = false;
        }
    }


    ///////////////
    /// <summary>
    /// UNDOCUMENTED
    /// </summary>
    ///////////////
    public void TakeDamage(float damage)
    {
        //Normalize float vs int???
        CurrentHealth -= (int)damage;

        //Armor?????? TO DO



        //TEXT
        PopupText(damage);



        if (CurrentHealth <= 0)
        {
            //print("Death takes me...");
            Destroy(gameObject);
        }
    }

    public void PopupText(float damage)
    {
        //Normalize float vs int???
        int normalDamage = (int)damage;
        
        //Get From PLayer Stasts
        GameObject textPrefab = playerStats.popupDamageText_Prefab;


        //Create
        GameObject popupText = Instantiate(textPrefab, transform.position, Quaternion.identity);

        //Setup
        DamagePopup dmgPopup = popupText.GetComponent<DamagePopup>();


        dmgPopup.Setup(normalDamage);

    }

    ///////////////
    /// <summary>
    /// UNDOCUMENTED
    /// </summary>
    ///////////////
    public void ApplySlow(float slowSpeed, float slowTimer)
    {
  

        if (currentSpeed == speed)
        {
            //Apply Effect + Timer
            currentSpeed = currentSpeed * slowSpeed;
            speedWearOffTime = slowTimer;
        }
        else
        {
            //If can be slower, slow down more
            if ((speed * slowSpeed) < currentSpeed)
            {
                currentSpeed = speed * slowSpeed;
            }

            //Timer Refresh
            if (speedWearOffTime <= 0)
            {
                //New Timer
                speedWearOffTime = slowTimer;
            }
            else
            {
                //Stack Timer
                speedWearOffTime += slowTimer;
            }
        }

        if (currentSpeed < 1f)
        {
            //Apply Color
            gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            //Apply Color
            gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }

    ///////////////
    /// <summary>
    /// UNDOCUMENTED
    /// </summary>
    ///////////////
    private void RefreshSlow()
    {
        if (speedWearOffTime < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            currentSpeed = speed;
            speedWearOffTime = 0;
        }
        else
        {
            speedWearOffTime -= Time.deltaTime;
        }
    }

    ///////////////
    /// <summary>
    /// UNDOCUMENTED
    /// </summary>
    ///////////////
    public IEnumerator AttackBarrier(BarrierData barrierData)
    {
        while (isAttacking)
        {
            yield return new WaitForSeconds(2);

            if (barrierData.Health >= 0)
                barrierData.Health -= 10;

            else
            {
                isAttacking = false;
                barrierData.IsDestroyed = true;
                //TODO: Remove this
                currentBarrier.GetComponent<SpriteRenderer>().color = Color.grey;
                currentBarrier.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

    }
}
