using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    //[SerializeField] private float _turretHealth = 100f;
    //[SerializeField] private float _turretDamage = 2f;
    [SerializeField] private float _turretRotationSpeed = 30f;
    [SerializeField] private GameObject _bulletPrefab;
    private Transform _gunFirePossition;
    [SerializeField] private float _turretFireSpeed = 10f;

    private float _lastTime;

    private bool _playerNear = false;
    private GameObject _player;

    void Awake()
    {
        _gunFirePossition = transform.Find("GunPossition").transform;
        _player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        _lastTime += Time.deltaTime;
        if (_playerNear)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward,_player.transform.position - transform.position, _turretRotationSpeed*Time.deltaTime,0));
            GunFire();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _playerNear = true;
            Debug.Log("Player Find!");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _playerNear = false;
            Debug.Log("Player Go Out!");
        }
    }

    void GunFire()
    {
        if (_lastTime > _turretFireSpeed)
        {
            Instantiate(_bulletPrefab, _gunFirePossition.position, _gunFirePossition.rotation);
            _lastTime = 0f;
            Debug.Log("TurretFire");
        }
    }
}
