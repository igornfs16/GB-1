using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _enemyInfantryPrefab;
    private GameObject[] _enemyInfantrySpawnPoints;

    [SerializeField] private GameObject _enemyTurretPrefab;
    private GameObject[] _enemyTurretSpawnPoints;

    [SerializeField] private GameObject _enemyTankPrefab;
    private GameObject[] _enemyTankSpawnPoints;

    [SerializeField] private GameObject _ammoPrefab;
    private GameObject[] _ammoSpawnPoints;

    [SerializeField] private GameObject _repairPrefab;
    private GameObject[] _repairSpawnPoints;


    void Awake()
    {
        _enemyInfantrySpawnPoints = GameObject.FindGameObjectsWithTag("InfantrySpawnPoint");
        _enemyTurretSpawnPoints = GameObject.FindGameObjectsWithTag("TurretSpawnPoint");
        _enemyTankSpawnPoints = GameObject.FindGameObjectsWithTag("TankSpawnPoint");
        _ammoSpawnPoints = GameObject.FindGameObjectsWithTag("AmmoSpawnPoint");
        _repairSpawnPoints = GameObject.FindGameObjectsWithTag("RepairSpawnPoint");
        Spawn(_enemyInfantryPrefab, _enemyInfantrySpawnPoints);
        Spawn(_enemyTurretPrefab, _enemyTurretSpawnPoints);
        Spawn(_enemyTankPrefab, _enemyTankSpawnPoints);
        Spawn(_ammoPrefab, _ammoSpawnPoints);
        Spawn(_repairPrefab, _repairSpawnPoints);
    }

    void Spawn(GameObject prefab, GameObject[] spawnPoints)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Instantiate(prefab, spawnPoints[i].transform);
        }
    }

}
