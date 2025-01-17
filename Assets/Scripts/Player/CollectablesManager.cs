using System;
using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    public static event Action RedSpiritPointsCollected;
    public static event Action BlueSpiritPointsCollected;
    public static event Action HealthCollected;
    public static event Action ExtraLifeCollected;
    public static event Action TimeFreezeCollected;
    public static event Action RedPointsCollected;
    public static event Action BluePointsCollected;
    
    public static event Action RegularShurikenCollected;
    public static event Action SpecialShurikenCollected;
    public static event Action FlameCollected;
    public static event Action FireCircleCollected;
    public static event Action SpecialJumpCollected;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Red Spirit Points"))
        {
            RedSpiritPointsCollected?.Invoke();
        }
        else if (other.CompareTag("Blue Spirit Points"))
        {
            BlueSpiritPointsCollected?.Invoke();
        }

        else if (other.CompareTag("Health"))
        {
            HealthCollected?.Invoke();
        }

        else if (other.CompareTag("Extra Life"))
        {
            ExtraLifeCollected?.Invoke();
        }

        else if (other.CompareTag("Time Freeze"))
        {
            TimeFreezeCollected?.Invoke();
        }

        else if (other.CompareTag("Red Points"))
        {
            RedPointsCollected?.Invoke();
        }

        else if (other.CompareTag("Blue Points"))
        {
            BluePointsCollected?.Invoke();
        }

        else if (other.CompareTag("Regular Shuriken"))
        {
            RegularShurikenCollected?.Invoke();
        }

        else if (other.CompareTag("Fire Circle"))
        {
            FireCircleCollected?.Invoke();
        }

        else if (other.CompareTag("Special Jump"))
        {
            SpecialJumpCollected?.Invoke();
        }
    }
}
