using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    private EnemyMovement movement;
    private EnemyAnimatorHandler animatorHandler;
    private EnemyWeapon weapon;

    [SerializeField] private GameObject dagger;
    private EnemyWeapon daggerWeapon;

    private Transform player;
    private Vector3 acceleratingTarget;

    private string lastAttack = string.Empty;

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

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        animatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
        weapon = GetComponentInChildren<EnemyWeapon>();
        daggerWeapon = dagger.GetComponent<EnemyWeapon>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        //print("Agro: " + agression + " last atk: " + lastAttack);
        passiveTimer -= Time.deltaTime;
        isInteracting = animatorHandler.animator.GetBool("isInteracting");
        movement.RotateTowardsTarget(player, rotationSpeedMultiplier);
        if (isAccelerating)
        {
            movement.AccelerateTowardsTarget(acceleratingTarget);
        }
        if (isInteracting)
            return;

        distance = Vector3.Distance(transform.position, player.position);
        angle = CalculateAngle();
        if (distance >= 2f)
        {
            if (lastAttack != "RunningDoubleSlash" && passiveTimer <= 0f && agression >= 25 && distance <= 4f)
            {
                RunningDoubleSlash();
            }
            movement.MoveTowardsTarget(player);
            animatorHandler.UpdateMovementAnimation();
            rotationSpeedMultiplier = 1f;
            isMoving = true;

            return;
        }
        HorizontalSwing();
        isMoving = false;
        return;
        if (agression >= 100)
        {
            Retreat();
            return;
        }
        //DoubleDaggerSlash();
        if (lastAttack != "Uppercut" && (angle <= 10 && angle >= -25) && Random.Range(0, 100) <= 25 + 50 * System.Convert.ToInt32(isMoving))
        {
            Uppercut();
            isMoving = false;
            return;
        }
        if (lastAttack != "DoubleDaggerSlash" && distance < 1.2f && Random.Range(0, 100) <= 25)
        {
            DoubleDaggerSlash();
            return;
        }
        if (lastAttack != "DelayedHorizontalSwing" && Random.Range(0, 100) <= 25)
        {
            DelayedHorizontalSwing();
        }
        else
        {
            HorizontalSwing();
        }

        isMoving = false;
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
        isAccelerating = true;
        acceleratingTarget = (transform.right * 100); // for some reasons it's actually backwards
    }
    public void StopAcceleration()
    {
        isAccelerating = false;
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
        if (distance <= 2f && Random.Range(0, 100) <= 50)
        {
            DoubleDaggerSlash();
        }
    }
    #region Attacks
    private void Uppercut()
    {
        agression += 20;
        rotationSpeedMultiplier = 0.1f;
        weapon.damage = 40;
        weapon.impactType = OnHitImpactType.SlightStagger;
        animatorHandler.PlayAnimation("Uppercut", true);
        lastAttack = "Uppercut";
    }
    public void UppercutFollowUp()
    {
        if (distance >= 3f || Random.Range(0, 100) <= 75)
            return;
        agression += 10;
        rotationSpeedMultiplier = 0.1f;
        weapon.damage = 70;
        weapon.impactType = OnHitImpactType.KnockDown;
        animatorHandler.PlayAnimation("UppercutFollowUp", true);
    }
    private void DelayedHorizontalSwing()
    {
        agression += 35;
        rotationSpeedMultiplier = 0.1f;
        weapon.damage = 70;
        weapon.impactType = OnHitImpactType.KnockDown;
        animatorHandler.PlayAnimation("DelayedHorizontalSwing", true);
        lastAttack = "DelayedHorizontalSwing";
    }
    private void HorizontalSwing()
    {
        agression += 15;
        rotationSpeedMultiplier = 0.3f;
        weapon.damage = 60;
        weapon.impactType = OnHitImpactType.KnockDown;
        animatorHandler.PlayAnimation("HorizontalSwing", true);
        lastAttack = "HorizontalSwing";
    }
    public void HorizontalSwingFollowUp()
    {
        agression += 10;
        rotationSpeedMultiplier = 0.4f;
        weapon.damage = 60;
        animatorHandler.PlayAnimation("HorizontalSwingFollowUp", true);
        if (distance >= 3.5f || Random.Range(0, 100) <= 75)
            return;
    }
    private void RunningDoubleSlash()
    {
        agression += 40;
        rotationSpeedMultiplier = 1f;
        weapon.damage = 50;
        weapon.impactType = OnHitImpactType.SlightStagger;
        animatorHandler.PlayAnimation("RunningDoubleSlash", true);
        lastAttack = "RunningDoubleSlash";
    }

    private void DoubleDaggerSlash()
    {
        agression += 15;
        rotationSpeedMultiplier = 0.1f;
        dagger.SetActive(true);
        daggerWeapon.damage = 20;
        daggerWeapon.impactType = OnHitImpactType.SlightStagger;
        animatorHandler.PlayAnimation("DoubleDaggerSlash", true);
    }
    #endregion
}
