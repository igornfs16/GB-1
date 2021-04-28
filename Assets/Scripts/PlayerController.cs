using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header ("player fields")]
    [SerializeField] private float _maxPlayerHealth = 1000;
    [SerializeField] private float _speed = 1;
    [SerializeField] private float headMinY = -80f;
    [SerializeField] private float headMaxY = 80f;
    [Header("weapon fields")]
    [SerializeField] private int _gunAmmo = 500;
    [SerializeField] private float _gunDamage = 34;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _gunFireRate = 0.1f;
    [SerializeField] private float _mortarFireRate = 0.5f;
    [SerializeField] private int _mortarAmmo = 100;
    [SerializeField] private GameObject _minePrefab;
    //[SerializeField] private float _bulletSpeed = 200f;
    [SerializeField] private float _mineSpeed = 0.3f;
    [SerializeField] private float _maxMortarForce = 50f;
    [SerializeField] private ParticleSystem _fireSmokePrefab;
    [SerializeField] private LineRenderer _fireLine;
    [Header("UI fields")]
    [SerializeField] private Slider _UIFireForce;
    [SerializeField] private Text _UIPlayerHealth;
    [SerializeField] private Text _UIPlayerAmmo;
    [SerializeField] private Text _UIPlayerMinesAmmo;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private Material _fireMaterial;
    [Header("Audio fields")]
    [SerializeField] private AudioSource _robotGo;
    [SerializeField] private AudioSource _robotFire;
    [Header("Other fields")]
    [SerializeField] private bool _haveMortar = false;
    [SerializeField] private Camera _camera;
    

    private GameController gc;
    private float _rotationX;
    private float _rotationY;
    private float _playerHealth;
    private Transform _gunFirePossition;
    private Vector3 _direction;
    private Transform _mortarFirePossition;
    private float _mortarFireForce;
    private Vector3 _currentPos;
    private float _lastTime;
    private Animator _animator;
    


    void Awake()
    {

        _gunFirePossition = GetComponentInChildren<Camera>().transform.Find("GunPossition").transform;
        _mortarFirePossition = GetComponentInChildren<Camera>().transform.Find("MortarPossition").transform;
        _playerHealth = _maxPlayerHealth;
        _mortarFireForce = 10f;
        _animator = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _UIFireForce.gameObject.SetActive(false);
    }

    void Update()
    {
        _lastTime += Time.deltaTime;

        if (!GameController._menuOpened)
        {
            _rotationX = Input.GetAxis("Mouse X");
            _rotationY -= Input.GetAxis("Mouse Y");
        }
        _rotationY = Mathf.Clamp(_rotationY, headMinY, headMaxY);
        transform.Rotate(Vector3.up * _rotationX);
        _camera.transform.localRotation = Quaternion.Euler(-_rotationY + 180, 0, 180);

        _direction = transform.forward;

        if (Input.GetMouseButton(0) && _lastTime > _gunFireRate && _gunAmmo > 0 && !GameController._menuOpened)
        {
            GunFire();
        }
        if (Input.GetMouseButtonUp(0) && !GameController._menuOpened)
        {
            _animator.SetBool("PlayerFire", false);
            _robotFire.Stop();
        }

        if (Input.GetMouseButton(1) && _lastTime > _mortarFireRate && _mortarAmmo > 0 && _haveMortar && !GameController._menuOpened)
        {
            MortarFireStart();
        }
        if (Input.GetMouseButtonUp(1) && !GameController._menuOpened)
        {
            MortarFire();
        }
        _UIPlayerHealth.text = "Health: " + _playerHealth.ToString();
        _UIPlayerAmmo.text = "Ammo: " + _gunAmmo.ToString();
        _UIPlayerMinesAmmo.enabled = false;
    }

    private void FixedUpdate()
    {
        if(!GameController._menuOpened)
        {
            _currentPos.x = -Input.GetAxis("Horizontal");
            _currentPos.z = -Input.GetAxis("Vertical");
        }
        
        Vector3 move = _currentPos * _speed * Time.fixedDeltaTime;

        if (move != Vector3.zero)
            _robotGo.Play();
        else
            _robotGo.Stop();

        transform.Translate(move);

        //Debug.DrawLine(transform.position + new Vector3(0, 1, 1), Vector3.forward * Mathf.Infinity, Color.black);
    }
    void GunFire()
    {
        //Quaternion q = new Quaternion(_gunFirePossition.rotation.x+90, _gunFirePossition.rotation.y, _gunFirePossition.rotation.z, 0);
        //Bullet bullet = Instantiate(_bulletPrefab, _gunFirePossition.position, q).GetComponent<Bullet>();
        //bullet.Init(_gunFirePossition.transform);
        //bullet.GetComponent<Rigidbody>().velocity = _camera.transform.forward * _bulletSpeed;
        //_gunAmmo--;
        //_lastTime = 0f;
        //Destroy(bullet, 3f);
        _animator.SetBool("PlayerFire",true);
        _robotFire.Play();
        ParticleSystem _smoke = Instantiate(_fireSmokePrefab, _gunFirePossition);
        LineRenderer _line = Instantiate(_fireLine, _gunFirePossition);
        RaycastHit _raycast;
        bool rayCast = Physics.Raycast(_gunFirePossition.position, _camera.transform.forward, out _raycast, Mathf.Infinity, _mask);

        //DrawLine(_gunFirePossition.position, _camera.transform.forward);

        if (rayCast)
        {
            if (_raycast.collider.gameObject.CompareTag("Enemy"))
            {
                _raycast.collider.GetComponent<Enemy>().Damage(_gunDamage);
                //Debug.Log("Ouch");
            }
            if (_raycast.collider.gameObject.CompareTag("airdef"))
            {
                _raycast.collider.GetComponent<airdef>().Damage(_gunDamage);
                //Debug.Log("OuchTower");
            }
        }
        Destroy(_smoke, 2.0f);
        Destroy(_line, 0.1f);
        _gunAmmo--;
        _lastTime = 0f;

    }
    void DrawLine(Vector3 start, Vector3 end, float duration = 0.1f)
    {
        GameObject myLine = new GameObject("Fire");
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = _fireMaterial;
        lr.startColor = Color.red;
        lr.endColor = Color.yellow;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        //Debug.Log(start);
        //Debug.Log(end);
        GameObject.Destroy(myLine, duration);
    }
    void MortarFireStart()
    {
        _UIFireForce.gameObject.SetActive(true);
        //Debug.Log("MortarFireForceDOWN");
        if (_mortarFireForce < _maxMortarForce)
        {
            _mortarFireForce += Time.deltaTime * 30;
            _UIFireForce.value = _mortarFireForce;
            //Debug.Log("Mortar Power: " + _mortarFireForce.ToString());
        }
        else
            _mortarFireForce = _maxMortarForce;
    }

    void MortarFire()
    {
        GameObject mine = Instantiate(_minePrefab, _mortarFirePossition.position, _mortarFirePossition.rotation);
        //Debug.Log("MortarFireForceUP");
        mine.GetComponent<Rigidbody>().velocity = (_camera.transform.forward + Vector3.up * (Vector3.Magnitude(_camera.transform.forward))) * _mortarFireForce*_mineSpeed;
        //mine.GetComponent<Mine>().Init(_mortarFirePossition,_mortarFireForce);
        
        _mortarAmmo--;
        _lastTime = 0f;
        //Destroy(mine, 4f);
        _mortarFireForce = 10f;
        _UIFireForce.value += _mortarFireForce;
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
            //Debug.Log("You are Killed");
            GameController._youLoose = true;
        }
        else
        {
            _playerHealth -= damage;
        }
    }

    public void GiveMortar()
    {
        _haveMortar = true;
        _UIPlayerMinesAmmo.enabled = true;
        _UIPlayerMinesAmmo.text = "Mines: "+ _mortarAmmo.ToString();
    }

}
