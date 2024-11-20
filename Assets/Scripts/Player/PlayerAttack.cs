using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private AnimatorHandler animatorHandler;
    private WeaponSlotManager weaponSlotManager;
    private PlayerStats playerStats;

    private void Awake()
    {
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        playerStats = GetComponent<PlayerStats>();
    }
    public void HandleLightAttack(WeaponItem weapon)
    {
        if (playerStats.currentStamina <= 0)
            return;
        weaponSlotManager.attackingWeapon = weapon;
        animatorHandler.PlayTargetAnimation(weapon.LightAttackAnimation, true, false);
    }
}
