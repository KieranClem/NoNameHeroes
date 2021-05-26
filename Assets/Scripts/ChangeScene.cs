using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            SceneManager.LoadScene("EndScreen");
        }
    }

    public void LoadMainGame()
    {
        SceneManager.LoadScene("Dungeon");
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
