﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class InformationStorage : MonoBehaviour
{
    [HideInInspector] public int PlayerLevel, PlayerCurrentHP, PlayerEXP, PlayerEXPToNext, PlayerMaxHP, PlayerDamage;
    [HideInInspector] public int Party2Level, Party2CurrentHP, Party2EXP, Party2EXPToNext, Party2MaxHP, Party2Damage;
    [HideInInspector] public string EnemyName;
    [HideInInspector] public Element EnemyElement;
    [HideInInspector] public int EnemyLevel, EnemyMaxHP, EnemyDamage, EnemyEXPToGive;
    [HideInInspector] public int BattleNumber;
    [HideInInspector] public Vector3 Position;
    [HideInInspector] public List<string> EnemiesFought;
    
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("InfoStorage").Length > 1)
            Destroy(this.gameObject);
        
        DontDestroyOnLoad(this.gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (EnemiesFought.Contains("BossMonster") && SceneManager.GetActiveScene().name =="Dungeon")
        {
            GameObject DoorToUnlock = GameObject.FindGameObjectWithTag("Door");
            DoorToUnlock.GetComponent<TilemapRenderer>().enabled = true;
            DoorToUnlock.GetComponent<Collider2D>().enabled = true;
            DoorToUnlock.transform.Find("SceneChanger").gameObject.SetActive(true);
        }
    }
}
