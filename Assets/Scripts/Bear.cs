using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Player
{
    //TODO: Coming in 2025 - Soon™ 
    void Awake()
    {
        CurrentForm = "bear";

    }
    void Update()
    {

    }

    protected override void Movement()
    {
        base.Movement();
    }

    protected override void Morph()
    {
        base.Morph();
    }

    protected override void CastSpecialAbilityOne()
    {
        base.CastSpecialAbilityOne();
    }

    protected override void CastSpecialAbilityTwo()
    {
        base.CastSpecialAbilityTwo();
    }

    
}
