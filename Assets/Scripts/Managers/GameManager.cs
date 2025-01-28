using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private bool IsGamePaused { get; set; }
    private const float InitTimer = 150;
    private float _timer;

    public event Action<int> OnTimerTick;

    private void Start()
    {
        _timer = InitTimer;
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (_timer > 0)
        {
            yield return new WaitForSeconds(1);
            _timer--;
            OnTimerTick?.Invoke(Mathf.CeilToInt(_timer));
        }
    }

    private void PauseGame()
    {
        IsGamePaused = true;
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        IsGamePaused = false;
        Time.timeScale = 1;
    }

    public void TogglePauseGame()
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