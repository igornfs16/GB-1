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
    [SerializeField] private float _sightRange = 30;
    [SerializeField] private float _attackRange = 15;
    [SerializeField] private float _fireSpeed = 15;

    private Transform _gunFirePossition;
    private bool _playerDetected = false;
    private int _currentPoint;
    private float _health;
    private NavMeshAgent _navMesh;
    private Transform _spawnPoint;
    private GameObject _player;
    private float _lastTime;

    private void Awake()
    {
        _gunFirePossition = transform.Find("GunPossition").transform;
        _health = _maxHealth;
        _navMesh = GetComponent<NavMeshAgent>();
        _currentPoint = 0;
        _player = GameObject.FindGameObjectWithTag("Player");
        _spawnPoint = transform;
    }

    private void Start()
    {
        if (waypoints.Length-1 > 0)
        {
            _navMesh.SetDestination(waypoints[_currentPoint].transform.position);
        }
        //transform.localRotation = Quaternion.Euler(0,0,0);
    }

    public void Damage(float damage)
    {
        if (_health - damage <= 0)
        {

            gameObject.transform.Rotate(Vector3.left, 90);
            Destroy(gameObject, 3f);
        }
        else
            _health -= damage;
    }

    private void Update()
    {
        _lastTime += Time.deltaTime;
        if (waypoints.Length > 0 && !_navMesh.hasPath && !_playerDetected)
        {
            Patrol();
        }

        if (Vector3.Distance(transform.position, _player.transform.position) < _sightRange)
        {
            PlayerFounded();
        }
        else if (Vector3.Distance(transform.position, _player.transform.position) > _sightRange)
        {
            //_navMesh.SetDestination(_spawnPoint.position);
        }
            

    }
    private void Patrol()
    {
        if (_currentPoint < waypoints.Length)
        {
            _currentPoint++;
            _navMesh.SetDestination(waypoints[_currentPoint].transform.position);
        }
        else
        {
            _currentPoint = -1;
        }
    }


    private void PlayerFounded()
    {
        //_playerDetected = true;
        transform.LookAt(_player.transform);
        _navMesh.SetDestination(_player.transform.position);
        if (Vector3.Distance(transform.position, _player.transform.position) < _attackRange)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (_lastTime > _fireSpeed)
        {
            GameObject bullet = Instantiate(_bulletPrefab, _gunFirePossition.position, _gunFirePossition.rotation);
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * 100f;
            _lastTime = 0f;
            Destroy(bullet, 1f);
        }
    }
}
