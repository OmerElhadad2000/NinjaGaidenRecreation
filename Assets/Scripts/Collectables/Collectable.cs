using System;
using Mono_Pool;
using UnityEngine;

public class Collectable : MonoBehaviour, IPoolableObject
{
    private SpriteRenderer _spriteRenderer;
    
    public static event Action CollectableCollected;
    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Reset()
    {
        gameObject.tag = "Untagged";
        gameObject.transform.position = Vector3.zero;
        _spriteRenderer.sprite = null;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        CollectableCollected?.Invoke();
        CollectablePool.Instance.Return(this);
    }
    
    public void SetGameTag(string collectableTag)
    {
        gameObject.tag = collectableTag;
    }
    
    public void SetSpriteRenderer(Sprite newSprite)
    {
        _spriteRenderer.sprite = newSprite;
    }
    
    public void SetPlayerTransform(Transform player)
    {
        // Do nothing
    }
}
