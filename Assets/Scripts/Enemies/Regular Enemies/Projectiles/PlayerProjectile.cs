
using UnityEngine;

public class PlayerProjectile: BasicProjectileMovement
{
    private bool _isFacingRight;
    private new void OnEnable()
    {
        ProjectileLifeTime = projectileLifeTime;
        ProjectileRigidbody = GetComponent<Rigidbody2D>();
        PlayerMovement.FacingRight += OnFacingRight;
    }
    private void FixedUpdate()
    {
        if (!_flag)
        {
            _flag = true;
            var direction = _isFacingRight ? Vector2.right : Vector2.left;
            ProjectileRigidbody.linearVelocity = direction * projectileSpeed;
            ProjectileLifeTime -= Time.deltaTime;
            if (ProjectileLifeTime <= 0)
            {
                PlayerProjectilePool.Instance.Return(this);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Regular Enemy") || other.CompareTag("Boss Enemy"))
        {
            PlayerProjectilePool.Instance.Return(this);
        }
    }
    
    private void OnFacingRight(bool isFacingRight)
    {
        _isFacingRight = isFacingRight;
    }
    
    private void OnDisable()
    {
        PlayerMovement.FacingRight -= OnFacingRight;
    }
    
    
}