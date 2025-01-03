﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorHandler : MonoBehaviour
{
    public Animator animator;
    private BossAI ai;
    private EnemyMovement movement;
    [SerializeField] private Collider damageCollider;
    [SerializeField] private GameObject holyDagger;
    [SerializeField] private Collider daggerCollider;
    [SerializeField] private GameObject holyHammer;
    [SerializeField] private Collider hammerCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        ai = GetComponentInParent<BossAI>();
        movement = GetComponentInParent<EnemyMovement>();
        //damageCollider = GetComponentInChildren<Collider>();
    }
    public void UpdateMovementAnimation()
    {
        animator.SetFloat("y", 1f);
    }
    public void PlayAnimation(string name, bool isInteracting)
    {
        animator.CrossFade(name, 0.2f);
        animator.SetBool("isInteracting", isInteracting);
    }
    public void CheckForDaggerSlashes()
    {
        ai.CheckDaggerAttack();
    }
    public void AccelerateTowardsPlayer(float speed)
    {
        ai.AccelerateTowardsPlayer(speed);
    }
    public void StopAccelerating()
    {
        ai.StopAcceleration();
    }
    public void SetImpactType(OnHitImpactType type)
    {
        ai.SetImpactType(type);
    }
    public void StartRetreating()
    {
        ai.StartRetreating();
    }
    public void UppercutFollowUp()
    {
        ai.UppercutFollowUp();
    }
    public void HorizontalSwingFollowUp()
    {
        ai.HorizontalSwingFollowUp();
    }
    public void DaggerThrowFollowUp()
    {
        ai.DaggerThrowFollowUp();
    }
    public void ThrowDagger()
    {
        GameObject thrownDagger = Instantiate(Resources.Load("throwingDagger") as GameObject, holyDagger.transform.position, Quaternion.identity);
        thrownDagger.GetComponent<Rigidbody>().velocity = transform.right * -15f;
        Destroy(thrownDagger, 3f);
    }
    public void EnableDagger()
    {
        holyDagger.SetActive(true);
    }
    public void DisableDagger()
    {
        holyDagger.SetActive(false);
    }
    public void DisableHammer()
    {
        holyHammer.SetActive(false);
    }
    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }
    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }
    public void EnableHolyDaggerCollider()
    {
        daggerCollider.enabled = true;
    }
    public void DisableHolyDaggerCollider()
    {
        daggerCollider.enabled = false;
    }
    public void EnableHolyHammerCollider()
    {
        hammerCollider.enabled = true;
    }
    public void DisableHolyHammerCollider()
    {
        hammerCollider.enabled = false;
    }
}
