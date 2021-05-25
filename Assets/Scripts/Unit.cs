using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element { Electricity, Wind, Earth }

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;

    public int maxHP;
    public int currentHP;

    public int EXPToGive;

    //public enum Element { Electricity, Wind, Earth }

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
            IncreaseEXPToLevel();
        }
        else
        {
            LeveledUp = false;
            HealthIncrease = HealthGain;
            DamageIncrease = DamageGain;
        }
    }

    void IncreaseEXPToLevel()
    {
        switch (unitLevel)
        {
            case 2:
                EXPToLevel += 20;
                break;
            case 3:
                EXPToLevel += 60;
                break;
            case 4:
                EXPToLevel += 100;
                break;
            case 5:
                EXPToLevel += 140;
                break;
            case 6:
                EXPToLevel += 180;
                break;
            case 7:
                EXPToLevel += 220;
                break;
            case 8:
                EXPToLevel += 40;
                break;
            case 9:
                EXPToLevel += 45;
                break;
        }
    }
}
