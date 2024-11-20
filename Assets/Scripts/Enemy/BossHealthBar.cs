using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public static BossHealthBar Instance;
    public EnemyStats enemyStats;
    private Slider hpBar;
    public Text bossName;
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        hpBar = GetComponent<Slider>();
        //bossName = GetComponentInChildren<Text>();
        bossName.name = "Beast Clergyman";
    }

    public void SetMaxHP(float maxHp)
    {
        hpBar.maxValue = maxHp;
        hpBar.value = maxHp;
    }
    public void SetHP(float health)
    {
        hpBar.value = health;
    }
}
