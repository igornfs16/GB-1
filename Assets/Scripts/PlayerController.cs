using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float _maxPlayerHealth = 1000;
    [SerializeField] private int _gunAmmo = 500;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _gunFireSpeed = 0.1f;
    [SerializeField] private int _mortarAmmo = 100;
    [SerializeField] private GameObject _minePrefab;
    [SerializeField] private float _mortarFireSpeed = 0.5f;
    [SerializeField] private float headMinY = -80f;
    [SerializeField] private float headMaxY = 80f;
    [SerializeField] private float _sensitivity = 1f;
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _bulletSpeed = 300f;
    [SerializeField] private float _mineSpeed = 100f;
    [SerializeField] private float _maxMortarRange = 100f;
    [SerializeField] private bool _haveMortar = false;

    private float rotationX;
    private float rotationY;
    private float _playerHealth;
    private Transform _gunFirePossition;
    private Transform _mortarFirePossition;
    private float _mortarFireDistance;
    private Vector3 _currentPos;
    private float _lastTime;
    

   


    void Awake()
    {

        _gunFirePossition = transform.Find("GunPossition").transform;
        _mortarFirePossition = transform.Find("MortarPossition").transform;
        _playerHealth = _maxPlayerHealth;
        _mortarFireDistance = 0f;
    }

    void Update()
    {
        _lastTime += Time.deltaTime;
        _currentPos.x = -Input.GetAxis("Horizontal");
        _currentPos.z = -Input.GetAxis("Vertical");


        rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _sensitivity;
        rotationY += Input.GetAxis("Mouse Y") * _sensitivity;
        rotationY = Mathf.Clamp(rotationY, headMinY, headMaxY);
        transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
        
    }

    private void FixedUpdate()
    {
        var move = _currentPos * _speed * Time.fixedDeltaTime;
        transform.Translate(move);
        GunFire();
        MortarFire();
    }

    void GunFire()
    {
        if (Input.GetMouseButton(0) && _lastTime > _gunFireSpeed && _gunAmmo > 0)
        {
            Quaternion q = new Quaternion(_gunFirePossition.rotation.x+90, _gunFirePossition.rotation.y, _gunFirePossition.rotation.z,0);
            GameObject bullet = Instantiate(_bulletPrefab, _gunFirePossition.position, _gunFirePossition.rotation);
            bullet.GetComponent<Rigidbody>().velocity = -gameObject.transform.forward * _bulletSpeed;
            _gunAmmo--;
            _lastTime = 0f;
            Destroy(bullet, 1f);
        }
    }
    void MortarFire()
    {
        if(Input.GetMouseButtonDown(1) && _lastTime > _mortarFireSpeed && _mortarAmmo > 0 &&_haveMortar)
        {
            if (_mortarFireDistance < _maxMortarRange)
                _mortarFireDistance += Time.deltaTime *10f;
            else
                _mortarFireDistance = _maxMortarRange;
        }
        if (Input.GetMouseButtonUp(1) && _lastTime > _mortarFireSpeed && _mortarAmmo > 0 && _haveMortar)
        {
            GameObject mine = Instantiate(_minePrefab, _mortarFirePossition.position, _mortarFirePossition.rotation);
            mine.transform.Rotate(Vector3.left,45);
            mine.GetComponent<Rigidbody>().velocity = -gameObject.transform.forward * _mortarFireDistance * _mineSpeed;
            _mortarAmmo--;
            _lastTime = 0f;
            Destroy(mine, 1f);
            _mortarFireDistance = 0f;
        }
    }

    public void AmmoAdd(int ammo)
    {
        _gunAmmo += ammo;
    }
    public void MineAmmoAdd(int ammo)
    {
        _mortarAmmo += ammo;
    }
    public void HealthAdd(float health)
    {
        if (_playerHealth + health > _maxPlayerHealth)
            _playerHealth = _maxPlayerHealth;
        else
            _playerHealth += health;
    }
    public void Damage(float damage)
    {
        if(_playerHealth - damage <= 0)
        {
            Debug.Log("You are Killed");
        }
        else
        {
            _playerHealth -= damage;
        }
    }

    public void GiveMortar()
    {
        _haveMortar = true;
    }

}
