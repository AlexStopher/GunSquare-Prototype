using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupGenerator : MonoBehaviour
{
    public GameObject[] PowerupList;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SpawnPowerup()
    {
        //PowerupList.Length
        //Spawn random powerup
        int i = Random.Range(0, PowerupList.Length);

        Instantiate<GameObject>(PowerupList[i], new Vector3(0, 1, 0), Quaternion.identity);
    }
}
