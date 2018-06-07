using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    public GameObject Bullet;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Get the next object from the pool

        //Prototype code
        

	}

    public void ShootBullet()
    {
        Instantiate<GameObject>(Bullet, this.transform.position, this.transform.rotation);
    }

}
