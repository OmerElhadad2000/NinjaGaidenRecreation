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
        {"SpecialShurikenAttack", 15},
        {"FireballAttack", 20},
        {"FireCircleAttack", 25}
    };
    
    private void OnEnable()
    {
        CollectablesManager.RedSpiritPointsCollected += OnRedSpiritPointsCollected;
        CollectablesManager.BlueSpiritPointsCollected += OnBlueSpiritPointsCollected;
        CollectablesManager.RegularShurikenCollected += OnRegularShurikenCollected;
        CollectablesManager.SpecialShurikenCollected += OnSpecialShurikenCollected;
        CollectablesManager.FlameCollected += OnFlameCollected;
        CollectablesManager.FireCircleCollected += OnFireCircleCollected;
        CollectablesManager.SpecialJumpCollected += OnSpecialJumpCollected;
    }
    
    private void OnRedSpiritPointsCollected()
    {
        _mana += 10;
        Debug.Log("Red spirit points collected. Current mana: " +  _mana);
    }
    
    private void OnBlueSpiritPointsCollected()
    {
        _mana += 5;
        Debug.Log("Blue spirit points collected. Current mana: " +  _mana);
    }
    
    private void OnRegularShurikenCollected()
    {
        // get a shuriken prefab from the pool
    }
    
    private void OnSpecialShurikenCollected()
    {
        // get a special shuriken prefab from the pool
    }
    
    private void OnFlameCollected()
    {
        // get a flame prefab from the pool
    }
    
    private void OnFireCircleCollected()
    {
        // get a fire circle prefab from the pool
    }
    
    private void OnSpecialJumpCollected()
    {
        // get a special jump prefab from the pool
    }
    
    private void OnDisable()
    {
        CollectablesManager.RedSpiritPointsCollected -= OnRedSpiritPointsCollected;
        CollectablesManager.BlueSpiritPointsCollected -= OnBlueSpiritPointsCollected;
        CollectablesManager.RegularShurikenCollected -= OnRegularShurikenCollected;
        CollectablesManager.SpecialShurikenCollected -= OnSpecialShurikenCollected;
        CollectablesManager.FlameCollected -= OnFlameCollected;
        CollectablesManager.FireCircleCollected -= OnFireCircleCollected;
        CollectablesManager.SpecialJumpCollected -= OnSpecialJumpCollected;
    }
}
