using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOverworldSpawning : MonoBehaviour
{
    public InformationStorage info;

    // Start is called before the first frame update
    void Start()
    {
        info = GameObject.FindGameObjectWithTag("InfoStorage").GetComponent<InformationStorage>();
        if (info.EnemiesFought.Contains(this.name))
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
