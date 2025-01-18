using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class CanvasManager : MonoSingleton<CanvasManager>
{
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text lives;
    [SerializeField] private TMP_Text spiritPoints;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private List<Image> ninjaHealthBars; // List of health bar images
    [SerializeField] private List<Image> enemyHealthBars; // List of health bar images
    [SerializeField] private Image specialAttackSlot;
    private int _currentScore;
    private void Start()
    {
        _currentScore = 0;
        score.text = _currentScore.ToString("D6");
        lives.text = "03";
        spiritPoints.text = "00";
        timer.text = "150";
        specialAttackSlot.enabled = false;
    }
    private void OnEnable()
    {
        PlayerAttacks.AttackChanged += OnSpecialAttackCollected;
        
        CollectablesManager.RedPointsCollected += UpdateScore;
        CollectablesManager.BluePointsCollected += UpdateScore;
        
        PlayerAttacks.ManaChanged += UpdateSpiritPoints;
        
        PlayerBehavior.PlayerHealthChanged += UpdateNinjaHealth;
    }

    private void UpdateScore(int value)
    {
        _currentScore += value;
        score.text = _currentScore.ToString("D6");
    }

    private void UpdateLives(int value)
    {
        lives.text = value.ToString("D2");
    }

    private void UpdateNinjaHealth(int currentHealth)
    {
        for (int i = 0; i < ninjaHealthBars.Count; i++)
        {
            ninjaHealthBars[i].enabled = i < currentHealth;
        }
    }
    
    public void ResetEnemyHealth(int currentHealth)
    {
        foreach (var t in enemyHealthBars)
        {
            t.enabled = true;
        }
    }
    
    public void UpdateEnemyHealth(int currentHealth)
    {
        for (int i = 0; i < enemyHealthBars.Count; i++)
        {
            enemyHealthBars[i].enabled = i < currentHealth;
        }
    }

    private void UpdateSpiritPoints(int value)
    {
        spiritPoints.text = value.ToString("D2");
    }
    
    public void UpdateTimer(float value)
    {
        timer.text = value.ToString("D3");
    }
    
    private void OnSpecialAttackCollected(Sprite specialAttackSprite)
    {
        specialAttackSlot.enabled = true;
        specialAttackSlot.sprite = specialAttackSprite;
    }
    
    public void DisableSpecialAttackSlot()
    {
        specialAttackSlot.enabled = false;
    }
    
    public void OnGameOver()
    {
        _currentScore = 0;
        score.text = _currentScore.ToString("D6");
        lives.text = "03";
        spiritPoints.text = "00";
        timer.text = "150";
        specialAttackSlot.enabled = false;
    }

    private void OnDisable()
    {
        PlayerAttacks.AttackChanged -= OnSpecialAttackCollected;
        PlayerAttacks.ManaChanged -= UpdateSpiritPoints;
    }
}