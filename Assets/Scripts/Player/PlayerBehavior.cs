using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private float invincibilityDuration = 2f; // Duration of invincibility in seconds
    [SerializeField] private float invincibilityAlpha = 0.5f; // Alpha value when invincible
    [SerializeField] private float flickerInterval = 0.1f; // Interval between flickers
    
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;    
    private bool _isInvincible;
    private int _currentHealth;
    private bool _isDead;
    
    public static event Action<Vector2> PlayerHit;
    private void Start()
    {
        _currentHealth = maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    private void TakeDamage(int damage)
    {
        if (_isDead || _isInvincible) return;
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            _isDead = true;
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    private void Heal(int healAmount)
    {
        if (_isDead) return;
        _currentHealth += healAmount;
        if (_currentHealth > maxHealth)
        {
            _currentHealth = maxHealth;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!(other.gameObject.CompareTag("Regular Enemy") || other.gameObject.CompareTag("Enemy Attack"))) return;
        PlayerHit?.Invoke(other.GetContact(0).point);
        TakeDamage(1);
    }

    private void Die()
    {
        Debug.Log("Player died");
    }

    private IEnumerator InvincibilityCoroutine()
    {
        _isInvincible = true;
        float elapsedTime = 0f;
        IgnoreEnemyLayer();
        while (elapsedTime < invincibilityDuration)
        {
            _spriteRenderer.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, invincibilityAlpha);
            yield return new WaitForSeconds(flickerInterval);
            _spriteRenderer.color = _originalColor;
            yield return new WaitForSeconds(flickerInterval);
            elapsedTime += 2 * flickerInterval;
        }
        ReenableEnemyLayer();
        _spriteRenderer.color = _originalColor;
        _isInvincible = false;
    }
    
    private void IgnoreEnemyLayer()
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Regular Enemies");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
    }

    private void ReenableEnemyLayer()
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Regular Enemies");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
    }
}