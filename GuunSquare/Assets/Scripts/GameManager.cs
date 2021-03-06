﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int Level;
    public int EnemiesLeft;
    public EnemyGenerator sEnemyGen;
    public PowerupGenerator sPowerupGenerator;
    public EventSystem mEventSystem;
    public Canvas uiPauseMenu;
    public Canvas uiEndGame;
    public GameObject mEndGameButton;
    public Text tHighScore;
    public Text tCurrentScore;
    public Player sPlayer;

   // public TextMesh test;
    
    public int Score;
    int HighScore;

    eGameState eGameManagerState;

    
    enum eGameState
    {
        Running,
        Paused,
        Ended
    };

	// Use this for initialization
	void Start ()
    {
        
        Level = 0;
        eGameManagerState = eGameState.Running;

        if (PlayerPrefs.HasKey("HighScore"))
            HighScore = PlayerPrefs.GetInt("HighScore");

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
       // test.text = EnemiesLeft.ToString();

        if (eGameManagerState == eGameState.Running)
        {
            //Future implementation - Read from a file to generate enemy numbers based on level

            if (EnemiesLeft <= 0)
            {
                //The level will create enemy waves based off of the Level number, increasing difficulty as time goes on
                //for now do hard set levels, change this to be different for levels of x multiple
                //Change magic numbers later
                if (Level <= 3)
                {
                    //Spawn the wave
                    sEnemyGen.SpawnWave(Level, 9, 0, 0);
                    EnemiesLeft = 9;

                    //Spawn random item drop
                    if (Level % 2 == 0)
                        sPowerupGenerator.SpawnPowerup();
                }
                else if (Level > 3 && Level <= 6)
                {
                    //Spawn the wave
                    sEnemyGen.SpawnWave(Level, 7, 2, 0);
                    EnemiesLeft = 9;

                    //Spawn random item drop
                    if (Level % 2 == 0)
                        sPowerupGenerator.SpawnPowerup();
                }
                else if (Level % 7 == 0)
                {
                    //Spawn the wave
                    sEnemyGen.SpawnWave(Level, 0, 0, 9);
                    EnemiesLeft = 9;

                    //Spawn random item drop                    
                    sPowerupGenerator.SpawnPowerup();
                }
                else if (Level == 10)
                {
                    sEnemyGen.SpawnBoss();
                    EnemiesLeft = 1;
                }
                else
                {
                    //Number of enemies that will be light, decides the remaining chasing enemies
                    int tempLight = Random.Range(0, 7);

                    //Spawn the wave
                    sEnemyGen.SpawnWave(Level, tempLight, 2, (7 - tempLight));

                    EnemiesLeft = 9;

                    //Spawn random item drop
                    if (Level % 2 == 0)
                        sPowerupGenerator.SpawnPowerup();
                }

                Level++;
            }

            if (sPlayer.mHealth <= 0)
                eGameManagerState = eGameState.Ended;

            //If the player has paused change the state of the game manager and turn the menu on
            if (sPlayer.mPaused == true)
            {
                eGameManagerState = eGameState.Paused;
                uiPauseMenu.gameObject.SetActive(true);
            }
        }
        else if (eGameManagerState == eGameState.Paused)
        {
            //Allow the player to press pause again to unpause the game

            if (sPlayer.mPaused == false)
            { 
                eGameManagerState = eGameState.Running;
                uiPauseMenu.gameObject.SetActive(false);
            }
        }
        else if(eGameManagerState == eGameState.Ended)
        {
            //if the game has ended this brings up the players score and the end of game screen

            mEventSystem.SetSelectedGameObject(mEndGameButton);

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
