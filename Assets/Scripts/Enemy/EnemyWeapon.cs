using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum OnHitImpactType
{
    NoKnockback = 0,
    SlightStagger = 1,
    KnockDown = 2
}
public class EnemyWeapon : MonoBehaviour
{
    public int damage = 50;
    public OnHitImpactType impactType = OnHitImpactType.SlightStagger;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerStats>()?.DealDamage(damage, impactType);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerStats>() != null)
            other.GetComponent<PlayerStats>().collidingWeapon = this;
    }
}
