using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    Vector3[] SpawnPoints;
    public List<GameObject> Enemies;

    public List<EnemyScript> gLightEnemy;
    public List<EnemyScript> gMediumEnemy;
    public GameObject gHeavyEnemy;

    // Use this for initialization
    void Start ()
    {
        SpawnPoints = new Vector3[] {
            new Vector3(-25.0f, 1.0f,-10.0f),
            new Vector3(-25.0f, 1.0f, 0.0f),
            new Vector3(-25.0f, 1.0f, 15.0f),
            new Vector3(-10.0f, 1.0f, 15.0f),
            new Vector3( 0.0f, 1.0f, 15.0f),
            new Vector3( 10.0f, 1.0f, 15.0f),
            new Vector3(25.0f, 1.0f, 15.0f),
            new Vector3(25.0f, 1.0f, 0),
            new Vector3(25.0f, 1.0f, -10)
        };


    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //to do - Track what spawn points are used to make sure enemies are spread properly
    public bool SpawnWave(int level, int LightEnemy, int MediumEnemy, int HeavyEnemy)
    {
        int EnemiesSpawned = 0;

        for(int i = 0; i < LightEnemy; i++)
        {
            int random = Random.Range(0, 8);


            SpawnLightEnemy(SpawnPoints[random]);
            //Instantiate<GameObject>(gLightEnemy, SpawnPoints[random], Quaternion.identity);
            
            EnemiesSpawned++;
        }

        for (int i = 0; i < MediumEnemy; i++)
        {
            int random = Random.Range(0, 8);


            SpawnMediumEnemy(SpawnPoints[random]);
            //Instantiate<GameObject>(gMediumEnemy, SpawnPoints[random], Quaternion.identity);
            EnemiesSpawned++;
        }
        for (int i = 0; i < HeavyEnemy; i++)
        {
            //To do when Boss Enemies are implemented
            EnemiesSpawned++;
        }
        
        return false;
    }
    
    void SpawnLightEnemy(Vector3 spawnPoint)
    {
        gLightEnemy[0].FetchFromPool(spawnPoint);     

        gLightEnemy.Remove(gLightEnemy[0]);
    }

    void SpawnMediumEnemy(Vector3 spawnPoint)
    {
        gMediumEnemy[0].FetchFromPool(spawnPoint);

        gMediumEnemy.Remove(gMediumEnemy[0]);
    }
}
