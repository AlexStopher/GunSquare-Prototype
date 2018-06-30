using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupGenerator : MonoBehaviour
{
    public List<GameObject> lPowerup;
	// Use this for initialization
	void Start ()
    {
		
	}

    public void SpawnPowerup()
    {


        //Spawn random powerup and place it within a random zone
        if (lPowerup.Count > 0)
        {
            int i = Random.Range(0, lPowerup.Count);
            int j = Random.Range(-10, 10);
            int k = Random.Range(-10, 8);

            FetchFromPool(new Vector3(j, 1, k), i);
        }
    }

    void FetchFromPool(Vector3 position, int item)
    {
        lPowerup[item].SetActive(true);
        lPowerup[item].transform.position = position;

        lPowerup.Remove(lPowerup[item]);
    }
}
