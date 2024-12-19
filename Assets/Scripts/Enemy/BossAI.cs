using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    private EnemyMovement movement;
    private EnemyAnimatorHandler animatorHandler;
    private EnemyStats stats;

    [SerializeField] private GameObject mainWeaponObject;
    private EnemyWeapon weapon;
    [SerializeField] private GameObject dagger;
    private EnemyWeapon daggerWeapon;
    [SerializeField] private GameObject hammer;
    private EnemyWeapon hammerWeapon;

    private Transform player;
    private Vector3 acceleratingTarget;

    private string lastAttack = string.Empty;

    private bool isPhaseTwo = false;
    private float distance;
    private float angle;
    /// <summary>
    /// increases after attacking, resets after retreating move, determines agression and likeness of some moves (makes combos endable)
    /// </summary>
    private float agression = 0f;
    private float passiveTimer = 0f; // depends on agression, determines time when boss is passive
    private float rotationSpeedMultiplier = 1f;

    private bool isMoving;
    private bool isInteracting;
    private bool isAccelerating = false;
    private bool isRetreating = false;
    public bool isActive = false;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        animatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
        weapon = mainWeaponObject.GetComponent<EnemyWeapon>();
        stats = GetComponent<EnemyStats>();
        daggerWeapon = dagger.GetComponent<EnemyWeapon>();
        hammerWeapon = hammer.GetComponent<EnemyWeapon>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        isPhaseTwo = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;
        //print("Agro: " + agression + " last atk: " + lastAttack);
        passiveTimer -= Time.deltaTime;
        isInteracting = animatorHandler.animator.GetBool("isInteracting");
        movement.RotateTowardsTarget(player, rotationSpeedMultiplier);
        if (isAccelerating)
        {
            AccelerateTowardsPlayer(movement.acceleratingSpeed);
            movement.AccelerateTowardsTarget(acceleratingTarget);
        }
        if (isRetreating)
        {
            movement.AccelerateTowardsTarget(acceleratingTarget);
        }
        if (!isAccelerating && !isRetreating && !isMoving)
        {
            movement.Stop();
        }
        if (isInteracting)
            return;

        distance = Vector3.Distance(transform.position, player.position);
        angle = CalculateAngle();
        if (((float) stats.currentHealth / (float) stats.maxHealth) <= 0.6f && !isPhaseTwo)
        {
            isPhaseTwo = true;
            PhaseTransition();
            return;
        }
        if (distance >= 3f)
        {
            if (lastAttack != "DaggerThrow" && Random.value <= 0.2f * Time.deltaTime)
            {
                DaggerThrow();
                return;
            }
        }
        if (isPhaseTwo && lastAttack != "HammerLeap" && distance >= 3f && Random.value <= 0.2f * Time.deltaTime)
        {
            HammerLeap();
            return;
        }
        if (distance > 2f && distance <= 3.5f && Random.value <= 0.2f * Time.deltaTime)
        {
            FlyingPlunge();
            return;
        }
        if (distance >= 2f)
        {
            if (lastAttack != "RunningDoubleSlash" && agression >= 25 && distance <= 4f
                && Random.value <= 0.4f * Time.deltaTime)
            {
                RunningDoubleSlash();
                return;
            }
            Move();
            return;
        }
        isMoving = false;
        
        if (agression >= 60)
        {
            if (Random.Range(0, 100) <= 75)
                Retreat();
            else
                RetreatingDaggerSlash();          
            return;
        }
        float r = Random.Range(0, 100);
        if (isPhaseTwo)
        {
            if (r <= 15f && lastAttack != "HammerSwing")
            {
                HammerSwing();
                return;
            }
            else if (r <= 30f && lastAttack != "FourComboSlashes")
            {
                FourComboSlashes();
                return;
            }
            else if (r <= 45f && lastAttack != "TripleStrike")
            {
                TripleStrike();
                return;
            }
        }
        r = Random.Range(0, 100);
        //DoubleDaggerSlash();
        if (lastAttack != "Uppercut" && (angle <= 10 && angle >= -25) && r <= 25 + 50 * System.Convert.ToInt32(isMoving))
        {
            Uppercut();
        }
        else if (lastAttack != "DoubleDaggerSlash" && distance < 1.2f && r <= 50)
        {
            DoubleDaggerSlash();
        }
        else if (lastAttack != "HorizontalSwing" && r <= 65)
        {
            HorizontalSwing();
        }
        else if (lastAttack != "DelayedHorizontalSwing" && r <= 90)
        {
            DelayedHorizontalSwing();
        }
        else
        {
            Retreat();
        }
    }

    public void ResetBoss()
    {
        isActive = false;
        agression = 0;
        isPhaseTwo = false;
        isAccelerating = false;
        isRetreating = false;
        isMoving = false;
    }

    private void ChooseAttack()
    {

    }

    private void Move()
    {
        movement.MoveTowardsTarget(player);
        animatorHandler.UpdateMovementAnimation();
        rotationSpeedMultiplier = 1f;
        isMoving = true;
    }

    private float CalculateAngle()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        return Vector3.Angle(transform.forward, direction) -90f;
    }
    public void AccelerateTowardsPlayer(float speed)
    {
        movement.acceleratingSpeed = speed;
        isAccelerating = true;
        acceleratingTarget = player.position;
    }
    public void StartRetreating()
    {
        movement.acceleratingSpeed = 3f;
        isRetreating = true;
        int r = Random.Range(0, 100);
        if (r < 33)
            acceleratingTarget = (transform.right * 100); // for some reasons it's actually backwards
        else if (r < 67)
            acceleratingTarget = transform.forward * 100;
        else
            acceleratingTarget = transform.forward * (-100);
    }
    public void StopAcceleration()
    {
        isAccelerating = false;
        isRetreating = false;
        movement.Stop();
    }
    public void SetImpactType(OnHitImpactType type)
    {
        weapon.impactType = type;
    }
    private void Retreat()
    {
        animatorHandler.PlayAnimation("Retreat", true);
        agression = 0;
    }
    public void CheckDaggerAttack()
    {
        if (distance <= 2f && Random.Range(0, 100) <= 40)
        {
            DoubleDaggerSlash();
            return;
        }
        if (distance <= 2.5f && Random.Range(0, 100) <= 25)
        {
            RetreatingDaggerSlash();
        }
    }
    #region Attacks
    private void Uppercut()
    {
        agression += 15;
        rotationSpeedMultiplier = 0.1f;
        weapon.damage = 40;
        weapon.impactType = OnHitImpactType.SlightStagger;
        animatorHandler.PlayAnimation("Uppercut", true);
        lastAttack = "Uppercut";
    }
    public void UppercutFollowUp()
    {
        if (distance >= 3f || Random.Range(0, 100) <= 50)
            return;
        agression += 10;
        rotationSpeedMultiplier = 0.1f;
        weapon.damage = 70;
        weapon.impactType = OnHitImpactType.KnockDown;
        animatorHandler.PlayAnimation("UppercutFollowUp", true);
    }
    private void DelayedHorizontalSwing()
    {
        agression += 15;
        rotationSpeedMultiplier = 0.1f;
        weapon.damage = 100;
        weapon.impactType = OnHitImpactType.KnockDown;
        animatorHandler.PlayAnimation("DelayedHorizontalSwing", true);
        lastAttack = "DelayedHorizontalSwing";
    }
    private void HorizontalSwing()
    {
        agression += 10;
        rotationSpeedMultiplier = 0.3f;
        weapon.damage = 75;
        weapon.impactType = OnHitImpactType.KnockDown;
        animatorHandler.PlayAnimation("HorizontalSwing", true);
        lastAttack = "HorizontalSwing";
    }
    public void HorizontalSwingFollowUp()
    {
        if (distance >= 3.5f || Random.Range(0, 100) <= 35)
            return;
        agression += 5;
        rotationSpeedMultiplier = 0.4f;
        weapon.damage = 70;
        animatorHandler.PlayAnimation("HorizontalSwingFollowUp", true);
    }
    private void RunningDoubleSlash()
    {
        agression += 30;
        rotationSpeedMultiplier = 1f;
        weapon.damage = 80;
        weapon.impactType = OnHitImpactType.SlightStagger;
        animatorHandler.PlayAnimation("RunningDoubleSlash", true);
        lastAttack = "RunningDoubleSlash";
    }

    private void DoubleDaggerSlash()
    {
        agression += 10;
        rotationSpeedMultiplier = 0.1f;
        dagger.SetActive(true);
        daggerWeapon.damage = 40;
        daggerWeapon.impactType = OnHitImpactType.SlightStagger;
        animatorHandler.PlayAnimation("DoubleDaggerSlash", true);
        lastAttack = "DoubleDaggerSlash";
    }
    private void RetreatingDaggerSlash()
    {
        agression = 0;
        rotationSpeedMultiplier = 0.1f;
        dagger.SetActive(true);
        daggerWeapon.damage = 50;
        daggerWeapon.impactType = OnHitImpactType.SlightStagger;
        animatorHandler.PlayAnimation("RetreatingDaggerSlash", true);
        lastAttack = "RetreatingDaggerSlash";
    }
    private void DaggerThrow()
    {
        agression += 0;
        rotationSpeedMultiplier = 0.1f;
        dagger.SetActive(true);
        animatorHandler.PlayAnimation("DaggerThrow", true);
        lastAttack = "DaggerThrow";
    }
    public void DaggerThrowFollowUp()
    {
        float r = Random.Range(0, 100);
        if (distance >= 2.5f && distance <= 5f)
        {
            if (isPhaseTwo && r <= 40)
            {
                HammerLeap();
            }
            else if (r >= 40 && r <= 70)
            {
                FlyingPlunge();
            }
        }
        else if (distance <= 2.5f && r <= 20)
        {
            HorizontalSwing();
        }
    }
    private void FlyingPlunge()
    {
        agression += 10;
        rotationSpeedMultiplier = 0f;
        weapon.damage = 70;
        weapon.impactType = OnHitImpactType.KnockDown;
        animatorHandler.PlayAnimation("FlyingPlunge", true);
        lastAttack = "FlyingPlunge";
    }
    private void PhaseTransition()
    {
        hammer.SetActive(true);
        agression = 0;
        rotationSpeedMultiplier = 1f;
        hammerWeapon.damage = 100;
        hammerWeapon.impactType = OnHitImpactType.KnockDown;
        animatorHandler.PlayAnimation("PhaseTransition", true);
    }
    private void HammerSwing()
    {
        hammer.SetActive(true);
        agression += 15;
        rotationSpeedMultiplier = 0.4f;
        weapon.damage = 10;
        weapon.impactType = OnHitImpactType.SlightStagger;
        hammerWeapon.damage = 80;
        hammerWeapon.impactType = OnHitImpactType.KnockDown;
        animatorHandler.PlayAnimation("HammerSwing", true);
        lastAttack = "HammerSwing";
    }
    private void HammerLeap()
    {
        hammer.SetActive(true);
        agression += 25;
        rotationSpeedMultiplier = 0.8f;
        hammerWeapon.damage = 120;
        hammerWeapon.impactType = OnHitImpactType.KnockDown;
        animatorHandler.PlayAnimation("HammerLeap", true);
        lastAttack = "HammerLeap";
    }
    private void TripleStrike()
    {
        dagger.SetActive(true);
        agression += 25;
        rotationSpeedMultiplier = 0.5f;
        daggerWeapon.damage = 60;
        daggerWeapon.impactType = OnHitImpactType.SlightStagger;
        weapon.damage = 120;
        weapon.impactType = OnHitImpactType.KnockDown;
        animatorHandler.PlayAnimation("TripleStrike", true);
        lastAttack = "TripleStrike";
    }
    private void FourComboSlashes()
    {
        dagger.SetActive(true);
        agression += 40;
        rotationSpeedMultiplier = 0.6f;
        daggerWeapon.damage = 60;
        daggerWeapon.impactType = OnHitImpactType.SlightStagger;
        weapon.damage = 80;
        weapon.impactType = OnHitImpactType.SlightStagger;
        animatorHandler.PlayAnimation("FourComboSlashes", true);
        lastAttack = "FourComboSlashes";
    }
    #endregion
}
