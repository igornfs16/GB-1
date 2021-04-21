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
    [SerializeField] private bool _isTurret = false;
    [SerializeField] private LayerMask _mask;

    private Transform _gunFirePossition;
    private bool _playerDetected = false;
    private int _currentPoint;
    private float _health;
    private NavMeshAgent _navMesh;
    private Vector3 _spawnPoint;
    private GameObject _player;
    private float _lastTime;
    private void Awake()
    {
        _gunFirePossition = transform.Find("GunPossition").transform;
        _health = _maxHealth;
        if (!_isTurret)
        {
            _navMesh = GetComponent<NavMeshAgent>();
        }
        //_currentPoint = 0;
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerDetected = false;
    }
    private void Start()
    {
        _spawnPoint = transform.position;
        //Debug.Log("My spawn point: " + _spawnPoint.ToString());
        //if (!_isTurret)
        //{
        //    if (waypoints.Length - 1 > 0)
        //    {
        //        _navMesh.SetDestination(waypoints[_currentPoint].transform.position);
        //    }
        //    //transform.localRotation = Quaternion.Euler(0,0,0);
        //}
    }
    public void Damage(float damage)
    {
        if (_health - damage <= 0)
        {
            Death();
        }
        else
            _health -= damage;
    }
    private void Update()
    {
        _lastTime += Time.deltaTime;
        //if (!_isTurret)
        //{
        //    if (waypoints.Length > 0 && !_navMesh.hasPath && !_playerDetected)
        //    {
        //        Patrol();
        //    }
        //}
        if ((Vector3.Distance(transform.position, _player.transform.position) < _sightRange) && _playerDetected)
        {
            PlayerFounded();
        }
        if ((Vector3.Distance(transform.position, _player.transform.position) > _sightRange) && _playerDetected)
        {
            if (!_isTurret)
            {
                //Debug.Log("I return to: " + _spawnPoint.ToString());
                Invoke("GoBack",3);
            }     
        }
    }
    private void FixedUpdate()
    {
        RaycastHit _raycast;
        bool rayCast = Physics.Raycast(transform.position, _player.transform.position - transform.position, out _raycast, _sightRange,_mask);

        if (rayCast)
        {
            if (_raycast.collider.gameObject.CompareTag("Player"))
            {
                _playerDetected = true;
            }
        }

        //Debug.DrawRay(_gunFirePossition.position, _player.transform.position - transform.position, Color.green);
    }
    private void GoBack()
    {
        _navMesh.ResetPath();
        _navMesh.SetDestination(_spawnPoint);
        _playerDetected = false;
        //Debug.Log("Go back!");
    }
    //private void Patrol()
    //{
    //    if (_currentPoint < waypoints.Length)
    //    {
    //        _currentPoint++;
    //        _navMesh.SetDestination(waypoints[_currentPoint].transform.position);
    //    }
    //    else
    //    {
    //        _currentPoint = -1;
    //    }
    //}
    private void PlayerFounded()
    {
        transform.LookAt(_player.transform);
        //Debug.Log("Player founded!");
        if (!_isTurret)
        {
            _navMesh.SetDestination(_player.transform.position);
        }
        if (Vector3.Distance(transform.position, _player.transform.position) < _attackRange)
        {
            Fire();
        }
    }
    private void Fire()
    {
        if (_lastTime > _fireSpeed)
        {
            Bullet bullet = Instantiate(_bulletPrefab, _gunFirePossition.position, _gunFirePossition.rotation).GetComponent<Bullet>();
            bullet.Init(_gunFirePossition.transform);
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * 150f;
            _lastTime = 0f;
            Destroy(bullet, 1f);
        }
    }
    private void Death()
    {
        gameObject.transform.Rotate(Vector3.left, 90);
        Destroy(gameObject, 0f);
    }
}