using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class CollectableContainer : MonoBehaviour
{
    [SerializeField] private CollectableTag collectableTag;
    [SerializeField] private Sprite collectableSprite;

    private void OnEnable()
    {
        GameManager.Instance.PlayerDied += ResetSpawner;
        GameManager.Instance.GameOverLost += ResetSpawner;
        GameManager.Instance.GameOverWon += ResetSpawner;
    }

    private void OnDeathItemDrop()
    {
        var collectable = CollectablePool.Instance.Get();
        collectable.transform.position = transform.position;
        collectable.SetGameTag(ConvertEnumToString(collectableTag));
        collectable.SetSpriteRenderer(collectableSprite);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player Attack")) return;
        OnDeathItemDrop();
        gameObject.SetActive(false);
    }

    private string ConvertEnumToString(CollectableTag tagToConvert)
    {
        return Regex.Replace(tagToConvert.ToString(), "(\\B[A-Z])", " $1");
    }
    
    private void ResetSpawner()
    {
        gameObject.SetActive(true);
    }
    
}

public enum CollectableTag
{
    RedSpiritPoints,
    BlueSpiritPoints,
    Health,
    ExtraLife,
    TimeFreeze,
    RedPoints,
    BluePoints,
    RegularShuriken,
    FireCircle,
    SpecialJump,
    SmokeBomb
}