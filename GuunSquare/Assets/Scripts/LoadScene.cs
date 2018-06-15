using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;



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
