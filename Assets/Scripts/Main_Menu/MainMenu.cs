using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(1); //main game scene
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        SceneManager.LoadScene(2); //howtoplay scene
    }

    public void BacktoMenu()
    {
        SceneManager.LoadScene(0); //mainmenu scene
    }
}
