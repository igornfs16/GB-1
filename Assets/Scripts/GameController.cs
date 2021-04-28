using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _enemyInfantryPrefab;
    private GameObject[] _enemyInfantrySpawnPoints;

    [SerializeField] private GameObject _enemyTurretPrefab;
    private GameObject[] _enemyTurretSpawnPoints;

    [SerializeField] private GameObject _enemyTankPrefab;
    private GameObject[] _enemyTankSpawnPoints;

    [SerializeField] private GameObject _ammoPrefab;
    private GameObject[] _ammoSpawnPoints;

    [SerializeField] private GameObject _repairPrefab;
    private GameObject[] _repairSpawnPoints;

    private GameObject[] _liveAirDef;
    private static int _airDefNum;

    private GameObject _gate;
    [SerializeField] GameObject _boomPrefab;
    [SerializeField] AudioClip _boomSound;


    [SerializeField] private Text _winLooseText;
    [SerializeField] private Canvas _menu;
    [SerializeField] private Canvas _options;

    [SerializeField] private Slider _suroundVolumeSlider;
    [SerializeField] private AudioMixer _mixer;

    public static bool _menuOpened;
    public static bool _youLoose;
    void Awake()
    {
        _enemyInfantrySpawnPoints = GameObject.FindGameObjectsWithTag("InfantrySpawnPoint");
        _enemyTurretSpawnPoints = GameObject.FindGameObjectsWithTag("TurretSpawnPoint");
        _enemyTankSpawnPoints = GameObject.FindGameObjectsWithTag("TankSpawnPoint");
        _ammoSpawnPoints = GameObject.FindGameObjectsWithTag("AmmoSpawnPoint");
        _repairSpawnPoints = GameObject.FindGameObjectsWithTag("RepairSpawnPoint");
        _liveAirDef = GameObject.FindGameObjectsWithTag("airdef");
        _youLoose = false;
        _gate = GameObject.Find("Gate3");
    }
    void Start()
    {
        Spawn(_enemyInfantryPrefab, _enemyInfantrySpawnPoints);
        Spawn(_enemyTurretPrefab, _enemyTurretSpawnPoints);
        Spawn(_enemyTankPrefab, _enemyTankSpawnPoints);
        Spawn(_ammoPrefab, _ammoSpawnPoints);
        Spawn(_repairPrefab, _repairSpawnPoints);

        _airDefNum = _liveAirDef.Length;

        _winLooseText.enabled = false;
        _menu.enabled = false;
        _menuOpened = false;
        _suroundVolumeSlider.value = GameObject.Find("SuroundSounds").GetComponent<AudioSource>().volume;
        _mixer.SetFloat("Master", Mathf.Log10(0.5f) * 20);
        
    }
    void Spawn(GameObject prefab, GameObject[] spawnPoints)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject go;
            go = Instantiate(prefab, spawnPoints[i].transform.position, Quaternion.identity,spawnPoints[i].transform);
        }
    }
    private void Update()
    {
        if(_airDefNum == 0)
        {
            Win();
        }
        if(_youLoose)
        {
            Loose();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !_menuOpened)
        {
            OpenMenu(true);
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && _menuOpened)
        {
            OpenMenu(false);
        }
    }
    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
        OpenMenu(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Options()
    {
        _menu.enabled = false;
        _options.enabled = true;
    }
    public void Back()
    {
        _menu.enabled = true;
        _options.enabled = false;
    }
    public void SuroundVolume()
    {
        GameObject.Find("SuroundSounds").GetComponent<AudioSource>().volume = _suroundVolumeSlider.value;
    }
    public void SoundVolume(float SliderValue)
    {
        _mixer.SetFloat("Master", Mathf.Log10(SliderValue) * 20);
    }
    public void OpenMenu(bool value)
    {
        _menu.enabled = value;
        Cursor.lockState = CursorLockMode.None;
        _menuOpened = value;
        if(value)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
    }

    public static void AirDefKilled()
    {
        _airDefNum--;
    }

    private void Loose()
    {
        _winLooseText.enabled = true;
        _winLooseText.text = "GAMEOVER";
    }

    void Win()
    {
        _winLooseText.enabled = true;
        for (int i = 0; i < 40; i++)
        {
            float _boomRadius = 30f;
            float randX = Random.Range(-_boomRadius, _boomRadius);
            float randY = Random.Range(-_boomRadius, _boomRadius);
            float randZ = Random.Range(-_boomRadius, _boomRadius);
            Vector3 pos = new Vector3(randX, randY, randZ);
            Instantiate(_boomPrefab, _gate.transform.position + pos, _gate.transform.rotation);
            AudioSource.PlayClipAtPoint(_boomSound, _gate.transform.position + pos);
        }
        _gate.transform.Translate(0f,-30f,0f);
    }
}
