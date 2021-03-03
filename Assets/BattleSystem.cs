using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;

    public Transform PlayerBattleStation;
    public Transform EnemyBattleStattion;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHud playerHud;
    public BattleHud enemyHud;

    public BattleState state;
    
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetUpBattle());
    }

    IEnumerator SetUpBattle()
    {
        GameObject playerGo = Instantiate(PlayerPrefab, PlayerBattleStation);
        playerUnit = playerGo.GetComponent<Unit>();

        GameObject enemyGo = Instantiate(EnemyPrefab, EnemyBattleStattion);
        enemyUnit = enemyGo.GetComponent<Unit>();

        dialogueText.text ="Monster " + enemyUnit.unitName + " attacks!";

        playerHud.SetHud(playerUnit);
        enemyHud.SetHud(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isdead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHud.SetHP(enemyUnit.currentHP);
        dialogueText.text = enemyUnit.unitName + " took " + playerUnit.damage + " points of damage";

        yield return new WaitForSeconds(2f);

        if (isdead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(enemyTurn());
        }

    }

    IEnumerator enemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHud.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        dialogueText.text = playerUnit.unitName + " took " + enemyUnit.damage + " points of damage";

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            dialogueText.text = "All enemies defeated!";
        }
        else if(state == BattleState.LOST)
        {
            dialogueText.text = "You where defeated";
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Make a move: ";
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);

        playerHud.SetHP(playerUnit.currentHP);

        dialogueText.text = "You healed";

        yield return new WaitForSeconds(1f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(enemyTurn());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }

}
