
using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawnPointController: MonoBehaviour
{
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform player;
    
    public static event Action ResetPlayer; 
    
    
    private void OnEnable()
    {
        GameManager.Instance.PlayerDied += OnPlayerDied;
        GameManager.Instance.GameOverLost += OnPlayerDied;
        GameManager.Instance.GameOverWon += OnPlayerDied;
        PlayerBehavior.DoorReached += OnDoorReached;
    }
    private void ResetPlayerPosition()
    {
        player.position = spawnPoint.position;
        ResetPlayer?.Invoke();
    }

    private void OnPlayerDied()
    {
        ResetPlayerPosition();
    }
    
    private void OnDoorReached()
    {
        player.position = bossSpawnPoint.position;
        ResetPlayer?.Invoke();
    }
    
    private void OnDisable()
    {
        GameManager.Instance.PlayerDied -= OnPlayerDied;
        GameManager.Instance.GameOverLost -= OnPlayerDied;
        PlayerBehavior.DoorReached -= ResetPlayerPosition;
    }
    
}