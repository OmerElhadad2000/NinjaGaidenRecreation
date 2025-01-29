using System;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
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

    private AudioSource backgroundMusicSource;
    private AudioSource soundEffectsSource;

    private void Awake()
    {
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        soundEffectsSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable()
    {
        BasicEnemyMovement.EnemyDiedByPlayer += PlayEnemyDeathSound;
        PlayerMovement.JumpKeyPressed += PlayPlayerJumpSound;
        PlayerAttacks.SwordAttack += PlayPlayerSwordSound;
        Collectable.CollectableCollected += PlayCollectableSound;
        PlayerAttacks.FireCircleTick += PlayerFireCircleTimer;
        PlayerBehavior.PlayerDeath += PlayPlayerDeathSound;
        PlayerBehavior.PlayerHit += PlayPlayerHitSound;
        PlayerBehavior.DoorReached += PlayBossSound;
        GameManager.Instance.GameStart += OnGameStart;
        GameManager.Instance.GamePause += OnGamePause;
        GameManager.Instance.GameResume += OnGamePause;
    }

    private void OnGameStart()
    {
        PlayBackgroundMusic(backgroundMusic);
    }

    private void PlayBossSound()
    {
        StartCoroutine(PlayBossSoundWithDelay());
    }

    private IEnumerator PlayBossSoundWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        PlayBackgroundMusic(bossMusic);
    }

    private void PlayBackgroundMusic(AudioClip clip)
    {
        backgroundMusicSource.clip = clip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    private void PlayEnemyDeathSound(int score)
    {
        PlaySoundEffect(enemyDeathSound);
    }

    private void OnGamePause()
    {
        PlaySoundEffect(togglePauseSound);
    }

    private void PlayPlayerDeathSound()
    {
        StartCoroutine(PlayPlayerDeathSoundWithDelay());
    }

    private IEnumerator PlayPlayerDeathSoundWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        PlaySoundEffect(playerDeathSound);
        yield return new WaitForSeconds(playerDeathSound.length);
        OnGameStart();
    }

    private void PlayPlayerShootSound()
    {
        PlaySoundEffect(playerShootSound);
    }

    private void PlayPlayerSwordSound()
    {
        PlaySoundEffect(playerSwordSound);
    }

    private void PlayPlayerHitSound(Vector2 hitDirection)
    {
        PlaySoundEffect(playerHitSound);
    }

    private void PlayerFireCircleTimer()
    {
        PlaySoundEffect(timerSound);
    }

    private void PlayCollectableSound()
    {
        PlaySoundEffect(collectableSound);
    }

    private void PlayPlayerJumpSound()
    {
        PlaySoundEffect(playerJumpSound);
    }

    private void PlaySoundEffect(AudioClip clip)
    {
        soundEffectsSource.PlayOneShot(clip);
    }

    private void OnDisable()
    {
        BasicEnemyMovement.EnemyDiedByPlayer -= PlayEnemyDeathSound;
        PlayerMovement.JumpKeyPressed -= PlayPlayerJumpSound;
        PlayerAttacks.SwordAttack -= PlayPlayerSwordSound;
        Collectable.CollectableCollected -= PlayCollectableSound;
    }
}