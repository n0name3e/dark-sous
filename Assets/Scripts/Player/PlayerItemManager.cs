using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemManager : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerInventory playerInventory;
    private AnimatorHandler animatorHandler;
    private WeaponSlotManager weaponSlotManager;
    private PlayerManager playerManager;

    public int healAmount;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerInventory = GetComponent<PlayerInventory>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        playerManager = GetComponent<PlayerManager>();
    }

    public void UseCurrentConsumable()
    {
        GetCurrentConsumable().UseItem(animatorHandler, weaponSlotManager, this, playerManager);
    }

    private Consumable GetCurrentConsumable()
    {
        return playerInventory.consumable;
    }
    public void HealPlayer()
    {
        playerStats.Heal(healAmount);
    }
}
