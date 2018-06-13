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

    public void SpawnPowerup()
    {
        //Replace this with object pooling, if one power up is active then it cannot be on the screen twice etc etc
      
        //Spawn random powerup and place it within a random zone
        int i = Random.Range(0, PowerupList.Length);
        int j = Random.Range(-10, 10);
        int k = Random.Range(-10,10);

        Instantiate<GameObject>(PowerupList[i], new Vector3(j, 1, k), Quaternion.identity);
    }
}
