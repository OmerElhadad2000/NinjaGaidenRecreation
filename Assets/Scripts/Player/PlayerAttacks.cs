using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    private int _mana;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject standingSwordAttackCollider;
    [SerializeField] private GameObject crouchingSwordAttackCollider;
    private GameObject _currentSwordCollider;
    
    [SerializeField] private GameObject fireCircleAttack;
    
    
    private string _currentAttack;
    
    private readonly Dictionary<string, int> _attackCostDictionary = new Dictionary<string, int> 
    {
        {"JumpSwordAttack", 0},
        {"ShurikenAttack", 5},
        {"FireCircleAttack", 0},
        {"SmokeBomb", 5}
    };
    
    public static event Action<int> ManaChanged;
    public static event Action<Sprite> AttackChanged;
    
    public static event Action SwordAttack;
    
    public static event Action JumpingSwordAttack;
    
    public static event Action FireCircleTick;
    
    public static event Action ShurikenAttack;
    
    public static event Action SmokeBombPreform;

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (Input.GetKeyDown(KeyCode.Q) && _currentAttack == "SmokeBomb")
            {
                PreformSmokeBomb();
            }
            if (!Input.GetKeyDown(KeyCode.Q) || _currentAttack != "ShurikenAttack") return;
            ShurikenAttack?.Invoke();
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            PreformShurikenAttack();
            
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            if (!Input.GetKey(KeyCode.Q) || _currentAttack != "JumpSwordAttack") return;
            print("Jumping sword attack");
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            OnJumpingSwordAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            SwordAttack?.Invoke();
            PreformSwordAttack();
        }
    }

    private void PreformSmokeBomb()
    {
        if (_mana < _attackCostDictionary["SmokeBomb"]) return;
        _mana -= _attackCostDictionary["SmokeBomb"];
        ManaChanged?.Invoke(_mana);
        SmokeBombPreform?.Invoke();
    }

    private void OnEnable()
    {
        CollectablesManager.RedSpiritPointsCollected += UpdateSpiritPoints;
        CollectablesManager.BlueSpiritPointsCollected += UpdateSpiritPoints;
        CollectablesManager.RegularShurikenCollected += OnRegularShurikenCollected;
        CollectablesManager.FireCircleCollected += OnFireCircleCollected;
        CollectablesManager.SpecialJumpCollected += OnSpecialJumpCollected;
        PlayerMovement.Crouching += OnCrouching;
        GameManager.Instance.PlayerDied += OnPlayerDeath;
        CollectablesManager.SmokeBombCollected += OnSmokeBombCollected;
    }

    private void UpdateSpiritPoints(int spiritPoints)
    {
        print("Updating spirit points with " + spiritPoints);
        _mana += spiritPoints;
        if (_mana < 0)
        {
            _mana = 0;
        }
        ManaChanged?.Invoke(_mana);
    }

    private void OnRegularShurikenCollected(Sprite shurikenSprite)
    {
        // will update the canvas with the pic of the shuriken
        AttackChanged?.Invoke(shurikenSprite);
        _currentAttack = "ShurikenAttack";
        
    }
    
    private void OnSmokeBombCollected(Sprite smokeBombSprite)
    {
        // will update the canvas with the pic of the smoke bomb
        AttackChanged?.Invoke(smokeBombSprite);
        _currentAttack = "SmokeBomb";
    }
    
    private void PreformShurikenAttack()
    {
        if (_mana < _attackCostDictionary["ShurikenAttack"] || _currentAttack != "ShurikenAttack") { return; }
        _mana -= _attackCostDictionary["ShurikenAttack"];
        ManaChanged?.Invoke(_mana);
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        var projectile = PlayerProjectilePool.Instance.Get();
        projectile.transform.position = attackPoint.position;
    }
    
    private void PreformSwordAttack()
    {
        _currentSwordCollider.SetActive(true);
    }
    
    
    private void OnFireCircleCollected(Sprite fireCircleSprite)
    {
        // will update the canvas with the pic of the fire circle
        AttackChanged?.Invoke(fireCircleSprite);
        _currentAttack = "FireCircleAttack";
        StartCoroutine(FireCircleCoroutine());

    }
    
    private void OnSpecialJumpCollected(Sprite specialJumpSprite)
    {
        // will update the canvas with the pic of the special jump
        AttackChanged?.Invoke(specialJumpSprite);
        _currentAttack = "JumpSwordAttack";
    }
    
    private void OnCrouching(bool isCrouching)
    {
        _currentSwordCollider = isCrouching ? crouchingSwordAttackCollider : standingSwordAttackCollider;
    }

    private void OnSwordAttackEnded()
    {
        _currentSwordCollider.SetActive(false);
    }
    
    private void OnPlayerDeath()
    {
        _mana = 0;
        ManaChanged?.Invoke(_mana);
        _currentAttack = null;
        AttackChanged?.Invoke(null);
        _currentSwordCollider.SetActive(false);
    }
    
    private void OnJumpingSwordAttack()
    {
        if (_mana < _attackCostDictionary["JumpSwordAttack"] || _currentAttack != "JumpSwordAttack") return;
        _mana -= _attackCostDictionary["JumpSwordAttack"];
        ManaChanged?.Invoke(_mana);
        JumpingSwordAttack?.Invoke();
    }
    
    private IEnumerator FireCircleCoroutine()
    {
        fireCircleAttack.SetActive(true);
        for (int i = 5; i > 0; i--)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            FireCircleTick?.Invoke();
            yield return new WaitForSeconds(1);
        }
        fireCircleAttack.SetActive(false);
        AttackChanged?.Invoke(null);
        _currentAttack = null;
    }

    
    private void OnDisable()
    {
        CollectablesManager.RedSpiritPointsCollected -= UpdateSpiritPoints;
        CollectablesManager.BlueSpiritPointsCollected -= UpdateSpiritPoints;
        CollectablesManager.RegularShurikenCollected -= OnRegularShurikenCollected;
        CollectablesManager.FireCircleCollected -= OnFireCircleCollected;
        CollectablesManager.SpecialJumpCollected -= OnSpecialJumpCollected;
        PlayerMovement.Crouching -= OnCrouching;
    }
}
