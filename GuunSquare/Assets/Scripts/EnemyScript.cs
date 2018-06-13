using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyScript : MonoBehaviour
{
    public Vector3 Target;
    public Transform PlayerTarget;
    public int Timer;
    bool CanFire;
    public int CurrentPosition = 100;
    public int LastPosition = 100;
    public int Health;

    public List<BulletGenerator> sBullGen;
    NodeManager sNodeManager;
    public GameManager sGameManager;
    public EnemyGenerator sEnemyGen;

    public float EnemySpeed;
    public float ShootSpeed;
    public eEnemyState eCurrentState;

    

    public enum eEnemyState
    {
        SeekingNode,
        Moving,
        Stationary,
        Disabled

    };

    
    // Use this for initialization
    void Start()
    {
        //EnemySpeed = 0.05f;
        PlayerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        sGameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        sNodeManager = GameObject.FindGameObjectWithTag("Nodes").GetComponent<NodeManager>();
        sEnemyGen = GameObject.FindGameObjectWithTag("Enemy Generator").GetComponent<EnemyGenerator>();
        CanFire = true;

        //if light no seekingnodes
        if (gameObject.tag == "Medium Enemy")
        {
            eCurrentState = eEnemyState.SeekingNode;
            Health = 3;
        }
        else if (gameObject.tag == "Enemy")
        {
            Health = 1;
        }

    }

    private void OnEnable()
    {
        CanFire = true;

        if (gameObject.tag == "Medium Enemy")
        {
            eCurrentState = eEnemyState.SeekingNode;
            Health = 3;
        }
        else if (gameObject.tag == "Enemy")
        {
            Health = 1;
        }
    }

    // Update is called once per frame
    void Update ()
    {

        

        if (gameObject.tag == "Medium Enemy")
        {
            
            //Enemy states 
            if (eCurrentState == eEnemyState.Stationary)
            {
                Vector3 temp = new Vector3(this.transform.rotation.x, this.transform.rotation.y + 1, transform.rotation.z);

                transform.Rotate(Vector3.up * 2, Space.World);

                //Make a proper timer here
                if (Timer >= 1000)
                    eCurrentState = eEnemyState.SeekingNode;
                else
                    Timer++;

                if (CanFire == true)
                {
                    StartCoroutine(FireBullet(ShootSpeed));
                }

            }
            else if (eCurrentState == eEnemyState.SeekingNode)
            {
                Timer = 0;
                NodeMovement();
            }
            else if (eCurrentState == eEnemyState.Moving)
            {
                transform.LookAt(Target);
                Movement();

                if (Vector3.Distance(transform.position, Target) < 0.2)
                    eCurrentState = eEnemyState.Stationary;
            }


        }
        else
        {
            transform.LookAt(PlayerTarget);

            if (Vector3.Distance(transform.position, PlayerTarget.position) > 4)
                Movement();

            if (CanFire == true)
            {
                //Debug.Log("Shoots");
                StartCoroutine(FireBullet(ShootSpeed));
            }
        }        

        
	}

   

    //Method to fire a bullet with a time delay before the next shot can be fired
    private IEnumerator FireBullet(float speed)
    {
        CanFire = false;

        for (int i = 0; i < sBullGen.Count; i++)
            sBullGen[i].ShootBullet();

        yield return new WaitForSeconds(speed);  

        CanFire = true;
    }

    //Basic movement towards the player (Replace with actual movement later)
    private void Movement()
    {
        transform.position = new Vector3((transform.position.x + transform.forward.x * EnemySpeed), 
                                              (transform.position.y + transform.forward.y * EnemySpeed),
                                              (transform.position.z + transform.forward.z * EnemySpeed));

       
    }

    //Basic movement using preset nodes. Ignores the current and last nodes when making a decision
    private void NodeMovement()
    {
        float minDistance = 1000;
        int tempPosition = 0;
        

        for(int i = 0; i < sNodeManager.GetNodePositions().Length; i++)
        {
            float tempDistance = Vector3.Distance(transform.position, sNodeManager.GetNodePositions()[i]);

            if (i == CurrentPosition || i == LastPosition)
            {

            }
            else if(tempDistance < minDistance)
            {
                minDistance = tempDistance;
                Target = sNodeManager.GetNodePositions()[i];
                tempPosition = i;
            }
        }

        LastPosition = CurrentPosition;
        CurrentPosition = tempPosition;

        eCurrentState = eEnemyState.Moving;
    }

    private void OnCollisionEnter(Collision collision)
    {

        
        //if player bullet and within a small distance IE not already hit then do this etc etc
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            if (eCurrentState != eEnemyState.Disabled)
            {
                Health--;
                collision.gameObject.GetComponent<BulletScript>().ReturnToPool();

                if (Health <= 0)
                {                  
                    sGameManager.EnemiesLeft--;
                    ReturnToPool();

                    if (this.gameObject.tag == "Medium Enemy")
                        sGameManager.Score += 300;
                    else
                        sGameManager.Score += 100;
                }
            }
            
        }
      
    }

    public void FetchFromPool(Vector3 spawnPoint)
    {
        this.gameObject.SetActive(true);

        eCurrentState = eEnemyState.SeekingNode;
        transform.position = spawnPoint;
        
    }

    public void ReturnToPool()
    {
        eCurrentState = eEnemyState.Disabled;

        if (gameObject.tag == "Medium Enemy")
            sEnemyGen.gMediumEnemy.Add(this);
        else if (gameObject.tag == "Enemy")
            sEnemyGen.gLightEnemy.Add(this);

        this.gameObject.SetActive(false);

        

        gameObject.transform.position = sEnemyGen.transform.position;
    }
}
