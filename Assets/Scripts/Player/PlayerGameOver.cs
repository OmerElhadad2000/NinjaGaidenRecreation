
using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGameOver: MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform player;
    
    public static event Action ResetPlayer; 
    
    
    private void OnEnable()
    {
        GameManager.Instance.PlayerLost += OnPlayerLost;
        GameManager.Instance.GameOverLost += OnPlayerLost;
    }
    private void ResetPlayerPosition()
    {
        player.position = spawnPoint.position;
        ResetPlayer?.Invoke();
    }

    private void OnPlayerLost()
    {
        ResetPlayerPosition();
    }
    
}