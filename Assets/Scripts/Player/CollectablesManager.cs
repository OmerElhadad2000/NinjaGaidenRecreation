using System;
using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    public static event Action<int> RedSpiritPointsCollected;
    public static event Action<int> BlueSpiritPointsCollected;
    public static event Action<int> HealthCollected;
    public static event Action ExtraLifeCollected;
    public static event Action TimeFreezeCollected;
    public static event Action<int> RedPointsCollected;
    public static event Action<int> BluePointsCollected;
    public static event Action<Sprite> RegularShurikenCollected;
    public static event Action<Sprite> FireCircleCollected;
    public static event Action<Sprite> SpecialJumpCollected;

    private void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.CompareTag("Red Spirit Points"))
        {
            RedSpiritPointsCollected?.Invoke(10);
        }
        else if (other.gameObject.CompareTag("Blue Spirit Points"))
        {
            BlueSpiritPointsCollected?.Invoke(5);
        }

        else if (other.gameObject.CompareTag("Health"))
        {
            HealthCollected?.Invoke(6);
        }

        else if (other.gameObject.CompareTag("Extra Life"))
        {
            ExtraLifeCollected?.Invoke();
        }

        else if (other.gameObject.CompareTag("Time Freeze"))
        {
            TimeFreezeCollected?.Invoke();
        }

        else if (other.gameObject.CompareTag("Red Points"))
        {
            RedPointsCollected?.Invoke(1000);
        }

        else if (other.gameObject.CompareTag("Blue Points"))
        {
            BluePointsCollected?.Invoke(500);
        }

        else if (other.gameObject.CompareTag("Regular Shuriken"))
        {
            RegularShurikenCollected?.Invoke(other.gameObject.GetComponent<SpriteRenderer>().sprite);
        }

        else if (other.gameObject.CompareTag("Fire Circle"))
        {
            FireCircleCollected?.Invoke(other.gameObject.GetComponent<SpriteRenderer>().sprite);
        }

        else if (other.gameObject.CompareTag("Special Jump"))
        {
            SpecialJumpCollected?.Invoke(other.gameObject.GetComponent<SpriteRenderer>().sprite);
        }
    }
}
