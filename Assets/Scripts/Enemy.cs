using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float __damage;
    [SerializeField] private GameObject _bulletPrefab;
    private float _health;

    private void Awake()
    {
        _health = _maxHealth;
    }

    public void Damage(float damage)
    {
        if (_health - damage <= 0)
        {

            gameObject.transform.Rotate(Vector3.left,90);
            Destroy(gameObject,3f);
        }
        else
            _health -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            Damage(20);
            Debug.Log(_health);
        }
        if (other.tag == "Mine" )
        {
            Damage(101);
            Debug.Log(_health);
        }
    }
}
