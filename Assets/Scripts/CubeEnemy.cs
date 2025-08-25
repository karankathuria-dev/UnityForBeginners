using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//CubeEnemy inherits all the functionallity from the base enemy class
public class CubeEnemy : Enemy
{
    // We can override the TakeDamage function to add unique behaviour

    public override void TakeDamage(float amount)
    {
        // We call the base TakeDamage function first to still substract health
        base.TakeDamage(amount);

        //Then we add some unique behaviour for this type of enemy 
        GetComponent<Renderer>().material.color = Color.red;
        Debug.Log("cube is now angry !");
    }
    // we can also override the Die function to add unique behavior
    protected override void Die()
    {
        Debug.Log("Cube Exploded!");
        // we call the base Die function to still destroy the game object
        base.Die();
    }
}
