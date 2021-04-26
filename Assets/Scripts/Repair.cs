using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : MonoBehaviour
{
    [SerializeField] private int _healthAdd = 100;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerController player = other.GetComponentInChildren<PlayerController>();
            player.HealthAdd(_healthAdd);
            Destroy(gameObject);
        }
    }
}
