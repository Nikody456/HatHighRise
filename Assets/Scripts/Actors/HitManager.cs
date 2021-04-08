#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
using UnityEngine;
using Statistics;


public static class HitManager 
{

    public static void CalculateHit(Stats dmgDealer, Stats receiver)
    {
        if (!dmgDealer || !receiver)
            return;


        receiver.GetComponent<CharacterView>().ImHit();

        var healthRemaining = receiver.TakeDamage(dmgDealer.CurrentAttack);
        if( healthRemaining <= 0)
        {
           receiver.ImDeadHack();
        }
        AudioManager.Instance.PlaySFX("hitSound");
        // Debug.Log(healthRemaining);
    }

}
