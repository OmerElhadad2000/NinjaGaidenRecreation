using System;
using Mono_Pool;
using UnityEngine;

public class BasicProjectileMovement : MonoBehaviour, IPoolableObject
{
    [Header("Projectile Settings")]
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected float projectileLifeTime;
    
    protected float ProjectileLifeTime;
    protected Rigidbody2D ProjectileRigidbody;
    protected bool _flag = false;
    
    
    protected virtual void OnEnable()
    {
        ProjectileLifeTime = projectileLifeTime;
        ProjectileRigidbody = GetComponent<Rigidbody2D>();
    }
    
    
    public void Reset()
    {
        ProjectileLifeTime = projectileLifeTime;
        transform.position = Vector2.zero;
        ProjectileRigidbody.linearVelocity = Vector2.zero;
        transform.rotation = Quaternion.identity;
        _flag = false;
    }
    
    public void SetPlayerTransform(Transform player)
    {
        // Do nothing
    }
}
