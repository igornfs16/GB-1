using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    [SerializeField] private int _gunAmmo = 500;
    private Transform _gunFirePossition;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _gunFireSpeed = 0.1f;

    [SerializeField] private int _mortarAmmo = 500;
    private Transform _mortarFirePossition;
    [SerializeField] private GameObject _minePrefab;
    [SerializeField] private float _mortarFireSpeed = 0.5f;

    private float _lastTime;

    //заготовки для вращения камеры
    [SerializeField] private float headMinY = -80f;
    [SerializeField] private float headMaxY = 80f;
    [SerializeField] private float _sensitivity = 1f;
    private float rotationX;
    private float rotationY;
    private Vector3 _viewDir;

    private Vector3 _currentPos;
    [SerializeField] private float _speed = 1;

   


    void Start()
    {
        //_currentPos = transform.position;
        //_viewDir.z = 0;
        _gunFirePossition = transform.Find("GunPossition").transform;
        _mortarFirePossition = transform.Find("MortarPossition").transform;
    }

    void Update()
    {
        _lastTime += Time.deltaTime;
        _currentPos.x = -Input.GetAxis("Horizontal");
        _currentPos.z = -Input.GetAxis("Vertical");

        //знаю что поворот реализован не правильно, но это заплатка, чтобы побродить по уровню :)
        // _currentDir.x = Input.GetAxis("Mouse X") * _rotationSpeed *Time.deltaTime;
        //_currentDir.y = Input.GetAxis("Mouse Y") * _rotationSpeed *Time.deltaTime;
        //_viewDir.x = (transform.rotation.eulerAngles.x + _currentDir.y) % 360;
        //_viewDir.y = (transform.rotation.eulerAngles.y + _currentDir.x) % 360;

        rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _sensitivity;
        rotationY += Input.GetAxis("Mouse Y") * _sensitivity;
        rotationY = Mathf.Clamp(rotationY, headMinY, headMaxY);
        transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
        GunFire();
        MortarFire();
    }

    private void FixedUpdate()
    {
        var move = _currentPos * _speed * Time.fixedDeltaTime;
        transform.Translate(move);
        
        
        //transform.rotation = Quaternion.Euler(_viewDir);
    }

    void GunFire()
    {
        if (Input.GetMouseButton(0) && _lastTime > _gunFireSpeed && _gunAmmo > 0)
        {
            Instantiate(_bulletPrefab, _gunFirePossition.position, _gunFirePossition.rotation);
            _gunAmmo--;
            _lastTime = 0f;
        }
    }
    void MortarFire()
    {
        if (Input.GetMouseButton(1) && _lastTime > _mortarFireSpeed && _mortarAmmo > 0)
        {
            Instantiate(_minePrefab, _mortarFirePossition.position, _mortarFirePossition.rotation);
            _gunAmmo--;
            _lastTime = 0f;
        }
    }
}
