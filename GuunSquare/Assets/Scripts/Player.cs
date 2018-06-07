﻿using System.Collections;
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

    public Vector3 Target;
    public List<BulletGenerator> sBulGen;
    public MeshRenderer[] HealthBar;

    

    // Use this for initialization
    void Start ()
    {
        rPlayer = GetComponent<Rigidbody>();
        Speed = 0.2f;
	}
	
	// Update is called once per frame
	void Update ()
    {

        //if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
        //{
        //    rPlayer.MovePosition(transform.position + new Vector3(-0.5f, 0.0f, 0.5f) * 0.75f);
        //}
        //else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
        //{
        //    rPlayer.MovePosition(transform.position + new Vector3(0.5f, 0.0f, 0.5f) * 0.75f);
        //}
        //else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
        //{
        //    rPlayer.MovePosition(transform.position + new Vector3(-0.5f, 0.0f, -0.5f) * 0.75f);
        //}
        //else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
        //{
        //    rPlayer.MovePosition(transform.position + new Vector3(0.5f, 0.0f, -0.5f) * 0.75f);
        //}
        //else if (Input.GetKey(KeyCode.W))
        //{
        //    rPlayer.MovePosition(transform.position + Vector3.forward * 0.5f);
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    rPlayer.MovePosition(transform.position + Vector3.right * 0.5f);
        //}
        //else if (Input.GetKey(KeyCode.A))
        //{
        //    rPlayer.MovePosition(transform.position + Vector3.left * 0.5f);
        //}

        rPlayer.MovePosition(transform.position + new Vector3(Input.GetAxis("HorizontalLeft"), 0, -Input.GetAxis("VerticalLeft")) * Speed);

        Target = new Vector3(transform.position.x + Input.GetAxis("HorizontalRight"), transform.position.y, transform.position.z + -Input.GetAxis("VerticalRight"));

        transform.LookAt(Target, Vector3.up);

        if(Input.GetKey(KeyCode.E) && CanFire == true)
        {
            StartCoroutine(FireWeapon());
        }

        if(Input.GetAxis("RightTrigger") >= 0.1 && CanFire == true)
        {
            StartCoroutine(FireWeapon());
        }

    }

    IEnumerator FireWeapon()
    {
        CanFire = false;


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
            for (int i = 0; i < 5; i++)
                sBulGen[i].ShootBullet();
        }

        yield return new WaitForSeconds(RateOfFire);

        CanFire = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PowerPickup"))
        {
            if(eGunLevel <= eItemLevels.Level3)
                eGunLevel++;

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("FireRatePickup"))
        {
            if (RateOfFire > 0.5f)
                RateOfFire -= 0.1f;

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("SpeedPickup"))
        {
            if (Speed < 0.5f)
                Speed += 0.1f;

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("HealthPickup"))
        {
            Health = 4;
            for(int i = 0; i < HealthBar.Length; i++)
                HealthBar[i].material.SetColor("_Color", Color.green);

            Destroy(other.gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);
            Health--;

            HealthBar[Health].material.SetColor("_Color", Color.red);

        }

        if (collision.gameObject.CompareTag("Boundary"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
