using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    public bool IsPlayer;
    public GameObject Bullet;
    BulletManager sBulletManager;

	// Use this for initialization
	void Start ()
    {
        sBulletManager = GameObject.FindGameObjectWithTag("Bullet Generator").GetComponent<BulletManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Get the next object from the pool

        //Prototype code
        

	}

    public void ShootBullet()
    {
        if (IsPlayer)
        {
            sBulletManager.lPlayerBullet[0].gameObject.transform.position = this.transform.position;
            sBulletManager.lPlayerBullet[0].gameObject.transform.rotation = this.transform.rotation;
            sBulletManager.lPlayerBullet[0].gameObject.SetActive(true);
            sBulletManager.lPlayerBullet.Remove(sBulletManager.lPlayerBullet[0]);
        }
        else
        {
            sBulletManager.lEnemyBullet[0].gameObject.transform.position = this.transform.position;
            sBulletManager.lEnemyBullet[0].gameObject.transform.rotation = this.transform.rotation;
            sBulletManager.lEnemyBullet[0].gameObject.SetActive(true);
            sBulletManager.lEnemyBullet.Remove(sBulletManager.lEnemyBullet[0]);
        }
    }




}
