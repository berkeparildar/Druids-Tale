using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    public bool inContact;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    private void OnTriggerStay(Collider col){
        Debug.Log("In collision");
        if (col.gameObject.name == "Player")
        {
            inContact = true;
        }
        else {
            inContact = false;
        }
    }
}
