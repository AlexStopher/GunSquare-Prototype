using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int Level;
    public int EnemiesLeft;
    public EnemyGenerator sEnemyGen;
    public PowerupGenerator sPowerupGenerator;
    public Canvas uiEndGame;
    public Text tHighScore;
    public Text tCurrentScore;
    public Player sPlayer;
    
    public int Score;
    int HighScore;

    eGameState eGameManagerState;

    enum eGameState
    {
        Running,
        Ended
    };

	// Use this for initialization
	void Start ()
    {
        Level = 0;
        eGameManagerState = eGameState.Running;

        if (PlayerPrefs.HasKey("HighScore"))
            HighScore = PlayerPrefs.GetInt("HighScore");
    }

    // Update is called once per frame
    void Update()
    { 
      
        if (eGameManagerState == eGameState.Running)
        {
            //Future implementation - Read from a file to generate enemy numbers based on level

            if (EnemiesLeft <= 0)
            {
                //for now do hard set levels, change this to be different for levels of x multiple
                    if (Level < 3)
                    {
                        sEnemyGen.SpawnWave(Level, 9, 0, 0);
                        EnemiesLeft = 9;

                        //Spawn random item drop
                        sPowerupGenerator.SpawnPowerup();
                    }
                    else if (Level > 2 && Level < 6)
                    {
                        sEnemyGen.SpawnWave(Level, 7, 2, 0);
                        EnemiesLeft = 9;

                        //Spawn random item drop
                        sPowerupGenerator.SpawnPowerup();
                    }
                    else
                    {
                        int tempLight = Random.Range(0, 7);
                    
                    sEnemyGen.SpawnWave(Level, tempLight, 2, (7 - tempLight));

                    EnemiesLeft = 9;

                    //Spawn random item drop
                    sPowerupGenerator.SpawnPowerup();
                }

                Level++;
            }

            if (sPlayer.Health <= 0)
                eGameManagerState = eGameState.Ended;

            
        }
        else if(eGameManagerState == eGameState.Ended)
        {
            tCurrentScore.text = Score.ToString();

            if (HighScore >= Score)
                tHighScore.text = HighScore.ToString();
            else if(HighScore < Score)
            {
                tHighScore.text = Score.ToString();

                PlayerPrefs.SetInt("HighScore", Score);
            }

            uiEndGame.gameObject.SetActive(true);
            
        }

	}


}
