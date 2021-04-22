using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] GameObject _gate;
    [SerializeField] private float _gateOpenSpeed = 0.005f;

    private bool _ontrigger = false;
    private bool _openGate = false;
    private Vector3 openPossition;
    private void Awake()
    {
        openPossition = new Vector3(_gate.transform.position.x, _gate.transform.position.y - 30, _gate.transform.position.z);
    }

    private void Update()
    {
        if (_openGate)
        {
            _gate.transform.position = Vector3.Lerp(_gate.transform.position, openPossition, _gateOpenSpeed);
            
        }
        if(Input.GetKeyDown(KeyCode.E) && _ontrigger)
        {
            _openGate = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            _ontrigger = true;
            Debug.Log("Gate Opened");
        }
            
    }
    private void OnTriggerExit(Collider other)
    {
        _openGate = false;
        _ontrigger = false;
    }
}
