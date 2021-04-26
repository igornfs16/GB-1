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
    //[SerializeField] private bool _isTurret = false;
    [SerializeField] private LayerMask _mask;

    
    private Transform _gunFirePossition;
    private bool _playerInForwardLook = false;
    private int _currentPoint;
    private float _health;
    private NavMeshAgent _navMesh;
    private Vector3 _spawnPoint;
    private GameObject _player;
    private float _lastTime;
    private Animator _animator;

    private enum _enemyTypes
    {
        Infantry,
        Tank,
        Turret
    }
    private _enemyTypes _enemyType;
    private void Awake()
    {
        _gunFirePossition = transform.Find("GunPossition").transform;
        _health = _maxHealth;
        
        if(gameObject.name.Contains("EnemyInfantry"))
        {
            _enemyType = _enemyTypes.Infantry;
        }
        if (gameObject.name.Contains("Turret"))
        {
            _enemyType = _enemyTypes.Turret;
        }
        if (gameObject.name.Contains("Tank"))
        {
            _enemyType = _enemyTypes.Tank;
        }
        if (_enemyType!=_enemyTypes.Turret)
        {
            _navMesh = GetComponent<NavMeshAgent>();
        }
        //_currentPoint = 0;
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerInForwardLook = false;
        if(_enemyType == _enemyTypes.Infantry)
        {
            _animator = gameObject.GetComponent<Animator>();
            _animator.SetBool("InfantryGo", false);
        }
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
        if ((Vector3.Distance(transform.position, _player.transform.position) < _sightRange) && _playerInForwardLook)
        {
            PlayerFounded();
        }
        if ((Vector3.Distance(transform.position, _player.transform.position) > _sightRange) && _playerInForwardLook)
        {
            if (_enemyType != _enemyTypes.Turret)
            {
                //Debug.Log("I return to: " + _spawnPoint.ToString());
                Invoke("GoBack",3);
            }     
        }
        Debug.Log(_playerInForwardLook.ToString());
    }
    private void FixedUpdate()
    {
        RaycastHit _raycast;
        bool rayCast = Physics.Raycast(transform.position, _player.transform.position - transform.position, out _raycast, _sightRange,_mask);

        if (rayCast)
            if (_raycast.collider.gameObject.CompareTag("Player"))
                _playerInForwardLook = true;


        if(_enemyType == _enemyTypes.Infantry && gameObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            _animator.SetBool("InfantryGo", true);
        }  
        //else
        //{
        //    _animator.SetBool("InfantryGo", false);
        //}
            

        Debug.DrawRay(transform.position, _player.transform.position - transform.position, Color.green);
    }
    private void GoBack()
    {
        _navMesh.ResetPath();
        _navMesh.SetDestination(_spawnPoint);
        _playerInForwardLook = false;
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
        if (_enemyType != _enemyTypes.Turret)
        {
            Debug.Log("I am " + _enemyType.ToString() + " and I see you");
            _navMesh.SetDestination(_player.transform.position);
        }
        if (Vector3.Distance(transform.position, _player.transform.position) < _attackRange)
        {
            if (_enemyType == _enemyTypes.Infantry)
            {
                _animator.SetBool("InfantryFire", true);
            }
            Fire();
        }
    }
    private void Fire()
    {
        Debug.Log("I am " + _enemyType.ToString() + " and I'll kill you");
        if (_lastTime > _fireSpeed)
        {
            Bullet bullet = Instantiate(_bulletPrefab, _gunFirePossition.position, _gunFirePossition.rotation).GetComponent<Bullet>();
            bullet.Init(_gunFirePossition.transform);
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * 150f;
            _lastTime = 0f;
            Destroy(bullet, 1f);
        }
        if(_enemyType == _enemyTypes.Infantry)
        {
            _animator.SetBool("InfantryFire", false);
        }

    }
    private void Death()
    {
        if (_enemyType == _enemyTypes.Infantry)
        {
            _animator.SetBool("InfantryDie", true);
        }
        Destroy(gameObject, 2f);
    }
}