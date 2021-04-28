using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private float _damage = 100f;
    //[SerializeField] private float _speed = 100f;
    [SerializeField] private float _damageRadius = 20f;
    [SerializeField] private GameObject _explodePrefab;
    [SerializeField] private AudioSource _boomSound;
    private AudioSource _mineSound;


    //private Transform _target;
    //private float _fireForce;

    private void Start()
    {
        _mineSound = gameObject.GetComponent<AudioSource>();
    }

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
        if (other.tag == "Ambish")
            return;
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
                //gameObject.GetComponent<Exploder>().enabled = true;
            }
            if(hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerController>().Damage(_damage);
                //gameObject.GetComponent<Exploder>().enabled = true;
            }
            if (hit.tag == "airdef")
            {
                hit.GetComponent<airdef>().Damage(_damage);
                //gameObject.GetComponent<Exploder>().enabled = true;
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
            //if(hit.CompareTag("Environment"))
            //{
            //    //gameObject.GetComponent<Exploder>().enabled = true;
            //}
        }
        GameObject _explode = Instantiate(_explodePrefab,transform.position,transform.rotation);
        _mineSound.Stop();
        _boomSound.Play();
        Destroy(gameObject,3f);
        Destroy(_explode, 2f);
    }
}
