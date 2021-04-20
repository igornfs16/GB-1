using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private float _damage = 100f;
    [SerializeField] private float _speed = 100f;


    private Transform _target;
    private float _fireForce;


    public float GetDamage()
    {
        return _damage;
    }

    public void Init(Transform target, float fireForce)
    {
        _target = target;
        _fireForce = fireForce;
    }

    private void Awake()
    {
        gameObject.GetComponent<Rigidbody>().AddForce((gameObject.transform.forward + Vector3.up) * _fireForce*100);
    }

}
