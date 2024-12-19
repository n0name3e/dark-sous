using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpdateManager : MonoBehaviour
{
    public static LevelUpdateManager instance;

    private float delay = Mathf.Infinity;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
    private void Update()
    {
        delay -= Time.deltaTime;
        if (delay <= 0)
        {
            ResetLevel();
        }
    }
    public void UpdateLevel(float delay)
    {
        this.delay = delay;
    }
    private void ResetLevel()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();

        foreach (EnemyStats enemy in FindObjectsOfType<EnemyStats>())
        {
            enemy.transform.position = enemy.startingPosition;
            enemy.currentHealth = enemy.maxHealth;
            
            BossAI boss = enemy.GetComponent<BossAI>();
            if (boss != null)
            {
                print(boss);
                boss.ResetBoss();
            }
        }
        BossHealthBar.Instance.SetHP(BossHealthBar.Instance.enemy.currentHealth);

        player.transform.position = player.startingPosition;
        player.Revive();

        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        foreach (Consumable consumable in inventory.consumables)
        {
            if (consumable != null)
                consumable.quantity = consumable.maxCount;
        }
        QuickSlotsUI.instance.UpdateConsumableQuickSlot(inventory.consumable);
        delay = Mathf.Infinity;
    }
}
