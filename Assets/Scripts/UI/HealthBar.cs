using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider hpBar;
    public Slider yellowHpBar;
    private float timer;
    private float currentYellowBarValue;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SetYellowBar();
        }
    }

    public void SetMaxHP(int maxHp)
    {
        hpBar.maxValue = maxHp;
        hpBar.value = maxHp;
        yellowHpBar.maxValue = maxHp;
    }

    public void SetHPBar(int health)
    {
        hpBar.value = health;
        timer = 1f;
    }
    private void SetYellowBar()
    {
        yellowHpBar.value = Mathf.Max(yellowHpBar.value - (100 * Time.deltaTime), hpBar.value);
    }

}
