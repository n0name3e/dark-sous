using UnityEngine;
using UnityEngine.UI;

public class QuickSlotsUI : MonoBehaviour
{
    public Image rightWeaponIcon;
    public Image consumableIcon;
    
    public void UpdateWeaponQuickSlots(Hand hand, WeaponItem weaponItem)
    {
        if (hand == Hand.Right)
        {
            if (weaponItem.itemIcon != null)
            {
                rightWeaponIcon.sprite = weaponItem.itemIcon;
                rightWeaponIcon.enabled = true;
            }
            else
            {
                rightWeaponIcon.enabled = false;
            }
        }
    }
    public void UpdateConsumableQuickSlot(Consumable consumable)
    {
        if (consumable == null)
        {
            consumableIcon.sprite = null;
            return;
        }
        if (consumable.itemIcon != null)
        {
            consumableIcon.sprite = consumable.itemIcon;
            consumableIcon.enabled = true;
        }
        else
        {
            consumableIcon.enabled = false;
        }
    }
}
