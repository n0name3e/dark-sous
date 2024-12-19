using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider hpBar;
    public Slider yellowHpBar; 
    public Text bossName;


    public static BossHealthBar Instance;
    public EnemyStats enemy;

    private float timer;

    private void OnEnable()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        //bossName = GetComponentInChildren<Text>();
        bossName.text = "Beast Clergyman";
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SetYellowBar();
        }
    }
    public void SetMaxHP(float maxHp)
    {
        hpBar.maxValue = maxHp;
        hpBar.value = maxHp;
        yellowHpBar.maxValue = maxHp;
    }
    public void SetHP(float health)
    {
        hpBar.value = health;
        timer = 0.85f;
    }
    private void SetYellowBar()
    {
        yellowHpBar.value = Mathf.Max(yellowHpBar.value - (450 * Time.deltaTime), hpBar.value);
    }
}
