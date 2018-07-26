using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


//Class that is used to load scenes and quit the game
public class LoadScene : MonoBehaviour {

    public void LoadGameScene(int Scene)
    {
        SceneManager.LoadScene(Scene);
    }
    
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        Application.Quit();
#endif
       
    }
}
