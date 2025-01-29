using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerBehavior : MonoBehaviour
{
    private const int MaxHealth = 16;
    [SerializeField] private float invincibilityDuration = 2f; // Duration of invincibility in seconds
    [SerializeField] private float invincibilityAlpha = 0.5f; // Alpha value when invincible
    [SerializeField] private float flickerInterval = 0.1f; // Interval between flickers
    [SerializeField] private int lives = 2;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Color _originalColor;    
    private int _currentHealth;
    private bool _isDead;
    
    public static event Action<Vector2> PlayerHit;
    public static event Action<int> PlayerHealthChanged;
    
    public static event Action PlayerDeath;
    public static event Action<int> PlayerLivesChanged;
    
    public static event Action GameOver;
    
    public static event Action DoorReached;
    
    
    private void OnEnable()
    {
        _currentHealth = MaxHealth;
        _originalColor = spriteRenderer.color;
        CollectablesManager.HealthCollected += OnPlayerHealthCollected;
        CollectablesManager.ExtraLifeCollected += OnPlayerExtraLifeCollected;
        PlayerMovement.StartInvincibility += StartInvincibility;
        GameManager.Instance.PlayerDied += OnPlayerDeath;
        GameManager.Instance.GameOverWon += OnGameOverWon;
        PlayerAttacks.SmokeBombPreform += OnSmokeBombPreform;
    }

    private void TakeDamage(int damage)
    {
        if (_isDead) return;
        _currentHealth -= damage;
        PlayerHealthChanged?.Invoke(_currentHealth);
        if (_currentHealth >= 0) return;
        _currentHealth = 0;
        _isDead = true;
        OnPlayerDeath();
    }
    
    private void StartInvincibility()
    {
        StartCoroutine(InvincibilityCoroutine());
    }
    
    private void OnPlayerHealthCollected(int healAmount)
    {
        if (_isDead) return;
        _currentHealth += healAmount;
        if (_currentHealth > MaxHealth)
        {
            _currentHealth = MaxHealth;
        }
        PlayerHealthChanged?.Invoke(_currentHealth);
    }
    
    private void OnPlayerExtraLifeCollected()
    {
        lives++;
        PlayerLivesChanged?.Invoke(lives);
    }
    
    private void OnPlayerDeath()
    {
        lives--;
        
        if (lives < 0)
        {
            //will call an event to end the game
            GameOver?.Invoke();
            return;
        }
        OnPlayerHealthCollected(MaxHealth);
        PlayerLivesChanged?.Invoke(lives);
        PlayerDeath?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (!(other.gameObject.CompareTag("Boss Enemy") || other.gameObject.CompareTag("Regular Enemy") || other.gameObject.CompareTag("Enemy Attack"))) return;
        PlayerHit?.Invoke((other.transform.position - transform.position).normalized);
        TakeDamage(other.gameObject.CompareTag("Boss Enemy") ? 2 : 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Door")) return;
        DoorReached?.Invoke();
    }

    private IEnumerator InvincibilityCoroutine()
    {
        float elapsedTime = 0f;
        IgnoreEnemyLayer();
        while (elapsedTime < invincibilityDuration)
        {
            spriteRenderer.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, invincibilityAlpha);
            yield return new WaitForSeconds(flickerInterval);
            spriteRenderer.color = _originalColor;
            yield return new WaitForSeconds(flickerInterval);
            elapsedTime += 2 * flickerInterval;
        }
        ReenableEnemyLayer();
        spriteRenderer.color = _originalColor;
    }
    
    private void IgnoreEnemyLayer()
    {
        
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Regular Enemies");
        int bossLayer = LayerMask.NameToLayer("Boss Enemies");
        
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, bossLayer, true);
    }

    private void ReenableEnemyLayer()
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Regular Enemies");
        int bossLayer = LayerMask.NameToLayer("Boss Enemies");
        
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        Physics2D.IgnoreLayerCollision(playerLayer, bossLayer, false);
    }
    
    private void OnGameOverWon()
    {
        _currentHealth = MaxHealth;
        lives = 2;
        _isDead = false;
        spriteRenderer.color = _originalColor;
        PlayerHealthChanged?.Invoke(_currentHealth);
        PlayerLivesChanged?.Invoke(lives);
    }
    
    private void OnSmokeBombPreform()
    {
        MakePlayerHalfVisibleAndTransparent();
    }
    
    private void MakePlayerHalfVisibleAndTransparent()
    {
        StartCoroutine(HalfVisibleAndTransparentCoroutine());
    }

    private IEnumerator HalfVisibleAndTransparentCoroutine()
    {
        // Make player half visible
        spriteRenderer.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0.5f);
        // Change player tag to be transparent to enemies
        ChangePlayerTag("Regular Enemy");
        
        //invencibility for 5 seconds'
        StartInvincibility();

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Return to normal opacity
        spriteRenderer.color = _originalColor;
        // Revert player tag
        ChangePlayerTag("Player");
    }

    private void ChangePlayerTag(string newTag)
    {
        gameObject.tag = newTag;
    }
    
}