using System.Collections;
using UnityEngine;

public class CamerasManager : MonoSingleton<CamerasManager>
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera bossCamera;
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 3f;

    private AudioListener mainCameraAudioListener;
    private AudioListener bossCameraAudioListener;

    private void OnEnable()
    {
        PlayerBehavior.DoorReached += OnDoorReached;
        PlayerBehavior.PlayerDeath += OnPlayerDeath;
        GameManager.Instance.GameOverWon += OnPlayerDeath;
        mainCameraAudioListener = mainCamera.GetComponent<AudioListener>();
        bossCameraAudioListener = bossCamera.GetComponent<AudioListener>();
    }

    private void OnDisable()
    {
        PlayerBehavior.DoorReached -= OnDoorReached;
        PlayerBehavior.PlayerDeath -= OnPlayerDeath;
        
    }

    private void OnDoorReached()
    {
        StartCoroutine(SwitchToBossCamera());
    }

    private void OnPlayerDeath()
    {
        StartCoroutine(SwitchToMainCamera());
    }

    private IEnumerator SwitchToBossCamera()
    {
        yield return StartCoroutine(FadeOut());
        mainCamera.enabled = false;
        mainCameraAudioListener.enabled = false;
        bossCamera.enabled = true;
        bossCameraAudioListener.enabled = true;
        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator SwitchToMainCamera()
    {
        yield return StartCoroutine(FadeOut());
        bossCamera.enabled = false;
        bossCameraAudioListener.enabled = false;
        mainCamera.enabled = true;
        mainCameraAudioListener.enabled = true;
        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;
    }
}