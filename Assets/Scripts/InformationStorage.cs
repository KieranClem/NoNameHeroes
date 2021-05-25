using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationStorage : MonoBehaviour
{
    [HideInInspector] public int PlayerLevel, PlayerCurrentHP, PlayerEXP, PlayerMaxHP, PlayerDamage;
    [HideInInspector] public int Party2Level, Party2CurrentHP, Party2EXP, Party2MaxHP, Party2Damage;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
