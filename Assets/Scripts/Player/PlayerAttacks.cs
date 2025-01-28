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
        {"FireCircleAttack", 0}
    };
    
    public static event Action<int> ManaChanged;
    public static event Action<Sprite> AttackChanged;
    
    public static event Action SwordAttack;
    
    public static event Action JumpingSwordAttack;
    
    public static event Action FireCircleTick;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                print("Jumping sword attack");
                OnJumpingSwordAttack();
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                print("Shuriken attack");
                return;
            }
            SwordAttack?.Invoke();
            PreformSwordAttack();
        }
    }

    private void OnEnable()
    {
        CollectablesManager.RedSpiritPointsCollected += UpdateSpiritPoints;
        CollectablesManager.BlueSpiritPointsCollected += UpdateSpiritPoints;
        CollectablesManager.RegularShurikenCollected += OnRegularShurikenCollected;
        CollectablesManager.FireCircleCollected += OnFireCircleCollected;
        CollectablesManager.SpecialJumpCollected += OnSpecialJumpCollected;
        PlayerMovement.Crouching += OnCrouching;
        GameManager.Instance.PlayerLost += OnPlayerDeath;
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
    
    private void PreformShurikenAttack()
    {
        if (_mana < _attackCostDictionary["ShurikenAttack"] || _currentAttack != "ShurikenAttack") return;
        _mana -= _attackCostDictionary["ShurikenAttack"];
        ManaChanged?.Invoke(_mana);
        // will instantiate a shuriken prefab
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
