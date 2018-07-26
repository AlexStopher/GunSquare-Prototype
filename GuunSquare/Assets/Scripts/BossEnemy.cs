using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
    This Boss class is a mess, needs to be cleaned up and optimised      
*/

public class BossEnemy : MonoBehaviour
{
    public enum eBossPhase
    {
        ePhaseSetup,
        ePhaseOne,
        ePhaseTwo,
        ePhaseThree,
        ePhaseFourTransform,
        ePhaseFour
    };

    public float rot;
    public int mHealth;
    public float mRotateSpeed;
    bool mCanFire;
    bool mAddCanFire;
    bool mAltPhaseFour;
    public Slider uiHealthBar;
    public Canvas uiEnemyHud;

    public bool mTurningClockwise;
    bool mMovingLeft;
    public float mSpeed;
    public float mMoveSpeed;
    public eBossPhase eCurrentPhase;

    Vector3 Target;

    EnemyGenerator sEnemyGen;
    GameManager sGameManager;

    public List<BulletGenerator> lBulletGensPhaseOne;
    public List<BulletGenerator> lBulletGensPhaseTwo;
    public List<BulletGenerator> lBulletGensPhaseThree;
    public List<BulletGenerator> lBulletGensPhaseFour;
    public List<BulletGenerator> lBulletGensPhaseFourAlt;
    public List<BulletGenerator> lBulletGensPhaseFourAdds;

    // Use this for initialization
    void Start ()
    {
       
        eCurrentPhase = eBossPhase.ePhaseSetup;
        mCanFire = true;
        mTurningClockwise = true;
        mMovingLeft = true;
        mAddCanFire = true;
        mAltPhaseFour = false;

        Target = new Vector3(0, 5, 4);
        sEnemyGen = GameObject.FindGameObjectWithTag("Enemy Generator").GetComponent<EnemyGenerator>();
        sGameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        rot = transform.eulerAngles.y;

        //switch that uses an enum to determine what stage the boss is in and carry out different behaviours
        switch(eCurrentPhase)
        {
            //Setup phase, currently moves towards a set target and changes to phase one when in range
            case eBossPhase.ePhaseSetup:
                {
                    transform.LookAt(Target);
                    MovementForward();
                    
                    if (Vector3.Distance(this.transform.position, Target) <= 1)
                        eCurrentPhase = eBossPhase.ePhaseOne;

                    break;
                }
                //Shoots on the spot and rotates
            case eBossPhase.ePhaseOne:
                {

                    if (mTurningClockwise == true && transform.eulerAngles.y <= 270.0f)
                    {
                        transform.Rotate(new Vector3(0, mRotateSpeed, 0));

                        if (transform.eulerAngles.y >= 269.0f)
                            mTurningClockwise = false;
                    }
                    else if (mTurningClockwise == false && transform.eulerAngles.y >= 90.0f)
                    {
                        transform.Rotate(new Vector3(0, -mRotateSpeed, 0));

                        Debug.Log("Gets Here");

                        if (transform.eulerAngles.y <= 91.0f)
                            mTurningClockwise = true;
                    }

                    if (mHealth < 75)
                    {
                        eCurrentPhase = eBossPhase.ePhaseTwo;

                        //Change this to an animation into the second phase rather than a straight set rotation
                        transform.eulerAngles = new Vector3(0, 180, 0);
                    }

                    break;
                }
                //Moves left and right and shoots towards -Z
            case eBossPhase.ePhaseTwo:
                {
                    if (mMovingLeft == true && transform.position.x >= -10.0f)
                    {
                        transform.position = new Vector3(transform.position.x - mMoveSpeed, transform.position.y, transform.position.z);

                        if (transform.position.x < -9.9f)
                            mMovingLeft = false;

                    }
                    else if (mMovingLeft == false && transform.position.x <= 10.0f)
                    {
                        transform.position = new Vector3(transform.position.x + mMoveSpeed, transform.position.y, transform.position.z);

                        if (transform.position.x > 9.9f)
                            mMovingLeft = true;
                    }

                    if (mHealth < 50)
                    {
                        eCurrentPhase = eBossPhase.ePhaseFourTransform;
                    }

                    break;
                }
            case eBossPhase.ePhaseThree:
                {
                    break;
                }
                //Moves to a spot to prepare for the final phase
            case eBossPhase.ePhaseFourTransform:
                {
                    transform.LookAt(Target);
                    MovementForward();

                    if (Vector3.Distance(this.transform.position, Target) <= 1)
                    {
                        transform.eulerAngles = new Vector3(0, 180, 0);
                        eCurrentPhase = eBossPhase.ePhaseFour;
                    }
                        

                    break;
                }
            case eBossPhase.ePhaseFour:
                {
                    mSpeed = 2.0f;

                    if (mAddCanFire)
                    {
                        StartCoroutine(FireAddsBullets(0.3f));

                    }
                    break;
                }
            default:
                break;
        }     

        if (mCanFire)
        {
            StartCoroutine(FireBullet(mSpeed));

        }
	}

