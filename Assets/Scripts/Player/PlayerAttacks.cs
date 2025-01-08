using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    
    private int _mana;
    
    private Dictionary<string, int> _attackCostDictionary = new Dictionary<string, int> {{"JumpSwordAttack", 10}, 
        {"ShuriKenAttack", 5}, {"SpecialShuriKenAttack", 15}, {"FireBallAttack", 20}, {"FireCircleAttack", 25}};
    private void JumpSwordAttack()
    {
        //Attack logic
    }
    
    private void ShuriKenAttack()
    {
        //Attack logic
    }
    
    private void SpecialShuriKenAttack()
    {
        //Attack logic
    }
    
    private void FireBallAttack()
    {
        //Attack logic
    }
    
    private void FireCircleAttack()
    {
        //Attack logic
    }
}
