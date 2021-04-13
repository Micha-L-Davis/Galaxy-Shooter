using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private Transform _basicPrefab;
    [SerializeField]
    private Transform _weaverPrefab;
    [SerializeField]
    private float _waitTime = 5.0f;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject [] _powerups;
    [SerializeField]
    private int _waveCount = 0;
    private List<Transform> _enemyList = new List<Transform>();
    private bool _stopSpawning = false;
    UIManager _uIManager;
    
    public void StartSpawning()
    {
        StartCoroutine(StartWaveSpawnerRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

   
    IEnumerator StartWaveSpawnerRoutine()
    {
        yield return new WaitForSeconds(3.5f);
        while (_stopSpawning == false)
        {
            //spawning the wave
            yield return SpawnWaveRoutine();
            //waiting for everything to die
            yield return new WaitWhile(EnemyisAlive);
            //run the wave incoming text
            //yield return EndOfWaveRoutine();
            yield return new WaitForSeconds(_waitTime);
        }
    }

    private bool EnemyisAlive()
    {
        _enemyList = _enemyList.Where(e => e != null).ToList();
        return _enemyList.Count > 0;
    }

    IEnumerator SpawnWaveRoutine()
    {
        _waveCount++;
        for (int i = 0; i < _waveCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(_waitTime);
        }
    }

    private void SpawnEnemy()
    {
        float randomX = Random.Range(-9.5f, 9.5f);
        _enemyList.Add(Instantiate(_basicPrefab, new Vector3(randomX, 8, 0), Quaternion.identity));

        randomX = Random.Range(-9.5f, 9.5f);
        _enemyList.Add(Instantiate(_weaverPrefab, new Vector3(randomX, 12, 0), Quaternion.identity));
    }

    IEnumerator SpawnPowerupRoutine()
    {
        float randomT = Random.Range(3f, 7f);
        yield return new WaitForSeconds(randomT);
        while (_stopSpawning == false)
        {
            int randomPowerUp = Random.Range(0, 6);
            if (randomPowerUp == 5)
            {
                Debug.Log("Powerup is " + randomPowerUp + "! Rolling to confirm black hole cannon");
                int randomPowerUp2 = Random.Range(3, 6);
                randomPowerUp = randomPowerUp2;
                Debug.Log("new powerup is " + randomPowerUp);
            }
            float randomX = Random.Range(-9.5f, 9.5f);
            Instantiate(_powerups[randomPowerUp], new Vector3(randomX, 8, 0), Quaternion.identity);
            yield return new WaitForSeconds(randomT);
        }


    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
 
    //IEnumerator EndOfWaveRoutine()
    //{
    //    _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    //    if (_uIManager != null)
    //    {
    //        _uIManager.WaveIncomingTextRoutine(_waveCount);
    //    }
    //    yield return new WaitForSeconds(4);
    //}
}
