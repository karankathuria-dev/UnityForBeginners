using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This is the base class that other enemy types will inherit from
public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health = 100f;
    
    //This is the virtual function which means child classes can override it.
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Enemy Took "+amount + "damage . Health : "+health);
        if (health<=0f)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        Debug.Log("Enemy has been defetead");
        Destroy(gameObject);
    }
}
