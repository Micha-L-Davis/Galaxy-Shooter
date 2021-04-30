using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private float _waitTime = 5.0f;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private int[] _powerUpSpawnProb;
    [SerializeField]
    private GameObject[] _enemies;
    private int[] _enemySpawnProb = new int[6]; //increment size for each new kind of enemy

    [SerializeField]
    private int _waveCount = 0;
    private List<GameObject> _enemyList = new List<GameObject>();
    public List<GameObject> powerUpList = new List<GameObject>();
    private bool _stopSpawningEnemies = false;
    private bool _stopSpawningPowerups = false;
    UIManager _uIManager;

    private void Start()
    {
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uIManager == null)
        {
            Debug.Log("_uIManager is Null");
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(StartWaveSpawnerRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }


    IEnumerator StartWaveSpawnerRoutine()
    {
        if (_waveCount == 0)
        {
            yield return _uIManager.WaveIncomingTextRoutine(_waveCount + 1);
        }

        yield return new WaitForSeconds(3.5f);
        while (_stopSpawningEnemies == false)
        {
            yield return SpawnWaveRoutine();
            yield return new WaitWhile(EnemyisAlive);
            yield return _uIManager.WaveIncomingTextRoutine(_waveCount + 1);

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
        AssignProbability();
        if (_waveCount == 13)
        {
            _stopSpawningEnemies = true;
            SpawnEnemy();
            yield break;
        }
        for (int i = 0; i < _waveCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(_waitTime);
        }

    }

    private void AssignProbability()
    {
        switch (_waveCount)
        {
            case 1:
                _enemySpawnProb[0] = 75;
                _enemySpawnProb[1] = 25;
                _enemySpawnProb[2] = 0;
                _enemySpawnProb[3] = 0;
                _enemySpawnProb[4] = 0;
                _enemySpawnProb[5] = 0;
                break;
            case 3:
                _enemySpawnProb[0] = 25;
                _enemySpawnProb[1] = 70;
                _enemySpawnProb[2] = 5;
                _enemySpawnProb[3] = 0;
                _enemySpawnProb[4] = 0;
                _enemySpawnProb[5] = 0;

                break;
            case 5:
                _enemySpawnProb[0] = 65;
                _enemySpawnProb[1] = 20;
                _enemySpawnProb[2] = 10;
                _enemySpawnProb[3] = 5;
                _enemySpawnProb[4] = 0;
                _enemySpawnProb[5] = 0;
                break;
            case 7:
                _enemySpawnProb[0] = 15;
                _enemySpawnProb[1] = 40;
                _enemySpawnProb[2] = 30;
                _enemySpawnProb[3] = 15;
                _enemySpawnProb[4] = 15;
                _enemySpawnProb[5] = 0;
                break;
            case 9:
                _enemySpawnProb[0] = 20;
                _enemySpawnProb[1] = 20;
                _enemySpawnProb[2] = 20;
                _enemySpawnProb[3] = 20;
                _enemySpawnProb[4] = 20;
                _enemySpawnProb[5] = 0;
                break;
            case 13:
                _enemySpawnProb[0] = 0;
                _enemySpawnProb[1] = 0;
                _enemySpawnProb[2] = 0;
                _enemySpawnProb[3] = 0;
                _enemySpawnProb[4] = 0;
                _enemySpawnProb[5] = 0;
                break;
            default:
                break;
        }

    }

    private void SpawnEnemy()
    {
        if (_waveCount == 13)
        {
            Instantiate(_enemies[5], new Vector3(0, 9, 0), Quaternion.identity);
        }
        else
        {
            int randomEnemy = Random.Range(1, 101);
            int low;
            int high = 0;
            for (int i = 0; i < _enemies.Length; i++)
            {
                low = high;
                high += _enemySpawnProb[i];
                if (randomEnemy >= low && randomEnemy < high)
                {
                    float randomX = Random.Range(-9.5f, 9.5f);
                    GameObject newEnemy = Instantiate(_enemies[i], new Vector3(randomX, 8, 0), Quaternion.identity);
                    newEnemy.transform.parent = GameObject.Find("Enemy_Container").transform;
                    _enemyList.Add(newEnemy);
                }
            }
        }

    }

    private void SpawnPowerup()
    {
        if (_uIManager.ammoText.text == "Ammo: 0/15")
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            GameObject newPowerup = Instantiate(_powerups[0], new Vector3(randomX, 8, 0), Quaternion.identity);
            powerUpList.Add(newPowerup);
        }
        else
        {
            int randomPowerUp = Random.Range(1, 101);
            int low;
            int high = 0;
            for (int i = 0; i < _powerups.Length; i++)
            {
                low = high;
                high += _powerUpSpawnProb[i];
                if (randomPowerUp >= low && randomPowerUp < high)
                {
                    float randomX = Random.Range(-9.5f, 9.5f);
                    GameObject newPowerup = Instantiate(_powerups[i], new Vector3(randomX, 8, 0), Quaternion.identity);
                    powerUpList.Add(newPowerup);
                }
            }
        }

    }

    IEnumerator SpawnPowerupRoutine()
    {
        float randomT = Random.Range(3f, 7f);
        yield return new WaitForSeconds(randomT);
        while (_stopSpawningPowerups == false)
        {
            SpawnPowerup();
            yield return new WaitForSeconds(randomT);
        }


    }

    public void OnPlayerDeath()
    {
        _stopSpawningEnemies = true;
        _stopSpawningPowerups = true;
    }
}
 