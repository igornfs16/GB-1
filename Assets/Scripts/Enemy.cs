using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float __damage;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject[] waypoints;
    private int _currentPoint;
    private float _health;
    private NavMeshAgent _navMesh;

    private void Awake()
    {
        _health = _maxHealth;
        _navMesh = GetComponent<NavMeshAgent>();
        _currentPoint = 0;
    }

    private void Start()
    {
        if(waypoints.Length>0)
        {
            _navMesh.SetDestination(waypoints[_currentPoint].transform.position);
        }
    }

    public void Damage(float damage)
    {
        if (_health - damage <= 0)
        {

            gameObject.transform.Rotate(Vector3.left,90);
            Destroy(gameObject,3f);
        }
        else
            _health -= damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            Damage(20);
            Debug.Log(_health);
        }
        if (other.tag == "Mine" )
        {
            Damage(101);
            Debug.Log(_health);
        }
    }
    private void Update()
    {
        if (waypoints.Length > 0 && !_navMesh.hasPath)
        {
            Patrol();
        }
    }
    private void Patrol()
    {
        if(_currentPoint < waypoints.Length-1)
        {
            _currentPoint++;
            _navMesh.SetDestination(waypoints[_currentPoint].transform.position);
        }
        else
        {
            _currentPoint = -1;
        }
    }
}
