using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private WeaponSlotManager weaponSlotManager;

    public WeaponItem leftWeapon;
    public WeaponItem rightWeapon;
    public WeaponItem unarmedWeapon;
    public Consumable consumable;

    public QuickSlotsUI quickSlotsUI;
    

    public WeaponItem[] weaponsInLeftSlot = new WeaponItem[2];
    public WeaponItem[] weaponsInRightSlot = new WeaponItem[2];
    public Consumable[] consumables = new Consumable[3];

    public int currentLeftWeaponIndex = 0;
    public int currentRightWeaponIndex = 0;
    public int currentConsumableIndex = 0;

    private void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }
    private void Start()
    {
        quickSlotsUI = QuickSlotsUI.instance;
        leftWeapon = weaponsInLeftSlot[currentLeftWeaponIndex];
        rightWeapon = weaponsInRightSlot[currentRightWeaponIndex];
        consumable = consumables[currentConsumableIndex];
        weaponSlotManager.LoadWeaponInSlot(leftWeapon, Hand.Left);
        weaponSlotManager.LoadWeaponInSlot(rightWeapon, Hand.Right);

        for (int i = 0; i < consumables.Length; i++)
        {
            consumable.quantity = consumable.maxCount;
        }

        quickSlotsUI.UpdateConsumableQuickSlot(consumable);
    }
    public void ChangeRightHandWeapon()
    {
        currentRightWeaponIndex++;
        if (weaponsInRightSlot.Length == 0)
        {
            rightWeapon = unarmedWeapon;
            currentRightWeaponIndex = -1;
        }

        if (currentRightWeaponIndex > weaponsInRightSlot.Length - 1)
        {
            // start from 0
            for (int i = 0; i < weaponsInRightSlot.Length; i++)
            {
                if (weaponsInRightSlot[i] != null)
                {
                    rightWeapon = weaponsInRightSlot[i];
                    currentRightWeaponIndex = i;
                    weaponSlotManager.LoadWeaponInSlot(rightWeapon, Hand.Right);
                    return;
                }
            }
        }
        for (int i = currentRightWeaponIndex; i < weaponsInRightSlot.Length; i++)
        {
            if (weaponsInRightSlot[i] != null)
            {
                rightWeapon = weaponsInRightSlot[i];
                currentRightWeaponIndex = i;
                weaponSlotManager.LoadWeaponInSlot(rightWeapon, Hand.Right);
            }
        }
    }
    public void ChangeConsumable()
    {
        currentConsumableIndex++;
        if (consumables.Length == 0)
        {
            consumable = null;
            quickSlotsUI.UpdateConsumableQuickSlot(null);
            currentConsumableIndex = -1;
        }

        if (currentConsumableIndex > consumables.Length - 1)
        {
            // start from 0
            for (int i = 0; i < consumables.Length; i++)
            {
                if (consumables[i] != null)
                {
                    consumable = consumables[i];
                    quickSlotsUI.UpdateConsumableQuickSlot(consumable);
                    currentConsumableIndex = i;
                    return;
                }
            }
        }
        for (int i = currentConsumableIndex; i < consumables.Length; i++)
        {
            if (consumables[i] != null)
            {
                consumable = consumables[i];
                quickSlotsUI.UpdateConsumableQuickSlot(consumable);
                currentConsumableIndex = i;
            }
        }
    }
}