    //Method to fire a bullet with a time delay before the next shot can be fired
    private IEnumerator FireBullet(float speed)
    {
        mCanFire = false;
        uiHealthBar.value = mHealth;

        //Fires bullets based off of the current phase
        switch(eCurrentPhase)
        {

            case eBossPhase.ePhaseOne:
                {
                    for (int i = 0; i < lBulletGensPhaseOne.Count; i++)
                           lBulletGensPhaseOne[i].ShootBullet();

                    break;
                }
            case eBossPhase.ePhaseTwo:
                {
                     for (int i = 0; i < lBulletGensPhaseTwo.Count; i++)
                            lBulletGensPhaseTwo[i].ShootBullet();

                     break;
                }
            case eBossPhase.ePhaseFour:
                {
                     if(mAltPhaseFour == false)
                     {
                        for (int i = 0; i < lBulletGensPhaseFour.Count; i++)
                            lBulletGensPhaseFour[i].ShootBullet();

                        mAltPhaseFour = true;
                     }
                    else
                    {
                        for (int i = 0; i < lBulletGensPhaseFourAlt.Count; i++)
                            lBulletGensPhaseFourAlt[i].ShootBullet();

                        mAltPhaseFour = false;
                    }

                    break;
                }
            default:
                break;
        }
        //if (eCurrentPhase == eBossPhase.ePhaseOne)
        //{
        //    for (int i = 0; i < lBulletGensPhaseOne.Count; i++)
        //        lBulletGensPhaseOne[i].ShootBullet();
        //}
        //else if (eCurrentPhase == eBossPhase.ePhaseTwo)
        //{
        //    for (int i = 0; i < lBulletGensPhaseTwo.Count; i++)
        //        lBulletGensPhaseTwo[i].ShootBullet();
        //}
        //else if (eCurrentPhase == eBossPhase.ePhaseFour && mAltPhaseFour == false)
        //{
        //    for (int i = 0; i < lBulletGensPhaseFour.Count; i++)
        //        lBulletGensPhaseFour[i].ShootBullet();

        //    mAltPhaseFour = true;
        //}
        //else if (eCurrentPhase == eBossPhase.ePhaseFour && mAltPhaseFour == true)
        //{
        //    for (int i = 0; i < lBulletGensPhaseFourAlt.Count; i++)
        //        lBulletGensPhaseFourAlt[i].ShootBullet();

        //    mAltPhaseFour = false;
        //}

        yield return new WaitForSeconds(speed);

        mCanFire = true;
    }

    //Fires the bullets for the smaller boss units, mainly for phase four
    private IEnumerator FireAddsBullets(float speed)
    {
        mAddCanFire = false;

        for (int i = 0; i < lBulletGensPhaseFourAdds.Count; i++)
            lBulletGensPhaseFourAdds[i].ShootBullet();

        yield return new WaitForSeconds(speed);

        mAddCanFire = true;
    }

    //Basic movement for moving to points
    void MovementForward()
    {
        transform.position = new Vector3((transform.position.x + transform.forward.x * 0.1f),
                                             (transform.position.y + transform.forward.y * 0.1f),
                                             (transform.position.z + transform.forward.z * 0.1f));
    }

    //grab object from pool
    public void FetchFromPool()
    {
        //Enable the game objects
        this.gameObject.SetActive(true);
        uiEnemyHud.gameObject.SetActive(true);
        uiHealthBar.gameObject.SetActive(true);

        //Set boss just outside of view
        transform.position = new Vector3(0,5,15);

    }

    //return to pool
    public void ReturnToPool()
    {
       //set game objects to false
        this.gameObject.SetActive(false);
        uiEnemyHud.gameObject.SetActive(false);
        uiHealthBar.gameObject.SetActive(false);

        //add game object back to pool
        sEnemyGen.lBossEnemy.Add(this);
       
        gameObject.transform.position = sEnemyGen.transform.position;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("PlayerBullet"))
        {
            mHealth--;
            collision.gameObject.GetComponent<BulletScript>().ReturnToPool();

            if(mHealth <= 0)
            {
                //Play death animation, sounds, and scripts. Return to pool

                sGameManager.EnemiesLeft--;
                ReturnToPool();
            }
        }
    }
}
