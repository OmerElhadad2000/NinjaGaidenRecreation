using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    private int _mana;
    
    [SerializeField] private Transform attackPoint;
    
    [SerializeField] private GameObject currentAttack;
    
    private Dictionary<string, int> _attackCostDictionary = new Dictionary<string, int> 
    {
        {"JumpSwordAttack", 10},
        {"ShurikenAttack", 5},
        {"FireCircleAttack", 0}
    };
    
    public static event Action<int> ManaChanged;
    public static event Action<Sprite> AttackChanged;
    private void OnEnable()
    {
        CollectablesManager.RedSpiritPointsCollected += UpdateSpiritPoints;
        CollectablesManager.BlueSpiritPointsCollected += UpdateSpiritPoints;
        CollectablesManager.RegularShurikenCollected += OnRegularShurikenCollected;
        CollectablesManager.FireCircleCollected += OnFireCircleCollected;
        CollectablesManager.SpecialJumpCollected += OnSpecialJumpCollected;
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
    
    private void OnDisable()
    {
        CollectablesManager.RedSpiritPointsCollected -= UpdateSpiritPoints;
        CollectablesManager.BlueSpiritPointsCollected -= UpdateSpiritPoints;
        CollectablesManager.RegularShurikenCollected -= OnRegularShurikenCollected;
        CollectablesManager.FireCircleCollected -= OnFireCircleCollected;
        CollectablesManager.SpecialJumpCollected -= OnSpecialJumpCollected;
    }
}
