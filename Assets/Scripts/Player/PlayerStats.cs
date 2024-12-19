using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 450;
    public int currentHealth;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegen = 20f;
    public float sprintingStaminaConsumption = 10f;
    public bool isImmune = false;

    public Vector3 startingPosition;

    /// <summary>
    /// the shittest idea to make enemies' weapon deal damage after i-frames expiration
    /// </summary>
    public EnemyWeapon collidingWeapon;


    public HealthBar healthBar;
    public StaminaBar staminaBar;
    private AnimatorHandler animatorHandler;
    private PlayerManager playerManager;

    private void Awake()
    {
        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        playerManager = GetComponent<PlayerManager>();
    }

    private void OnEnable()
    {
        startingPosition = transform.position;
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        healthBar.SetMaxHP(maxHealth);
        staminaBar.SetMaxStamina(maxStamina);
    }
    private void Update()
    {
        if (transform.position.y < -10 && currentHealth > 0)
        {
            Die();
        }
        if (!playerManager.isInteracting)
        {
            if (playerManager.isSprinting)
            {
                DealStaminaDamage(sprintingStaminaConsumption * Time.deltaTime);
            }
            else
            {
                ReplenishStamina();
            }
        }
    }
    private void LateUpdate()
    {
        collidingWeapon = null;
    }

    public void Revive()
    {
        animatorHandler.Revive();
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        healthBar.SetHPBar(currentHealth);
    }

    /// <summary>
    /// called after i-frames expiration to check whether player is in weapon's trigger
    /// </summary>
    public void CheckCollidingWeapon()
    {
        if (collidingWeapon != null)
        {
            DealDamage(collidingWeapon.damage, collidingWeapon.impactType, true);
        }
    }

    public void DealDamage(int damage, OnHitImpactType impactType, bool ignoreImmunity = false)
    {
        if (playerManager.isImmune && ignoreImmunity == false)
        {
            return;
        }
        currentHealth -= damage;
        healthBar.SetHPBar(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        if (impactType != OnHitImpactType.NoKnockback)
            GetComponentInChildren<WeaponSlotManager>().DisableRightWeaponDamageCollider();
        if (impactType == OnHitImpactType.SlightStagger)
            animatorHandler.PlayTargetAnimation("Damage_01", true, false);
        if (impactType == OnHitImpactType.KnockDown)
            animatorHandler.PlayTargetAnimation("KnockDown", true, false);
    }

    private void Die()
    {
        currentHealth = 0;
        healthBar.SetHPBar(0);
        animatorHandler.PlayTargetAnimation("Dead", true, false);
        LevelUpdateManager.instance.UpdateLevel(2f);
    }

    public void Heal(int heal)
    {
        currentHealth = Mathf.Min(currentHealth + heal, maxHealth);
        healthBar.SetHPBar(currentHealth);
    }
    public void DealStaminaDamage(float stamina)
    {
        currentStamina -= stamina;
        staminaBar.SetStaminaBar(currentStamina);
    }
    private void ReplenishStamina()
    {
        currentStamina = Mathf.Min(currentStamina + staminaRegen * Time.deltaTime, maxStamina);
        staminaBar.SetStaminaBar(currentStamina);
    }
}
