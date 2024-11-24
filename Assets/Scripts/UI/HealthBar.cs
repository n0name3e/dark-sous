using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider hpBar;

    public void SetMaxHP(int maxHp)
    {
        hpBar.maxValue = maxHp;
        hpBar.value = maxHp;
    }

    public void SetHPBar(int health)
    {
        hpBar.value = health;
    }
}
