using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _damage = 20f;
    //[SerializeField] private float _speed = 200f;


    private Transform _target;

    public float GetDamage()
    {
        return _damage;
    }

    public void Init(Transform target)
    {
        _target = target;
    }

    //private void Update()
    //{
    //    //transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    //    //transform.Translate(_target.position * _speed * Time.deltaTime);
    //}

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Enemy")
        //{
        //    other.GetComponent<Enemy>().Damage(_damage);
        //}
        if (other.tag == "Ambish")
            return;
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().Damage(_damage);
        }
        //if (other.tag == "airdef")
        //{
        //    other.GetComponent<airdef>().Damage(_damage);
        //}
        Destroy(gameObject);
    }
}
