using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private float _damage = 100f;
    [SerializeField] private float _speed = 100f;
    [SerializeField] private float _damageRadius = 20f;


    //private Transform _target;
    //private float _fireForce;


    public float GetDamage()
    {
        return _damage;
    }

    //public void Init(Transform target, float fireForce)
    //{
    //    _target = target;
    //    _fireForce = fireForce;
    //}

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("BOOOOOOM");
        Boom();
    }
    private void Boom()
    {

        Collider[] hits = Physics.OverlapSphere(transform.position, _damageRadius);
        
        foreach (Collider hit in hits)
        {
            Rigidbody rb;
           
            if (hit.CompareTag("Enemy") )
            {
                hit.GetComponent<Enemy>().Damage(_damage);
            }
            if(hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerController>().Damage(_damage);
            }
            if (hit.TryGetComponent<Rigidbody>(out rb))
            {
                float boomForce = Vector3.Magnitude(gameObject.transform.position - rb.position);
                if (boomForce == 0)
                {
                    boomForce = 1;
                }
                else
                    boomForce = 1 / boomForce;
                rb.AddExplosionForce(4000f*boomForce, gameObject.transform.position, _damageRadius*10, 30f,ForceMode.Impulse);
                //Debug.Log(rb.tag);
                //Debug.Log(boomForce);
            }
        }
        Destroy(gameObject);
    }
}
