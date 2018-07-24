﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    //This enum is used for the power level of the weapons in game
    public enum eItemLevels
    {
        Level1,
        Level2,
        Level3,
    }

    Rigidbody rPlayer;

    public float Speed;
    bool CanFire = true;
    public eItemLevels eGunLevel = 0;
    public int Health = 4;
    public float RateOfFire = 1.0f;
    public float ShotSpeed;
    public bool mPaused;
    public bool UnpausedThisFrame;

    int mGunAmmo;
    bool mGunPoweredUp;
    float mGunTimer;

    public Vector3 Target;
    public List<BulletGenerator> sBulGen;
    public MeshRenderer[] HealthBar;
    public MeshRenderer Shield;
    public MeshRenderer AmmoBar;
    public Slider uiAmmoBar;

    public GameObject mHowToPlay;
    Options sOptions;
    PowerupGenerator sPowerUps;
    AudioSource audioOutput;
    public AudioClip audioBulletsound;
    public AudioClip audioShieldPickup;
    

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
        mGunAmmo = 0;

        mPaused = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        mGunPoweredUp = false;
        UnpausedThisFrame = false;

        //This is a smashed together pause function to show a potential pause function, this needs to be replaced
        if (mPaused == true)
        {
            if (Input.GetButtonDown("Submit") == true || Input.GetKeyDown(KeyCode.Escape) == true)
            {
                UnpauseGame();
            }

            if (Input.GetButtonDown("B") == true || Input.GetMouseButtonDown(0) == true)
                mHowToPlay.SetActive(false);
        }

        if (Health > 0 && mPaused == false)
        {
            //checks to see if the player chose to use a keyboard and mouse or a controller
            if (sOptions.UsingController == false)
            {
                #region KeyboardAndMouse

                //This is the movement for the keyboard, needs to be changed to a Switch to improve efficiency
                
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

                if (Input.GetMouseButton(1))
                {
                    Speed = 0.1f;
                    //Raycast effects go here
                    if (mGunAmmo > 0)
                        mGunPoweredUp = true;
                }
                else
                {
                    Speed = 0.2f;
                }

                if (Input.GetMouseButton(0) && CanFire == true)
                {
                    audioOutput.PlayOneShot(audioBulletsound);
                    StartCoroutine(FireWeapon());
                }


                if (Input.GetKeyDown(KeyCode.Escape) == true && UnpausedThisFrame == false)
                {
                    Time.timeScale = 0;

                    mPaused = true;
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

                //Check to see if the player is pressing the left trigger to enter the slower powered mode
                if(Input.GetAxis("LeftTrigger") >= 0.1f)
                {
                    Speed = 0.1f;
                    //Raycast effects go here
                    if (mGunAmmo > 0)
                        mGunPoweredUp = true;
                }
                else
                {
                    Speed = 0.2f;
                }

                //Starts a coroutine to shoot the bullets
                if (Input.GetAxis("RightTrigger") >= 0.1f && CanFire == true)
                {
                    audioOutput.PlayOneShot(audioBulletsound);
                    StartCoroutine(FireWeapon());
                }

                if(Input.GetButtonDown("Submit") == true && UnpausedThisFrame == false)
                {
                    Time.timeScale = 0;
                    Debug.Log("gets here");
                    mPaused = true;                   
                }

   
            }

        }

       
    }

    //Coroutine that handles all of the firing of the bullet generators attached to the object based on data
    IEnumerator FireWeapon()
    {
        CanFire = false;

        
        //Checks the current level of the gun and player choices to see what bullet generators to use
        if (eGunLevel == eItemLevels.Level1 || mGunPoweredUp == false)
        {
            sBulGen[0].ShootBullet();
        }
        else if (eGunLevel == eItemLevels.Level2 && mGunPoweredUp)
        {
            //Loop through the bullet generator scripts attached to this object
            for (int i = 0; i < 3; i++)
            {
                sBulGen[i].ShootBullet();               
            }

            mGunAmmo--;

            uiAmmoBar.value = mGunAmmo;

            //test code
            if (mGunAmmo <= 0)
            {
                eGunLevel = eItemLevels.Level1;               
            }

            mGunPoweredUp = false;
        }
        else if (eGunLevel == eItemLevels.Level3 && mGunPoweredUp)
        {
            //Loop through the bullet generator scripts attached to this object
            for (int i = 0; i < 5; i++)
            {
                sBulGen[i].ShootBullet();
            }

            mGunAmmo--;

            uiAmmoBar.value = mGunAmmo;

            //test code
            if (mGunAmmo <= 0)
            {
                eGunLevel = eItemLevels.Level1;
               
            }


            mGunPoweredUp = false;
        }
        //else if (eGunLevel == eItemLevels.Level4)
        //{
        //    //expand on this later - Needs more to be designed properly

        //    for (int i = 0; i < 5; i++)
        //        sBulGen[i].ShootBullet();
        //}

        yield return new WaitForSeconds(RateOfFire);
        
        CanFire = true;
    }

    //Returns power up to the object pool
    void ReturnPowerupToPool(GameObject powerup)
    {
        powerup.gameObject.SetActive(false);
        sPowerUps.lPowerup.Add(powerup);
    }

    public void UnpauseGame()
    {
        mPaused = false;
        Time.timeScale = 1;
        UnpausedThisFrame = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PowerPickup"))
        {
            if (eGunLevel <= eItemLevels.Level2)
            {
                eGunLevel++;
                mGunAmmo = 10;
                uiAmmoBar.value = mGunAmmo;
            }

            mGunPoweredUp = true;

            //Need to slowly empty this out over time
            AmmoBar.material.SetColor("_Color", Color.red);


            ReturnPowerupToPool(other.gameObject);
        }
        else if (other.CompareTag("ShieldPickup"))
        {
            Shield.enabled = true;

            audioOutput.PlayOneShot(audioShieldPickup);

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
        else if(collision.gameObject.CompareTag("Boss Bullet"))
        {
            collision.gameObject.GetComponent<BulletScript>().ReturnToPool();

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
        }
    }

}
