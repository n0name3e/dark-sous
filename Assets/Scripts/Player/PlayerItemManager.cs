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

    public QuickSlotsUI quickSlotsUI;

    public int healAmount;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerInventory = GetComponent<PlayerInventory>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        playerManager = GetComponent<PlayerManager>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
    }

    public void UseCurrentConsumable()
    {
        Consumable currentConsumable = GetCurrentConsumable();
        currentConsumable.UseItem(animatorHandler, weaponSlotManager, this, playerManager);
        quickSlotsUI.UpdateConsumableQuickSlot(currentConsumable);
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
