using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{

    public GameObject wrathModel;
    public GameObject wrathAbility;
    public Transform wrathInit;
    public GameObject shredHitBox;
    
    public void ActivateWrathModel()
    {
        wrathModel.SetActive(true);
    }

    public void DeactivateWrathModel()
    {
        wrathModel.SetActive(false);
    }

    public void InitializeWrath()
    {
        var wrath = Instantiate(wrathAbility, wrathInit.position, Quaternion.identity);
        wrath.GetComponent<WrathAbility>().SetDirection(transform.forward);
    }

    public void ActivateShredHitBox()
    {
        shredHitBox.SetActive(true);
    }

    public void DeactivateShredHitBox()
    {
        shredHitBox.SetActive(false);
    }
}