using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private float _damage = 100f;

    Rigidbody rb;
    CapsuleCollider cc;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cc = gameObject.GetComponent<CapsuleCollider>();
        
    }

}
