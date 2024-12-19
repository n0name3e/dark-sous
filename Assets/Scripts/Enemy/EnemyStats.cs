using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int maxHealth = 450;
    public int currentHealth;
    [SerializeField] private Collider coll;
    [SerializeField] private Rigidbody rb;
    private Animator animator;
    public bool isBoss = true;

    public Vector3 startingPosition { get; private set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        startingPosition = transform.position;
    }

    void Start()
    {
        currentHealth = maxHealth;
        if (isBoss)
        {
            BossHealthBar.Instance.enemy = this;
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
            coll.enabled = true;
            rb.isKinematic = false;
            return;
        }
    }
    public void Revive()
    {
        animator.Play("Movement");
    }
}
