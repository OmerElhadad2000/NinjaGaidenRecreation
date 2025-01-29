using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private bool IsGamePaused { get; set; }
    private const float InitTimer = 150;
    private float _timer;

    public event Action<int> TimerTick;
    public event Action PlayerDied;
    
    public event Action GamePause;
    
    public event Action GameResume;
    
    public event Action GameStart;
    public event Action GameOverLost;
    
    public event Action GameOverWon;
    
    private bool _gameOverWon;
    
    
    private void Start()
    {
        GameStart?.Invoke();
        _timer = InitTimer;
        StartCoroutine(TimerCoroutine());
    }

    private void OnEnable()
    {
        BossMovement.BossDied += OnGameOverWon;
        PlayerBehavior.PlayerDeath += OnPlayerDeath;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void OnPlayerDeath()
    {
        PlayerDied?.Invoke();
    }
    
    private void OnGameOverWon()
    {
        StartCoroutine(HandleGameOverWon());
    }

    private IEnumerator HandleGameOverWon()
    {
        _gameOverWon = true;
        while (_timer > 0)
        {
            yield return new WaitForSeconds(0.1f);
            _timer--;
            TimerTick?.Invoke(Mathf.CeilToInt(_timer));
            // Assuming you have a method to update the score
            CanvasManager.Instance.UpdateScore(1000);
        }
        GameOverWon?.Invoke();
        _timer = InitTimer;
        _gameOverWon = false;
    }

    private IEnumerator TimerCoroutine()
    {
        if (_gameOverWon) yield break;
        while (_timer > 0)
        {
            yield return new WaitForSeconds(1);
            _timer--;
            TimerTick?.Invoke(Mathf.CeilToInt(_timer));
        }
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        PlayerDied?.Invoke();
        _timer = InitTimer;
    }

    private void PauseGame()
    {
        GamePause?.Invoke();
        IsGamePaused = true;
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        GameResume?.Invoke();
        IsGamePaused = false;
        Time.timeScale = 1;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void TogglePauseGame()
    {
        if (IsGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    private void OnDisable()
    {
        PlayerBehavior.PlayerDeath -= OnPlayerDeath;
    }
}