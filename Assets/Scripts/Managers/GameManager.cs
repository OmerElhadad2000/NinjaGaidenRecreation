using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private bool IsGamePaused { get; set; }
    private const float InitTimer = 150;
    private float _timer;

    public event Action<int> TimerTick;
    public event Action PlayerLost;
    
    public event Action GamePause;
    
    public event Action GameResume;
    
    public event Action GameStart;
    private void Start()
    {
        GameStart?.Invoke();
        _timer = InitTimer;
        StartCoroutine(TimerCoroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseGame();
        }
    }

    private IEnumerator TimerCoroutine()
    {
        while (_timer > 0)
        {
            yield return new WaitForSeconds(1);
            _timer--;
            TimerTick?.Invoke(Mathf.CeilToInt(_timer));
        }
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        PlayerLost?.Invoke();
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
}