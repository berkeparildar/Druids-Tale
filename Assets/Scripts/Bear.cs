using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Player
{
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

    protected override void SpecialAbilityOne()
    {
        base.SpecialAbilityOne();
    }

    protected override void SpecialAbilityTwo()
    {
        base.SpecialAbilityTwo();
    }

    
}
