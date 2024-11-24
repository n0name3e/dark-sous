using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItem : Item
{
    public GameObject model;
    public bool isUnarmed;

    public string LightAttackAnimation;

    public int baseStaminaConsumption = 30;
    public float lightAttackStaminaMultiplier = 1;
}
