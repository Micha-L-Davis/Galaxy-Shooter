using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    public Text _ammoText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private GameObject _gameOverText;
//    [SerializeField]
//    private GameObject _waveIncomingText;
    [SerializeField]
    private GameObject _restartPrompt;
    private GameManager _gameManager;
    

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: " + 15 + "/15";
        _gameOverText.SetActive(false);
        _restartPrompt.SetActive(false);
        //_waveIncomingText.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    public void UpdateScoreText(int newScore)
    {
        _scoreText.text = "Score: " + newScore;
    }
    public void Ammo_UI_Update(int ammoCount)
    {
        _ammoText.text = "Ammo: " + ammoCount + "/15";
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

//    public IEnumerator WaveIncomingTextRoutine(int waveNumber)
//    {
//        while (true)
//        {
//            Text text = _waveIncomingText.GetComponent<Text>();
//            text.text = "WAVE " + waveNumber + " INCOMING!";
//            _waveIncomingText.SetActive(true);
//            yield return new WaitForSeconds(3f);
//            _waveIncomingText.SetActive(false);
//        }
//        
//
  //  }

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
