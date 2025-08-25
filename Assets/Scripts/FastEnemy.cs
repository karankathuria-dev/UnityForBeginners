using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemy : Enemy
{
    public override void TakeDamage(float amount)
    {
        Debug.Log("Fast enemy is dodging !");
        //fast enemy will take only half the damage
        base.TakeDamage(amount/2);

        //you can add unique visuals here ...
    }
    protected override void Die()
    {
        Debug.Log("Fast enemy was deeated");
        base.Die();
    }


}
