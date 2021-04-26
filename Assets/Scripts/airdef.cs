using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airdef : MonoBehaviour
{
    [SerializeField] float _maxHealth;
    private float _health;

    private void Awake()
    {
        _health = _maxHealth;
    }

    public void Damage(float damage)
    {
        if (_health - damage <= 0)
        {
            Death();
        }
        else
        {
            _health -= damage;
            Debug.Log(_health);
        }
            
    }

    private void Death()
    {
        GameController.AirDefKilled();
        Destroy(gameObject);
    }
}
