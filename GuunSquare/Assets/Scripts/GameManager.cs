using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{

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
        eGameManagerState = eGameState.Running;

        if (PlayerPrefs.HasKey("HighScore"))
            HighScore = PlayerPrefs.GetInt("HighScore");
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
