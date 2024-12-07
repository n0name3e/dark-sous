using UnityEngine;

[CreateAssetMenu(fileName = "Jagermeister", menuName = "Items/Jagermeister", order = 2)]
public class JagermaisterItem : Consumable
{
    public int recoveryAmount;

    public override void UseItem(AnimatorHandler playerAnimatorHandler, WeaponSlotManager weaponSlotManager, 
        PlayerItemManager playerItemManager, PlayerManager playerManager)
    {
        if (playerManager.isInteracting)
            return;
        base.UseItem(playerAnimatorHandler, weaponSlotManager, playerItemManager, playerManager);
        playerItemManager.healAmount = recoveryAmount;
        if (model != null)
        {
            GameObject bottle = Instantiate(model, weaponSlotManager.rightWeaponSlot.gameObject.transform);
            //weaponSlotManager.rightWeaponSlot.UnloadWeapon();
        }
        quantity--;
    }
}
