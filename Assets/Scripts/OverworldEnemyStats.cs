using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldEnemyStats : MonoBehaviour
{
    public string EnemyName;
    public Element element;
    public int EnemyLevel, EnemyMaxHP, EnemyDamage, EnemyEXPToGive;

    public void StoreEnemy(InformationStorage info)
    {
        info.EnemyName = EnemyName;
        info.EnemyElement = element;
        info.EnemyLevel = EnemyLevel;
        info.EnemyMaxHP = EnemyMaxHP;
        info.EnemyDamage = EnemyDamage;
        info.EnemyEXPToGive = EnemyEXPToGive;
    }
}
