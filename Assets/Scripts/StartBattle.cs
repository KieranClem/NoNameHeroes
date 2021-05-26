using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBattle : MonoBehaviour
{
    public InformationStorage PlayerInfo;
    public Animator transition;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public IEnumerator ChangeToBattleScene()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("BattleScene");
    }

    public void ChangeToDungeon()
    {
        SceneManager.LoadScene("Dungeon");
    }
}
