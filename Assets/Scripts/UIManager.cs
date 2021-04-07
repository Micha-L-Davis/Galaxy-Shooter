using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private GameObject _gameOverText;
    [SerializeField]
    private GameObject _restartPrompt;
    private GameManager _gameManager;
    

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.SetActive(false);
        _restartPrompt.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    public void UpdateScoreText(int newScore)
    {
        _scoreText.text = "Score: " + newScore;
    }
    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
        if(currentLives == 0)
        {
            _restartPrompt.SetActive(true);
            _gameManager.GameIsOver();
            StartCoroutine(GameOverTextRoutine());
        }
    }
    IEnumerator GameOverTextRoutine()
    {
        while (true)
        {
            _gameOverText.SetActive(true);
            yield return new WaitForSeconds(.25f);
            _gameOverText.SetActive(false);
            yield return new WaitForSeconds(.25f);
        } 
    }
}
