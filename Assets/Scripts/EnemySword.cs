using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    public bool inContact;
    private void OnTriggerStay(Collider col){
        if (col.gameObject.name == "Player")
        {
            inContact = true;
        }
        else {
            inContact = false;
        }
    }
}
