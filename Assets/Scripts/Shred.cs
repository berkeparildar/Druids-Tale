using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shred : MonoBehaviour
{
    private bool _hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnEnable()
    {
        _hit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !_hit)
        {
            other.GetComponent<Enemy>().TakeDamage(10);
            _hit = true;
        }
    }
}
