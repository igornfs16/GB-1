using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    private float _health;

    private void Awake()
    {
        _health = _maxHealth;
    }

    public void Damage(float damage)
    {
        if (_health - damage <= 0)
        {
            Destroy(gameObject);
        }
        else
            _health -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            Damage(1);
            Debug.Log(_health);
        }
        if (other.tag == "Mine" )
        {
            Damage(100);
            Debug.Log(_health);
        }
    }
}
