using System;
using Mono_Pool;
using UnityEngine;

public class BasicProjectileMovement : MonoBehaviour, IPoolableObject
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileLifeTime;
    
    private float _projectileLifeTime;
    private Rigidbody2D _projectileRigidbody;
    
    private void OnEnable()
    {
        _projectileLifeTime = projectileLifeTime;
        _projectileRigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        _projectileRigidbody.linearVelocity = transform.right * projectileSpeed;
        _projectileLifeTime -= Time.deltaTime;
        if (_projectileLifeTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player Attack"))
        {
            gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        _projectileLifeTime = projectileLifeTime;
        transform.position = Vector2.zero;
    }
}
