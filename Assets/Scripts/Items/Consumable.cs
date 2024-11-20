using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public int maxCount = 1;
    public int quantity = 1;

    public GameObject model;

    public string consumeAnimation;

    public virtual void UseItem(AnimatorHandler playerAnimatorHandler, WeaponSlotManager weaponSlotManager, PlayerItemManager playerItemManager, PlayerManager playerManager)
    {
        if (playerManager.isInteracting)
            return;
        if (quantity >= 1)
        {
            playerAnimatorHandler.PlayTargetAnimation(consumeAnimation, true, false, true);
            //quantity -= 1;
        }
    }
}
