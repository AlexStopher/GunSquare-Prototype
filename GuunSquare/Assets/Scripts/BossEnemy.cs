using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool mTurningClockwise;
    bool mMovingLeft;
    public float mSpeed;
    public float mMoveSpeed;
    public eBossPhase eCurrentPhase;

    public List<EnemyScript> lAdds;

    EnemyGenerator sEnemyGen;

    public List<BulletGenerator> lBulletGensPhaseOne;
    public List<BulletGenerator> lBulletGensPhaseTwo;
    public List<BulletGenerator> lBulletGensPhaseThree;

    // Use this for initialization
    void Start ()
    {
       
        eCurrentPhase = eBossPhase.ePhaseOne;
        mCanFire = true;
        mTurningClockwise = true;
        mMovingLeft = true;

        sEnemyGen = GameObject.FindGameObjectWithTag("Enemy Generator").GetComponent<EnemyGenerator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        rot = transform.eulerAngles.y;

        switch(eCurrentPhase)
        {
            case eBossPhase.ePhaseSetup:
                {


                    break;
                }
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

                    if (mHealth < 50)
                    {
                        eCurrentPhase = eBossPhase.ePhaseTwo;

                        //Change this to an animation into the second phase rather than a straight set rotation
                        transform.eulerAngles = new Vector3(0, 180, 0);
                    }

                    break;
                }
            case eBossPhase.ePhaseTwo:
                {
                    if (mMovingLeft == true && transform.position.x >= -9.0f)
                    {
                        transform.position = new Vector3(transform.position.x - mMoveSpeed, transform.position.y, transform.position.z);

                        if (transform.position.x < -8.2)
                            mMovingLeft = false;

                    }
                    else if (mMovingLeft == false && transform.position.x <= 9.0f)
                    {
                        transform.position = new Vector3(transform.position.x + mMoveSpeed, transform.position.y, transform.position.z);

                        if (transform.position.x > 8.2)
                            mMovingLeft = true;
                    }

                    break;
                }
            case eBossPhase.ePhaseThree:
                {
                    break;
                }
            case eBossPhase.ePhaseFourTransform:
                {
                    break;
                }
            case eBossPhase.ePhaseFour:
                {
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

        if (eCurrentPhase == eBossPhase.ePhaseOne)
        {
            for (int i = 0; i < lBulletGensPhaseOne.Count; i++)
                lBulletGensPhaseOne[i].ShootBullet();
        }
        else if (eCurrentPhase == eBossPhase.ePhaseTwo)
        {
            for (int i = 0; i < lBulletGensPhaseTwo.Count; i++)
                lBulletGensPhaseTwo[i].ShootBullet();
        }

        yield return new WaitForSeconds(speed);

        mCanFire = true;
    }

    void Movement()
    {

    }

    public void FetchFromPool()
    {
        this.gameObject.SetActive(true);

        transform.position = new Vector3(0,5,4);

    }

    public void ReturnToPool()
    {
       
        this.gameObject.SetActive(false);

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

                Destroy(this.gameObject);
            }
        }
    }
}
