using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationStorage : MonoBehaviour
{
    [HideInInspector] public int PlayerLevel, PlayerCurrentHP, PlayerEXP, PlayerMaxHP, PlayerDamage;
    [HideInInspector] public int Party2Level, Party2CurrentHP, Party2EXP, Party2MaxHP, Party2Damage;
    [HideInInspector] public int BattleNumber;
    [HideInInspector] public Vector2 Position;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
