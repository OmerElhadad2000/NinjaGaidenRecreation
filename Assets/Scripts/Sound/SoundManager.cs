
using System;
using System.Collections;
using UnityEngine;

public class SoundManager: MonoSingleton<SoundManager>
{
    [SerializeField] private AudioClip enemyDeathSound;
    [SerializeField] private AudioClip playerDeathSound;
    [SerializeField] private AudioClip playerShootSound;
    [SerializeField] private AudioClip playerSwordSound;
    [SerializeField] private AudioClip playerHitSound;
    [SerializeField] private AudioClip collectableSound;
    [SerializeField] private AudioClip playerJumpSound;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private AudioClip bossDeathSound;
    [SerializeField] private AudioClip timerSound;
    [SerializeField] private AudioClip togglePauseSound;
    
    private void OnEnable()
    {
        BasicEnemyMovement.EnemyDiedByPlayer += PlayEnemyDeathSound;
        PlayerMovement.JumpKeyPressed += PlayPlayerJumpSound;
        PlayerAttacks.SwordAttack += PlayPlayerSwordSound;
        Collectable.CollectableCollected += PlayCollectableSound;
        PlayerAttacks.FireCircleTick += PlayerFireCircleTimer;
        PlayerBehavior.PlayerDeath += PlayPlayerDeathSound;
        PlayerBehavior.PlayerHit += PlayPlayerHitSound;
        GameManager.Instance.GameStart += OnGameStart;
        GameManager.Instance.GamePause += OnGamePause;
        GameManager.Instance.GameResume += OnGamePause;
        
    }
    
    private void OnGameStart()
    {
        var sound = SoundPool.Instance.Get();
        sound.SetUpClip(backgroundMusic);
        sound.PlayLoopSound(backgroundMusic);
    }
    
    private void OnBossFightStart()
    {
        var sound = SoundPool.Instance.Get();
        sound.SetUpClip(bossMusic);
        sound.PlayAudioSource();
    }
    private void PlayEnemyDeathSound(int score)
    {
         var sound = SoundPool.Instance.Get();
         sound.SetUpClip(enemyDeathSound);
         sound.PlayAudioSource();
    }
    
    private void OnGamePause()
    {
        var sound = SoundPool.Instance.Get();
        sound.SetUpClip(togglePauseSound);
        sound.PlayAudioSource();
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void PlayPlayerDeathSound()
    {
        StartCoroutine(PlayPlayerDeathSoundWithDelay());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator PlayPlayerDeathSoundWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        var sound = SoundPool.Instance.Get();
        sound.SetUpClip(playerDeathSound);
        sound.PlayAudioSource();
        yield return new WaitForSeconds(playerDeathSound.length);
        OnGameStart();
    }
    
    private void PlayPlayerShootSound()
    {
        var sound = SoundPool.Instance.Get();
        sound.SetUpClip(playerShootSound);
        sound.PlayAudioSource();
    }
    
    private void PlayPlayerSwordSound()
    {
        var sound = SoundPool.Instance.Get();
        sound.SetUpClip(playerSwordSound);
        sound.PlayAudioSource();
    }
    
    private void PlayPlayerHitSound(Vector2 hitDirection)
    {
        var sound = SoundPool.Instance.Get();
        sound.SetUpClip(playerHitSound);
        sound.PlayAudioSource();
    }
    
    private void PlayerFireCircleTimer()
    {
        var sound = SoundPool.Instance.Get();
        sound.SetUpClip(timerSound);
        sound.PlayAudioSource();
    }
    
    private void PlayCollectableSound()
    {
        var sound = SoundPool.Instance.Get();
        sound.SetUpClip(collectableSound);
        sound.PlayAudioSource();
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void PlayPlayerJumpSound()
    {
        var sound = SoundPool.Instance.Get();
        sound.SetUpClip(playerJumpSound);
        sound.PlayAudioSource();
    }
    
    private void OnDisable()
    {
        BasicEnemyMovement.EnemyDiedByPlayer -= PlayEnemyDeathSound;
        PlayerMovement.JumpKeyPressed -= PlayPlayerJumpSound;
        PlayerAttacks.SwordAttack -= PlayPlayerSwordSound;
        Collectable.CollectableCollected -= PlayCollectableSound;
    }
}
