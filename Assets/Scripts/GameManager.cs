using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _gameIsOver = false;
    [SerializeField]
    private GameObject _asteroidPrefab;

    private void Start()
    {
        Instantiate(_asteroidPrefab, new Vector3(0, 3.7f, 0), Quaternion.identity);
    }

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
