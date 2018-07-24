using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Rigidbody rBullet;
    public float Speed = 0.15f;
    BulletManager sBulletManager;
    
	// Use this for initialization
	void Start ()
    {
        rBullet = GetComponent<Rigidbody>();
        sBulletManager = GameObject.FindGameObjectWithTag("Bullet Generator").GetComponent<BulletManager>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        rBullet.MovePosition(new Vector3(transform.position.x + transform.forward.x * Speed, transform.position.y + transform.forward.y * Speed,
                                            transform.position.z + transform.forward.z * Speed));


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ProjectileBoundary"))
        {
            ReturnToPool();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
        else if (collision.gameObject.CompareTag("PlayerBullet") || collision.gameObject.CompareTag("EnemyBullet") || collision.gameObject.CompareTag("Boss Bullet"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }

    }

    public void ReturnToPool()
    {
        if(gameObject.CompareTag("PlayerBullet"))
        {
            gameObject.SetActive(false);
            sBulletManager.lPlayerBullet.Add(this);
        }
        else if(gameObject.CompareTag("Boss Bullet"))
        {
            gameObject.SetActive(false);
            sBulletManager.lBossBullet.Add(this);
        }
        else
        {
            gameObject.SetActive(false);
            sBulletManager.lEnemyBullet.Add(this);
        }
    }
}
