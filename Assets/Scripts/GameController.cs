using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField] private Text _winLooseText;

    [SerializeField] private Canvas _menu;

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
        GameObject.Find("Gate3").transform.Translate(0f,-30f,0f);
    }
}
