using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int maxHealth = 450;
    public int currentHealth;

    private Animator animator;
    public bool isBoss = true;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        if (isBoss)
        {
            BossHealthBar.Instance.SetMaxHP(maxHealth);
        }
    }

    public void DealDamage(int damage)
    {
        currentHealth -= damage;
        if (isBoss)
        {
            BossHealthBar.Instance.SetHP(currentHealth);
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.Play("Dead");
            return;
        }
    }
}
