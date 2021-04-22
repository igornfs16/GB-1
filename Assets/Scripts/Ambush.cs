using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambush : MonoBehaviour
{
    [SerializeField] private int _enemyNumber = 5;
    [SerializeField] private float _respawnRadius = 5;
    [SerializeField] private GameObject _spawnPointBase;
    [SerializeField] private GameObject _enemyPrefab;
    private bool _active = true;

    void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && _active)
        {
            Debug.Log(_spawnPointBase.transform.position);
            for (int i = 0; i < _enemyNumber; i++)
            {
                float randX = Random.Range(-_respawnRadius, _respawnRadius);
                float randZ = Random.Range(-_respawnRadius, _respawnRadius);
                Vector3 pos = new Vector3(randX, 0, randZ);
                Instantiate(_enemyPrefab, _spawnPointBase.transform.position + pos, _spawnPointBase.transform.rotation);
                _active = false;
            }
        }
    }
}
