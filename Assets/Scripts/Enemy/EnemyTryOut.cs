using System;
using UnityEngine;

public class EnemyTryOut : MonoBehaviour
{

    private int _health = 2;

    private void TakeDamage()
    {
        _health--;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Attack"))
        {
            Debug.Log("Player hit me");
            TakeDamage();
            Debug.Log("Health: " + _health);
        }
    }
    
    void Update()
    {
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    
}
