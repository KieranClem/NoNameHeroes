using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBattle : MonoBehaviour
{
    public InformationStorage PlayerInfo;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeToBattleScene()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void ChangeToDungeon()
    {
        SceneManager.LoadScene("Dungeon");
    }
}
