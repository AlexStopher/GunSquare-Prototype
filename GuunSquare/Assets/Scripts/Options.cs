using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Options : MonoBehaviour
{
    public bool UsingController;
    public Text tOptions;
    public string CurrentScene;
    public string LastScene;


    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        UsingController = true;
        CurrentScene = SceneManager.GetActiveScene().name;

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main Game")
        {
            tOptions = null;
            CurrentScene = "Main Game";
        }
        else if(SceneManager.GetActiveScene().name == "Main Menu")
        {
            LastScene = CurrentScene;
            CurrentScene = "Main Menu";
        }


        if (LastScene == "Main Game")
            Destroy(this.gameObject);

    }

    public void ChangeInputMethod()
    {

        if (UsingController == false)
        {
            UsingController = true;
            tOptions.text = "Using Controller";
        }
        else
        {
            UsingController = false;
            tOptions.text = "Using Keyboard and Mouse";
        }


    }
}
