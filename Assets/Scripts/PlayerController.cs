using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float _maxPlayerHealth = 1000;
    [SerializeField] private int _gunAmmo = 500;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _gunFireRate = 0.1f;
    [SerializeField] private float _mortarFireRate = 0.5f;
    [SerializeField] private int _mortarAmmo = 100;
    [SerializeField] private GameObject _minePrefab;
    [SerializeField] private float headMinY = -80f;
    [SerializeField] private float headMaxY = 80f;
    //[SerializeField] private float _sensitivity = 10f;
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _bulletSpeed = 200f;
    [SerializeField] private float _mineSpeed = 0.3f;
    [SerializeField] private float _maxMortarForce = 50f;
    [SerializeField] private bool _haveMortar = false;
    [SerializeField] private Camera _camera;
    [SerializeField] private Slider _UIFireForce;
    [SerializeField] private Text _UIPlayerHealth;

    private float _rotationX;
    private float _rotationY;
    private float _playerHealth;
    private Transform _gunFirePossition;
    private Vector3 _direction;
    private Transform _mortarFirePossition;
    private float _mortarFireForce;
    private Vector3 _currentPos;
    private float _lastTime;


    void Awake()
    {

        _gunFirePossition = GetComponentInChildren<Camera>().transform.Find("GunPossition").transform;
        _mortarFirePossition = GetComponentInChildren<Camera>().transform.Find("MortarPossition").transform;
        _playerHealth = _maxPlayerHealth;
        _mortarFireForce = 0f;
        
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _UIFireForce.gameObject.SetActive(false);
    }

    void Update()
    {
        _lastTime += Time.deltaTime;

        _rotationX = Input.GetAxis("Mouse X");
        _rotationY -= Input.GetAxis("Mouse Y");
        _rotationY = Mathf.Clamp(_rotationY, headMinY, headMaxY);
        transform.Rotate(Vector3.up * _rotationX);
        _camera.transform.localRotation = Quaternion.Euler(-_rotationY + 180, 0, 180);

        _direction = transform.forward;
        

        if (Input.GetMouseButton(0) && _lastTime > _gunFireRate && _gunAmmo > 0)
        {
            GunFire();
        }
        if (Input.GetMouseButton(1) && _lastTime > _mortarFireRate && _mortarAmmo > 0 && _haveMortar)
        {
            MortarFireStart();
        }
        if (Input.GetMouseButtonUp(1))
        {
            MortarFire();
        }
        _UIPlayerHealth.text = "Health: " + _playerHealth.ToString();
    }

    private void FixedUpdate()
    {
        _currentPos.x = -Input.GetAxis("Horizontal");
        _currentPos.z = -Input.GetAxis("Vertical");
        var move = _currentPos * _speed * Time.fixedDeltaTime;
        transform.Translate(move);
        
        Debug.DrawLine(transform.position + new Vector3(0, 1, 1), Vector3.forward * Mathf.Infinity, Color.black);
    }

    void GunFire()
    {
            Quaternion q = new Quaternion(_gunFirePossition.rotation.x+90, _gunFirePossition.rotation.y, _gunFirePossition.rotation.z, 0);
            Bullet bullet = Instantiate(_bulletPrefab, _gunFirePossition.position, q).GetComponent<Bullet>();
            bullet.Init(_gunFirePossition.transform);
            bullet.GetComponent<Rigidbody>().velocity = _camera.transform.forward * _bulletSpeed;
            _gunAmmo--;
            _lastTime = 0f;
            Destroy(bullet, 3f);
    }
    void MortarFireStart()
    {
        _UIFireForce.gameObject.SetActive(true);
        Debug.Log("MortarFireForceDOWN");
        if (_mortarFireForce < _maxMortarForce)
        {
            _mortarFireForce += Time.deltaTime * 30;
            //_UIFireForce.value += Time.deltaTime * 30;
            Debug.Log("Mortar Power: " + _mortarFireForce.ToString());
        }
        else
            _mortarFireForce = _maxMortarForce;
    }

    void MortarFire()
    {
        GameObject mine = Instantiate(_minePrefab, _mortarFirePossition.position, _mortarFirePossition.rotation);
        Debug.Log("MortarFireForceUP");
        mine.GetComponent<Rigidbody>().velocity = (_camera.transform.forward + Vector3.up * (Vector3.Magnitude(_camera.transform.forward))) * _mortarFireForce*_mineSpeed;
        //mine.GetComponent<Mine>().Init(_mortarFirePossition,_mortarFireForce);
        
        _mortarAmmo--;
        _lastTime = 0f;
        //Destroy(mine, 4f);
        _mortarFireForce = 0f;
        _UIFireForce.gameObject.SetActive(false);
        
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
