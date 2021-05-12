using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;
    public Text HP;
    private int MaxHP;

    public void SetHud(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Level: " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        MaxHP = unit.maxHP;
        HP.text = "HP: " + unit.currentHP + "/" + unit.maxHP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
        if(hp < 0)
            HP.text = "HP: " + 0 + "/" + MaxHP;
        else
            HP.text = "HP: " + hp + "/" + MaxHP;
    }
}
