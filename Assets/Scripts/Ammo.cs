using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private int _ammoAdd = 100;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerController player = other.GetComponentInChildren<PlayerController>();
            player.AmmoAdd(_ammoAdd);
            Destroy(gameObject);
        }
    }
}
