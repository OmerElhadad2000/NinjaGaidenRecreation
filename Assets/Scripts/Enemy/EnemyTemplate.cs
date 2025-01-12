using System;
using Mono_Pool;
using UnityEngine;

public class EnemyTemplate : MonoBehaviour, IPoolableObject
{
    private Animator _enemyAnimator;
    
    private void OnEnable()
    {
        _enemyAnimator = GetComponent<Animator>();
        
    }
    
    public void Reset()
    {
        if (_enemyAnimator == null) return;
        _enemyAnimator.runtimeAnimatorController = null;
    }
    
    public void SetEnemyAnimationController(RuntimeAnimatorController controller)
    {
        _enemyAnimator.runtimeAnimatorController = controller;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player Attack"))
        {
            //play death animation and send back to pool;
        }
    }
}
