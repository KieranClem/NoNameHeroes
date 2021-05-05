using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;

    public int maxHP;
    public int currentHP;

    public enum Element { Electricity, Wind, Earth }

    public Element element;

    [HideInInspector] public bool isDead;

    public int EXPToLevel = 10;
    [HideInInspector] public int EXP;

    public bool TakeDamage(int DamageDelt)
    {
        currentHP -= DamageDelt;

        if (currentHP <= 0)
        {
            isDead = true;
            return true;
        }
        else
            return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public void CheckForLevelUp(int ExpAdd, out bool LeveledUp,out int HealthIncrease, out int DamageIncrease)
    {
        EXP += ExpAdd;
        int HealthGain = 0;
        int DamageGain = 0;
        if (EXP >= EXPToLevel)
        {
            unitLevel += 1;
            DamageGain = Random.Range(1, 11);
            damage += DamageGain;
            HealthGain = Random.Range(1, 11);
            maxHP += HealthGain;
            currentHP += HealthGain;
            LeveledUp = true;
            HealthIncrease = HealthGain;
            DamageIncrease = DamageGain;
        }
        else
        {
            LeveledUp = false;
            HealthIncrease = HealthGain;
            DamageIncrease = DamageGain;
        }
    }
}
