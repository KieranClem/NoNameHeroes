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

}
