using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum eItemLevels
    {
        Level1,
        Level2,
        Level3,
        Level4
    }

    Rigidbody rPlayer;

    public float Speed;
    bool CanFire = true;
    public eItemLevels eGunLevel = 0;
    public int Health = 4;
    public float RateOfFire = 1.0f;
    public float ShotSpeed;

    bool mGunPoweredUp;
    float mGunTimer;

    public Vector3 Target;
    public List<BulletGenerator> sBulGen;
    public MeshRenderer[] HealthBar;
    public MeshRenderer Shield;

    Options sOptions;
    PowerupGenerator sPowerUps;
    AudioSource audioOutput;
    public AudioClip Bulletsound;
    

    // Use this for initialization
    void Start ()
    {
        rPlayer = GetComponent<Rigidbody>();
        Speed = 0.2f;
        audioOutput = GetComponent<AudioSource>();
        sPowerUps = GameObject.FindGameObjectWithTag("Powerup Generator").GetComponent<PowerupGenerator>();
        sOptions = GameObject.FindGameObjectWithTag("Options").GetComponent<Options>();

        mGunPoweredUp = false;
        mGunTimer = 5.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Health > 0)
        {
            //checks to see if the player chose to use a keyboard and mouse or a controller
            if (sOptions.UsingController == false)
            {
                #region KeyboardAndMouse
                if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
                {
                    //rPlayer.MovePosition(transform.position + new Vector3(-0.5f, 0.0f, 0.5f) * 0.75f);
                    transform.position = new Vector3(transform.position.x - Speed, transform.position.y, transform.position.z + Speed);
                }
                else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
                {
                    //rPlayer.MovePosition(transform.position + new Vector3(0.5f, 0.0f, 0.5f) * 0.75f);
                    transform.position = new Vector3(transform.position.x + Speed, transform.position.y, transform.position.z + Speed);
                }
                else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
                {
                    //rPlayer.MovePosition(transform.position + new Vector3(-0.5f, 0.0f, -0.5f) * 0.75f);
                    transform.position = new Vector3(transform.position.x - Speed, transform.position.y, transform.position.z - Speed);
                }
                else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
                {
                    //rPlayer.MovePosition(transform.position + new Vector3(0.5f, 0.0f, -0.5f) * 0.75f);
                    transform.position = new Vector3(transform.position.x + Speed, transform.position.y, transform.position.z - Speed);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    //rPlayer.MovePosition(transform.position + Vector3.forward * 0.5f);
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Speed);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    //rPlayer.MovePosition(transform.position + Vector3.back * Speed);
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Speed);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    //rPlayer.MovePosition((transform.position + Vector3.right) * 0.5f);
                    transform.position = new Vector3(transform.position.x + Speed, transform.position.y, transform.position.z);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    //rPlayer.MovePosition(transform.position + Vector3.left * 0.5f);
                    transform.position = new Vector3(transform.position.x - Speed, transform.position.y, transform.position.z);
                }

                Vector3 MousePosition = Input.mousePosition;

                Vector3 Raycast = MousePosition - transform.position;

                Target = Camera.main.ScreenToWorldPoint(Raycast);
                Target.y = 1;
                //new Vector3(transform.position.x + Raycast.x, transform.position.y, transform.position.z + Raycast.z);

                transform.LookAt(Target, Vector3.up);


                if (Input.GetMouseButton(0) && CanFire == true)
                {
                    audioOutput.PlayOneShot(Bulletsound);
                    StartCoroutine(FireWeapon());
                }




                #endregion
            }
            else if (sOptions.UsingController == true)
            {
                //Player movement based off of left analogue stick position
                rPlayer.MovePosition(transform.position + new Vector3(Input.GetAxis("HorizontalLeft"), 0, Input.GetAxis("VerticalLeft")) * Speed);

                //Set a point to look at based off of right analogue stick movement
                Target = new Vector3(transform.position.x + Input.GetAxis("HorizontalRight"), transform.position.y, transform.position.z + Input.GetAxis("VerticalRight"));

                transform.LookAt(Target, Vector3.up);


                if ((Input.GetAxis("RightTrigger") <= -0.1 || Input.GetAxis("RightTrigger2") <= -0.1) && CanFire == true)
                {
                    audioOutput.PlayOneShot(Bulletsound);
                    StartCoroutine(FireWeapon());
                }
            }

            //Timer before gun level drops
            if(mGunPoweredUp == true)
            {
                mGunTimer -= Time.deltaTime;

                if(mGunTimer <= 0)
                {
                    mGunPoweredUp = false;
                    eGunLevel = eItemLevels.Level1;
                    mGunTimer = 5.0f;
                }
            }
        }

    }

    IEnumerator FireWeapon()
    {
        CanFire = false;

        
        //Checks the current level of the gun to see what bullet generators to use
        if (eGunLevel == eItemLevels.Level1)
        {
            sBulGen[0].ShootBullet();
        }
        else if (eGunLevel == eItemLevels.Level2)
        {
            for (int i = 0; i < 3; i++)
                sBulGen[i].ShootBullet();
        }
        else if (eGunLevel == eItemLevels.Level3)
        {
            for (int i = 0; i < 5; i++)
                sBulGen[i].ShootBullet();
        }
        else if (eGunLevel == eItemLevels.Level4)
        {
            //expand on this later
            for (int i = 0; i < 5; i++)
                sBulGen[i].ShootBullet();
        }

        yield return new WaitForSeconds(RateOfFire);
        
        CanFire = true;
    }

    //Returns power up to the object pool
    void ReturnPowerupToPool(GameObject powerup)
    {
        powerup.gameObject.SetActive(false);
        sPowerUps.lPowerup.Add(powerup);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PowerPickup"))
        {
            if(eGunLevel <= eItemLevels.Level3)
                eGunLevel++;

            mGunPoweredUp = true;

            ReturnPowerupToPool(other.gameObject);
        }
        else if (other.CompareTag("ShieldPickup"))
        {
            Shield.enabled = true;

            ReturnPowerupToPool(other.gameObject);
        }
        else if (other.CompareTag("SpeedPickup"))
        {
            if (Speed < 0.5f)
                Speed += 0.1f;

            ReturnPowerupToPool(other.gameObject);
        }
        else if (other.CompareTag("HealthPickup"))
        {
            Health = 4;
            for(int i = 0; i < HealthBar.Length; i++)
                HealthBar[i].material.SetColor("_Color", Color.green);

            ReturnPowerupToPool(other.gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("EnemyBullet"))
        {
            collision.gameObject.GetComponent<BulletScript>().ReturnToPool();

            if (Shield.enabled == true)
            {
                Shield.enabled = false;
            }
            else
            {
                if(Health > 0)
                    Health--;

                HealthBar[Health].material.SetColor("_Color", Color.red);
            }
        }
        else if(collision.gameObject.CompareTag("Chasing Enemy"))
        {
            if (Shield.enabled == true)
            {
                Shield.enabled = false;
            }
            else
            {
                if (Health > 0)
                    Health--;

                HealthBar[Health].material.SetColor("_Color", Color.red);
            }

            HealthBar[Health].material.SetColor("_Color", Color.red);
        }
    }

}
