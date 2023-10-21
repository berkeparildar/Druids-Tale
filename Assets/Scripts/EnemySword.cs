using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    public bool inContact;
    private void OnTriggerStay(Collider col){
        if (col.gameObject.name == "Player")
        {
            inContact = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            inContact = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            inContact = false;
        }
    }
}
