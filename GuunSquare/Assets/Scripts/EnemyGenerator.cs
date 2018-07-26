using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public List<Vector3> lSpawnPoints;
    public List<Vector3> lUsedSpawnPoints;

    //Replace!
  

    public List<EnemyScript> lLightEnemy;
    public List<EnemyScript> lMediumEnemy;
    public List<EnemyScript> lChasingEnemy;
    public List<BossEnemy> lBossEnemy;

    // Use this for initialization
    void Start ()
    {
        //hardcoded spawn points that the enemy generator uses
        lSpawnPoints = new List<Vector3> {
            new Vector3(-25.0f, 1.0f,-10.0f),
            new Vector3(-25.0f, 1.0f, 0.0f),
            new Vector3(-25.0f, 1.0f, 15.0f),
            new Vector3(-10.0f, 1.0f, 15.0f),
            new Vector3( 0.0f, 1.0f, 15.0f),
            new Vector3( 10.0f, 1.0f, 15.0f),
            new Vector3(25.0f, 1.0f, 15.0f),
            new Vector3(25.0f, 1.0f, 0.0f),
            new Vector3(25.0f, 1.0f, -25.0f),
            new Vector3(10.0f, 1.0f, -25.0f),
            new Vector3(0.0f, 1.0f, -25.0f),
            new Vector3(-10.0f, 1.0f, -25.0f)

        };


    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //Spawns the wave based off of the information passed in the game manager. 
    //This then distributes the enemies across the outside of the map to have a balanced spread
    public bool SpawnWave(int level, int LightEnemy, int MediumEnemy, int ChasingEnemy)
    {
        int EnemiesSpawned = 0;

        for(int i = 0; i < LightEnemy; i++)
        {
            int random = Random.Range(0, lSpawnPoints.Count - 1);

            Debug.Log(lSpawnPoints.Count);

            SpawnLightEnemy(lSpawnPoints[random], level);

            lUsedSpawnPoints.Add(lSpawnPoints[random]);
            lSpawnPoints.Remove(lSpawnPoints[random]);

            EnemiesSpawned++;
        }

        for (int i = 0; i < MediumEnemy; i++)
        {
            int random = Random.Range(0, lSpawnPoints.Count - 1);

            Debug.Log(lSpawnPoints.Count);

            SpawnMediumEnemy(lSpawnPoints[random], level);

            lUsedSpawnPoints.Add(lSpawnPoints[random]);
            lSpawnPoints.Remove(lSpawnPoints[random]);
           
            EnemiesSpawned++;
        }
        for (int i = 0; i < ChasingEnemy; i++)
        {
            int random = Random.Range(0, lSpawnPoints.Count - 1);

            Debug.Log(lSpawnPoints.Count);

            SpawnChasingEnemy(lSpawnPoints[random], level);

            lUsedSpawnPoints.Add(lSpawnPoints[random]);
            lSpawnPoints.Remove(lSpawnPoints[random]);

            EnemiesSpawned++;
        }
        
        for(int i = 0; i <= 8; i++)
        {
            lSpawnPoints.Add(lUsedSpawnPoints[0]);
            lUsedSpawnPoints.Remove(lUsedSpawnPoints[0]);
        }


        return false;
    }

    //These functions pull the related enemy type from their respective pools and spawn them
    public void SpawnBoss()
    {
       
        lBossEnemy[0].FetchFromPool();
        lBossEnemy.Remove(lBossEnemy[0]);

    }
    
    void SpawnLightEnemy(Vector3 spawnPoint, int level)
    {
        lLightEnemy[0].FetchFromPool(spawnPoint);

        if (level >= 5)
            lLightEnemy[0].Shield.enabled = true;

        lLightEnemy.Remove(lLightEnemy[0]);
    }

    void SpawnMediumEnemy(Vector3 spawnPoint, int level)
    {
        lMediumEnemy[0].FetchFromPool(spawnPoint);

        lMediumEnemy.Remove(lMediumEnemy[0]);
    }

    void SpawnChasingEnemy(Vector3 spawnPoint, int level)
    {
        lChasingEnemy[0].FetchFromPool(spawnPoint);

        if (level >= 20)
            lChasingEnemy[0].Shield.enabled = true;

        lChasingEnemy.Remove(lChasingEnemy[0]);
    }
}
