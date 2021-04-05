using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemySpawnPoints;
    [SerializeField] private GameObject _enemyInfantryPrefab;

    // Start is called before the first frame update
    void Start()
    {
        EnemySpawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnemySpawn()
    {
        for(int i=0; i< _enemySpawnPoints.Length; i++)
        {
            Instantiate(_enemyInfantryPrefab, _enemySpawnPoints[i].transform);
        }
    }
}
