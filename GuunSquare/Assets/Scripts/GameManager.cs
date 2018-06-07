using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int EnemiesLeft;
    public EnemyGenerator sEnemyGen;
    public PowerupGenerator sPowerupGenerator;
    eGameState eGameManagerState;
    public Player sPlayer;

    enum eGameState
    {
        Running,
        Ended
    };

	// Use this for initialization
	void Start ()
    {
        eGameManagerState = eGameState.Running;
	}

    // Update is called once per frame
    void Update()
    { 
      
        if (eGameManagerState == eGameState.Running)
        {


            if (EnemiesLeft <= 0)
            {
                sEnemyGen.SpawnWave(1, 7, 2, 0);
                EnemiesLeft = 9;
                //Spawn random item drop
                sPowerupGenerator.SpawnPowerup();

            }

            if (sPlayer.Health <= 0)
                eGameManagerState = eGameState.Ended;

            
        }
        else if(eGameManagerState == eGameState.Ended)
        {
            SceneManager.LoadScene(1);
        }

	}


}
