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
    private Transform _porterPrefab;
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
            SpawnEnemy(_waveCount);
            yield return new WaitForSeconds(_waitTime);
        }
    }

    private void SpawnEnemy(int waveCount)
    {
        if (waveCount < 3)
        {
            float randomXB = Random.Range(-9.5f, 9.5f);
            Transform newBasicEnemy = Instantiate(_basicPrefab, new Vector3(randomXB, 8, 0), Quaternion.identity);
            newBasicEnemy.parent = GameObject.Find("Enemy_Container").transform;
            _enemyList.Add(newBasicEnemy);
        }
        else if (waveCount < 5)
        {
            float randomXB = Random.Range(-9.5f, 9.5f);
            Transform newBasicEnemy = Instantiate(_basicPrefab, new Vector3(randomXB, 8, 0), Quaternion.identity);
            newBasicEnemy.parent = GameObject.Find("Enemy_Container").transform;
            _enemyList.Add(newBasicEnemy);

            float randomXW = Random.Range(-9.5f, 9.5f);
            Transform newWeaverEnemy = Instantiate(_weaverPrefab, new Vector3(randomXW, 12, 0), Quaternion.identity);
            newBasicEnemy.parent = GameObject.Find("Enemy_Container").transform;
            _enemyList.Add(newWeaverEnemy);
        }
        else if (waveCount > 4)
        {
            float randomXB = Random.Range(-9.5f, 9.5f);
            Transform newBasicEnemy = Instantiate(_basicPrefab, new Vector3(randomXB, 8, 0), Quaternion.identity);
            newBasicEnemy.parent = GameObject.Find("Enemy_Container").transform;
            _enemyList.Add(newBasicEnemy);

            float randomXW = Random.Range(-9.5f, 9.5f);
            Transform newWeaverEnemy = Instantiate(_weaverPrefab, new Vector3(randomXW, 12, 0), Quaternion.identity);
            newBasicEnemy.parent = GameObject.Find("Enemy_Container").transform;
            _enemyList.Add(newWeaverEnemy);

            float randomXP = Random.Range(-9.5f, 9.5f);
            Transform newPorterEnemy = Instantiate(_porterPrefab, new Vector3(randomXP, 12, 0), Quaternion.identity);
            newBasicEnemy.parent = GameObject.Find("Enemy_Container").transform;
            _enemyList.Add(newPorterEnemy);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        float randomT = Random.Range(3f, 7f);
        yield return new WaitForSeconds(randomT);
        while (_stopSpawning == false)
        {
            int randomPowerUp = Random.Range(0, 7);
            if (randomPowerUp == 5)
            {
                Debug.Log("Powerup is " + randomPowerUp + "! Rolling to confirm black hole cannon");
                int randomPowerUp2 = Random.Range(0, 7);
                randomPowerUp = randomPowerUp2;
                Debug.Log("new powerup is " + randomPowerUp);
            }
            else if (randomPowerUp == 7)
            {
                Debug.Log("Powerup is " + randomPowerUp + "! Rolling to confirm slow Negaup");
                int randomPowerUp2 = Random.Range(0, 7);
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
