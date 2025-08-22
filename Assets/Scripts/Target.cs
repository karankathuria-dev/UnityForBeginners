using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float health = 50f;
    // Start is called before the first frame update
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health<=0f)
        {
            Die();
        }
    }
    private void Die()
    {
        GameManager.Instance.AddScore(10);
        Destroy(gameObject);
    }
}
