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

    [Header("Enemy Statues")]
    // percentage, (1 - cuurentcold) is how much you slow the enemy by
    public float currentCold;
    // could be the damge the enemy takes per second
    public float currentFire;

    private List<EnemyStatus> statues;

    // This seems better because allows us to change speed and reverse it
    Vector3 startPosition, endPosition, dir;

    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }


    /////////////////////////////////////////////////////////////////

    private void Awake()
    {
        statues = new List<EnemyStatus>();
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
        ProcessStatues();
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

        transform.position += dir.normalized * GetMovementSpeed() * Time.fixedDeltaTime;

        //If Close enough to base
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
        
        //Running Away From base? Fuck it you die.
        if (Vector3.Distance(gameObject.transform.position, playerStats.baseNode.transform.position) > 400f)
        {
            //Base Damage Destruction
            Destroy(gameObject);
        }

    }

    /// <summary>
    /// Updates all status cooldowns and take care of activating and desactivating them.
    /// </summary>
    private void ProcessStatues()
    {
        float timePassed = Time.fixedDeltaTime;
        currentCold = 0;
        currentFire = 0;
        foreach (EnemyStatus es in statues)
        {
            switch (es.status)
            {
                case ENEMY_STATUS.COLD:
                    if (es.statusEffect > currentCold)
                        currentCold = es.statusEffect;
                    break;
                case ENEMY_STATUS.FIRE:
                    if (es.statusEffect > currentFire)
                        currentFire = es.statusEffect;
                    break;
            }

            es.countdown -= timePassed;
        }
        for(int i = 0; i < statues.Count; i++)
        {
            if(statues[i].countdown <= 0f)
            {
                statues.RemoveAt(i);
                i--;
            }
        }

        // depending on the statues affecting the enmy, change the color of the sprite.
        if (currentCold > 0f)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }


    }


    private float GetMovementSpeed()
    {
        float currentSpeed = speed;
        currentSpeed = currentSpeed * (1 - currentCold);
        return currentSpeed;
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
    public void TakeDamage(int damage)
    {
        //Normalize float vs int???
        CurrentHealth -= damage;

        //Armor?????? TO DO
        
        //TEXT
        PopupText(damage);
        
        if (CurrentHealth <= 0)
        {
            //print("Death takes me...");
            Destroy(gameObject);
        }
    }

    private void PopupText(int damage)
    {
        //Normalize float vs int???
        int normalDamage = damage;
        
        //Get From PLayer Stasts
        GameObject textPrefab = playerStats.popupDamageText_Prefab;


        //Create
        GameObject popupText = Instantiate(textPrefab, transform.position, Quaternion.identity);

        //Setup
        DamagePopup dmgPopup = popupText.GetComponent<DamagePopup>();


        dmgPopup.Setup(normalDamage);

    }

   
    /// <summary>
    /// Add a temporary slow effect the to monster
    /// </summary>
    /// <param name="slowSpeed">Must bewteen 0 and 1, and indicates the percentage that the enemy is slowed</param>
    /// <param name="slowTimer">Determines how long the slow lasts</param>
    public void AddSlow(float slowSpeed, float slowTimer)
    {
        EnemyStatus newStatus = new EnemyStatus();

        newStatus.countdown = slowTimer;
        if (gameObject.name.Contains("BOSS"))
        {
            newStatus.countdown /= 2;
        }
        newStatus.statusEffect = slowSpeed;

        statues.Add(newStatus);

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
