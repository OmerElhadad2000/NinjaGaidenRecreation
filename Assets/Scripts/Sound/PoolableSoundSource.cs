using System;
using System.Collections;

using UnityEngine;

public class PoolableSoundSource : MonoBehaviour, IPoolableSoundSource
{
    
    [SerializeField]
    private AudioSource audioSource;
    
    // PoolableSoundSource Logic
    public void SetUpClip(AudioClip clip)
    {
        audioSource.clip = clip;
    }
    
    public void PlayAudioSource()
    {
        audioSource.Play();
        float duration = audioSource.clip.length;
        StartCoroutine(ReturnToPoolAfterDuration(duration));
    }
    
    public void PlayLoopSound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
    
    private IEnumerator ReturnToPoolAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        SoundPool.Instance.Return(this);
    }

    public void Reset()
    {
        audioSource.enabled = true;
        audioSource.clip = null;
        audioSource.transform.position = Vector3.zero;
    }
}