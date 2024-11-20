using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    public WeaponHolderSlot leftWeaponSlot;
    public WeaponHolderSlot rightWeaponSlot;

    private PlayerWeapon leftHandWeaponDamageCollider;
    private PlayerWeapon rightHandWeaponDamageCollider;

    public WeaponItem attackingWeapon;

    private PlayerStats playerStats;
    private QuickSlotsUI quickSlotsUI; 

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();

        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.handSlot == Hand.Left)
            {
                leftWeaponSlot = weaponSlot;
            }
            else if (weaponSlot.handSlot == Hand.Right)
            {
                rightWeaponSlot = weaponSlot;
            }
        }
    }
    public void LoadWeaponInSlot(WeaponItem weaponItem, Hand handSlot)
    {
        if (handSlot == Hand.Left)
        {
            leftWeaponSlot.LoadWeaponModel(weaponItem);
            LoadLeftHandWeaponDamageCollider();
        }
        else if (handSlot == Hand.Right)
        {
            rightWeaponSlot.LoadWeaponModel(weaponItem);
            LoadRightHandWeaponDamageCollider();
            quickSlotsUI.UpdateWeaponQuickSlots(Hand.Right, weaponItem);
        }
    }
    public void DrainStaminaLightAttack()
    {
        playerStats.DealStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStaminaConsumption * attackingWeapon.lightAttackStaminaMultiplier));
    }

    public void LoadLeftHandWeaponDamageCollider()
    {
        leftHandWeaponDamageCollider = leftWeaponSlot.currentWeaponModel.GetComponent<PlayerWeapon>();
    }
    public void LoadRightHandWeaponDamageCollider()
    {
        rightHandWeaponDamageCollider = rightWeaponSlot.currentWeaponModel.GetComponent<PlayerWeapon>();
    }

    public void EnableLeftWeaponDamageCollider()
    {
        leftHandWeaponDamageCollider.EnableDamageCollider();
    }
    public void EnableRightWeaponDamageCollider()
    {
        rightHandWeaponDamageCollider.EnableDamageCollider();
    }
    public void DisableLeftWeaponDamageCollider()
    {
        leftHandWeaponDamageCollider.DisableDamageCollider();
    }
    public void DisableRightWeaponDamageCollider()
    {
        rightHandWeaponDamageCollider.DisableDamageCollider();
    }
}
