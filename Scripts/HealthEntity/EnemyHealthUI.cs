using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EnemyHealthUI : MonoBehaviour
{
    public static EnemyHealthUI instance;
    public Slider hpSlider;
    public TextMeshProUGUI enemyNameText;

    private void Awake()
    {
        instance = this;
    }

    public void SetHP(float currentHP, float maxHP, string enemyName)
    {
        hpSlider.value = currentHP / maxHP;
        enemyNameText.text = enemyName;
    }
}
