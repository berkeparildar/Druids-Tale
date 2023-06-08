using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimationEvents : MonoBehaviour
{
    private Human _human;
    private Cat _cat;
    public GameObject wrathModel;

    private void Start()
    {
        _human = transform.GetComponentInParent<Human>();
        _cat = transform.GetComponentInParent<Cat>();
    }

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
        _human.WrathAbility();
    }

    public void ShredActivated()
    {
        _cat.hitShred = true;
    }
    
    public void ShredDeactivated()
    {
        _cat.hitShred = false;
    }

    public void FerociousBiteActivated()
    {
        _cat.hitBite = true;
    }
    
    public void FerociousBiteDeactivated()
    {
        _cat.hitBite = false;
    }
}