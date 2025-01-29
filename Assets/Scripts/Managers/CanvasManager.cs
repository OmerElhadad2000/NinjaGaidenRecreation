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
    [SerializeField] private TMP_Text stage;
    private int _currentStage = 1;
    private int _currentScore;
    private void Start()
    {
        _currentScore = 0;
        score.text = _currentScore.ToString("D6");
        lives.text = "02";
        spiritPoints.text = "00";
        timer.text = "150";
        specialAttackSlot.enabled = false;
        stage.text = _currentStage.ToString("D1");
    }
    private void OnEnable()
    {
        PlayerAttacks.AttackChanged += OnSpecialAttackCollected;
        
        CollectablesManager.RedPointsCollected += UpdateScore;
        CollectablesManager.BluePointsCollected += UpdateScore;
        
        PlayerAttacks.ManaChanged += UpdateSpiritPoints;
        
        PlayerBehavior.PlayerHealthChanged += UpdateNinjaHealth;
        PlayerBehavior.PlayerLivesChanged += UpdateLives;
        PlayerBehavior.PlayerDeath += OnPlayerDeath;
        
        BasicEnemyMovement.EnemyDiedByPlayer += UpdateScore;
        GameManager.Instance.TimerTick += UpdateTimer;
        GameManager.Instance.GameOverWon += OnGameOver;
        
    }

    private void UpdateStage()
    {
        _currentStage++;
        stage.text = _currentStage.ToString("D1");
    }

    private void ResetStage()
    {
        _currentStage = 1;
        stage.text = _currentStage.ToString("D1");
    }

    public void UpdateScore(int value)
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

    private void ResetEnemyHealth()
    {
        foreach (var t in enemyHealthBars)
        {
            t.enabled = true;
        }
    }
    
    private void OnPlayerDeath()
    {
        ResetEnemyHealth();
        UpdateTimer(150);
        DisableSpecialAttackSlot();
        ResetStage();
    }
    
    public void UpdateEnemyHealth(int currentHealth)
    {
        for (int i = 0; i < enemyHealthBars.Count; i++)
        {
            enemyHealthBars[i].enabled = i <= currentHealth;
        }
    }

    private void UpdateSpiritPoints(int value)
    {
        spiritPoints.text = value.ToString("D2");
    }
    
    private void UpdateTimer(int value)
    {
        timer.text = value.ToString("D3");
    }
    
    private void OnSpecialAttackCollected(Sprite specialAttackSprite)
    {
        if (!specialAttackSprite)
        {
            DisableSpecialAttackSlot();
            return;
        }
        specialAttackSlot.enabled = true;
        specialAttackSlot.sprite = specialAttackSprite;
    }
    
    
    private void DisableSpecialAttackSlot()
    {
        specialAttackSlot.enabled = false;
    }
    
    private void OnGameOver()
    {
        _currentScore = 0;
        score.text = _currentScore.ToString("D6");
        lives.text = "02";
        spiritPoints.text = "00";
        timer.text = "150";
        specialAttackSlot.enabled = false;
        ResetEnemyHealth();
    }
    

    private void OnDisable()
    {
        PlayerAttacks.AttackChanged -= OnSpecialAttackCollected;
        PlayerAttacks.ManaChanged -= UpdateSpiritPoints;
        PlayerBehavior.PlayerHealthChanged -= UpdateNinjaHealth;
        PlayerBehavior.PlayerLivesChanged -= UpdateLives;
        PlayerBehavior.PlayerDeath -= OnPlayerDeath;
        BasicEnemyMovement.EnemyDiedByPlayer -= UpdateScore;
        GameManager.Instance.TimerTick -= UpdateTimer;
    }
}