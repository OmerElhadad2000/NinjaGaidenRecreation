using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAttacks : MonoBehaviour
{
    private int _mana;
    private bool _groundedState = true;
    [SerializeField] private Transform attackPoint;
    
    [SerializeField] private GameObject standingSwordAttackCollider;
    [SerializeField] private GameObject crouchingSwordAttackCollider;
    [SerializeField] private GameObject currentSwordCollider;
    
    private Dictionary<string, int> _attackCostDictionary = new Dictionary<string, int> 
    {
        {"JumpSwordAttack", 0},
        {"ShurikenAttack", 5},
        {"FireCircleAttack", 0}
    };
    
    public static event Action<int> ManaChanged;
    public static event Action<Sprite> AttackChanged;
    
    public static event Action SwordAttack;
    
    public static event Action JumpingSwordAttack;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwordAttack?.Invoke();
            PreformSwordAttack();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpingSwordAttack();
        }
        
        //add if jumping grounded the gameobject should be turned off. (sword attack)
    }

    private void OnEnable()
    {
        CollectablesManager.RedSpiritPointsCollected += UpdateSpiritPoints;
        CollectablesManager.BlueSpiritPointsCollected += UpdateSpiritPoints;
        CollectablesManager.RegularShurikenCollected += OnRegularShurikenCollected;
        CollectablesManager.FireCircleCollected += OnFireCircleCollected;
        CollectablesManager.SpecialJumpCollected += OnSpecialJumpCollected;
        PlayerMovement.Crouching += OnCrouching;
        PlayerMovement.Grounded += UpdateGroundedState;
    }

    private void UpdateSpiritPoints(int spiritPoints)
    {
        print("Updating spirit points with " + spiritPoints);
        _mana += spiritPoints;
        ManaChanged?.Invoke(_mana);
    }

    private void OnRegularShurikenCollected(Sprite shurikenSprite)
    {
        // will update the canvas with the pic of the shuriken
        AttackChanged?.Invoke(shurikenSprite);
    }
    
    private void PreformSwordAttack()
    {
        currentSwordCollider.SetActive(true);
    }
    
    private void OnFireCircleCollected(Sprite fireCircleSprite)
    {
        // will update the canvas with the pic of the fire circle
        AttackChanged?.Invoke(fireCircleSprite);
    }
    
    private void OnSpecialJumpCollected(Sprite specialJumpSprite)
    {
        // will update the canvas with the pic of the special jump
        AttackChanged?.Invoke(specialJumpSprite);
    }
    
    private void OnCrouching(bool isCrouching)
    {
        currentSwordCollider = isCrouching ? crouchingSwordAttackCollider : standingSwordAttackCollider;
    }

    private void OnSwordAttackEnded()
    {
        currentSwordCollider.SetActive(false);
    }
    
    private void UpdateGroundedState(bool isGrounded)
    {
        _groundedState = isGrounded;
    }
    
    private void OnJumpingSwordAttack()
    {
        // if (_mana < _attackCostDictionary["JumpSwordAttack"]) return;
        // _mana -= _attackCostDictionary["JumpSwordAttack"];
        // ManaChanged?.Invoke(_mana);
        JumpingSwordAttack?.Invoke();
    }
    
    
    
    

    
    private void OnDisable()
    {
        CollectablesManager.RedSpiritPointsCollected -= UpdateSpiritPoints;
        CollectablesManager.BlueSpiritPointsCollected -= UpdateSpiritPoints;
        CollectablesManager.RegularShurikenCollected -= OnRegularShurikenCollected;
        CollectablesManager.FireCircleCollected -= OnFireCircleCollected;
        CollectablesManager.SpecialJumpCollected -= OnSpecialJumpCollected;
        PlayerMovement.Crouching -= OnCrouching;
        PlayerMovement.Grounded -= UpdateGroundedState;
    }
}
