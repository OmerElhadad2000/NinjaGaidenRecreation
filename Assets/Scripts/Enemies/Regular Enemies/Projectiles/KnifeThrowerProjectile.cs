using Mono_Pool;
using UnityEngine;
public class KnifeThrowerProjectile : BasicProjectileMovement
{
    private void FixedUpdate() {
        ProjectileRigidbody.linearVelocity = transform.right * projectileSpeed;
        ProjectileLifeTime -= Time.deltaTime;
        if (ProjectileLifeTime <= 0)
        {
            EnemyProjectilePool.Instance.Return(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player Attack"))
        {
            EnemyProjectilePool.Instance.Return(this);
        }
    }
}
    
