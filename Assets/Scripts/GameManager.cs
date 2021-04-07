using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _gameIsOver = false;

     void Update()
    {
        if (_gameIsOver == true && Input.GetKeyDown(KeyCode.R))
        {
           
            SceneManager.LoadScene(1); //current scene
         
        }
    }
    public void GameIsOver()
    {
        _gameIsOver = true;
    }
}
