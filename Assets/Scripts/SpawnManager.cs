using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private float _waitTime = 5.0f;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject [] _powerups;

    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(randomX, 8, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_waitTime);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        float randomT = Random.Range(3f, 7f);
        yield return new WaitForSeconds(randomT);
        while (_stopSpawning == false)
        {
            int randomPowerUp = Random.Range(0, 5);
            float randomX = Random.Range(-9.5f, 9.5f);
            Instantiate(_powerups[randomPowerUp], new Vector3(randomX, 8, 0), Quaternion.identity);
            yield return new WaitForSeconds(randomT);
        }


    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
    
}
