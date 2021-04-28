using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, PARTY2TURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject PartyMember2Prefab;
    public GameObject EnemyPrefab;

    public Transform PlayerBattleStation;
    public Transform PartyMember2Station;
    public Transform EnemyBattleStattion;

    Unit playerUnit;
    Unit Party2Unit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHud playerHud;
    public BattleHud Party2Hud;
    public BattleHud enemyHud;

    public Image magicUI;

    public BattleState state;
    
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetUpBattle());
        magicUI.gameObject.SetActive(false);
    }

    IEnumerator SetUpBattle()
    {
        GameObject playerGo = Instantiate(PlayerPrefab, PlayerBattleStation);
        playerUnit = playerGo.GetComponent<Unit>();

        GameObject Party2Go = Instantiate(PartyMember2Prefab, PartyMember2Station);
        Party2Unit = Party2Go.GetComponent<Unit>();

        GameObject enemyGo = Instantiate(EnemyPrefab, EnemyBattleStattion);
        enemyUnit = enemyGo.GetComponent<Unit>();

        dialogueText.text ="Monster " + enemyUnit.unitName + " attacks!";

        playerHud.SetHud(playerUnit);
        Party2Hud.SetHud(Party2Unit);
        enemyHud.SetHud(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack(Unit CurrentUnit)
    {
        bool isdead = enemyUnit.TakeDamage(CurrentUnit.damage);

        enemyHud.SetHP(enemyUnit.currentHP);
        dialogueText.text = enemyUnit.unitName + " took " + CurrentUnit.damage + " points of damage";

        yield return new WaitForSeconds(2f);

        if (isdead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            if(Party2Unit.currentHP > 0 && state == BattleState.PLAYERTURN)
            {
                state = BattleState.PARTY2TURN;
                Party2Turn();
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(enemyTurn());
            }
        }
    }

    IEnumerator PlayerElectricAttack(Unit CurrentUnit)
    {
        bool isdead;

        //Check if enemy is weak, resistent or neutral to the magic type
        if(enemyUnit.element == Unit.Element.Wind)
        {
            isdead = enemyUnit.TakeDamage(CurrentUnit.damage * 2);
            dialogueText.text = enemyUnit.unitName + " took " + (CurrentUnit.damage * 2) + " points of damage";
        }
        else if(enemyUnit.element == Unit.Element.Earth)
        {
            isdead = enemyUnit.TakeDamage(CurrentUnit.damage/2);
            dialogueText.text = enemyUnit.unitName + " took " + (CurrentUnit.damage / 2) + " points of damage";
        }
        else
        {
            isdead = enemyUnit.TakeDamage(CurrentUnit.damage);
            dialogueText.text = enemyUnit.unitName + " took " + CurrentUnit.damage + " points of damage";
        }

        enemyHud.SetHP(enemyUnit.currentHP);
        magicUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        //check if enemy is dead
        if (isdead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            if (Party2Unit.currentHP > 0 && state == BattleState.PLAYERTURN)
            {
                state = BattleState.PARTY2TURN;
                Party2Turn();
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(enemyTurn());
            }
        }
    }

    IEnumerator PlayerWindAttack(Unit CurrentUnit)
    {
        bool isdead;

        //Check if enemy is weak, resistent or neutral to the magic type
        if (enemyUnit.element == Unit.Element.Earth)
        {
            isdead = enemyUnit.TakeDamage(CurrentUnit.damage * 2);
            dialogueText.text = enemyUnit.unitName + " took " + (CurrentUnit.damage * 2) + " points of damage";
        }
        else if (enemyUnit.element == Unit.Element.Electricity)
        {
            isdead = enemyUnit.TakeDamage(CurrentUnit.damage / 2);
            dialogueText.text = enemyUnit.unitName + " took " + (CurrentUnit.damage / 2) + " points of damage";
        }
        else
        {
            isdead = enemyUnit.TakeDamage(CurrentUnit.damage);
            dialogueText.text = enemyUnit.unitName + " took " + CurrentUnit.damage + " points of damage";
        }

        enemyHud.SetHP(enemyUnit.currentHP);
        magicUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        //check if enemy is dead
        if (isdead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            if (Party2Unit.isDead == false && state == BattleState.PLAYERTURN)
            {
                state = BattleState.PARTY2TURN;
                Party2Turn();
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(enemyTurn());
            }
        }
    }

    IEnumerator PlayerEarthAttack(Unit CurrentUnit)
    {
        bool isdead;

        //Check if enemy is weak, resistent or neutral to the magic type
        if (enemyUnit.element == Unit.Element.Electricity)
        {
            isdead = enemyUnit.TakeDamage(CurrentUnit.damage * 2);
            dialogueText.text = enemyUnit.unitName + " took " + (CurrentUnit.damage * 2) + " points of damage";
        }
        else if (enemyUnit.element == Unit.Element.Wind)
        {
            isdead = enemyUnit.TakeDamage(CurrentUnit.damage / 2);
            dialogueText.text = enemyUnit.unitName + " took " + (CurrentUnit.damage / 2) + " points of damage";
        }
        else
        {
            isdead = enemyUnit.TakeDamage(CurrentUnit.damage);
            dialogueText.text = enemyUnit.unitName + " took " + CurrentUnit.damage + " points of damage";
        }

        enemyHud.SetHP(enemyUnit.currentHP);
        magicUI.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        //check if enemy is dead
        if (isdead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            if (Party2Unit.currentHP > 0 && state == BattleState.PLAYERTURN)
            {
                state = BattleState.PARTY2TURN;
                Party2Turn();
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(enemyTurn());
            }
        }
    }

    IEnumerator enemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(2f);

        Unit UnitTakingDamage;
        BattleHud HudForUnit;

        if(Random.Range(1,2) == 1)
        {
            UnitTakingDamage = playerUnit;
            HudForUnit = playerHud;
            if (UnitTakingDamage.currentHP <= 0)
            {
                UnitTakingDamage = Party2Unit;
                HudForUnit = Party2Hud;
            }
        }
        else
        {
            UnitTakingDamage = Party2Unit;
            HudForUnit = Party2Hud;
            if (UnitTakingDamage.currentHP <= 0)
            {
                UnitTakingDamage = playerUnit;
                HudForUnit = playerHud;
            }
        }

        bool isDead = UnitTakingDamage.TakeDamage(enemyUnit.damage);

        HudForUnit.SetHP(UnitTakingDamage.currentHP);

        yield return new WaitForSeconds(2f);

        dialogueText.text = UnitTakingDamage.unitName + " took " + enemyUnit.damage + " points of damage";

        yield return new WaitForSeconds(2f);

        if (playerUnit.isDead == true && Party2Unit.isDead == true)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            if (playerUnit.isDead != true)
            {
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }
            else
            {
                state = BattleState.PARTY2TURN;
                Party2Turn();
            }
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

    void Party2Turn()
    {
        dialogueText.text = Party2Unit.unitName + " makes a move";
    }

    IEnumerator PlayerHeal(Unit CurrentUnit)
    {
        CurrentUnit.Heal(5);

        playerHud.SetHP(CurrentUnit.currentHP);

        dialogueText.text = CurrentUnit.unitName + " healed";

        yield return new WaitForSeconds(1f);

        if (Party2Unit.currentHP > 0 && state == BattleState.PLAYERTURN)
        {
            state = BattleState.PARTY2TURN;
            Party2Turn();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(enemyTurn());
        }
    }

    public void OnAttackButton()
    {
 
        if (state != BattleState.PLAYERTURN) 
        { 
            if(state != BattleState.PARTY2TURN)
                return;
        }


        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerAttack(playerUnit));
        }
        else if (state == BattleState.PARTY2TURN)
        {
            StartCoroutine(PlayerAttack(Party2Unit));
        }
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            if (state != BattleState.PARTY2TURN)
                return;
        }

        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerHeal(playerUnit));
        }
        else if (state == BattleState.PARTY2TURN)
        {
            StartCoroutine(PlayerHeal(Party2Unit));
        }
    }

    public void OnMagicButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            if (state != BattleState.PARTY2TURN)
                return;
        }

        if (magicUI.gameObject.activeSelf == false)
        {
            magicUI.gameObject.SetActive(true);
        }
        else
        {
            magicUI.gameObject.SetActive(false);
        }
    }

    public void OnElectricButton() 
    {
        if (state != BattleState.PLAYERTURN)
        {
            if (state != BattleState.PARTY2TURN)
                return;
        }


        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerElectricAttack(playerUnit));
        }
        else if (state == BattleState.PARTY2TURN)
        {
            StartCoroutine(PlayerElectricAttack(Party2Unit));
        }
    }

    public void OnWindButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            if (state != BattleState.PARTY2TURN)
                return;
        }

        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerWindAttack(playerUnit));
        }
        else if (state == BattleState.PARTY2TURN)
        {
            StartCoroutine(PlayerWindAttack(Party2Unit));
        }
    }

    public void OnEarthButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            if (state != BattleState.PARTY2TURN)
                return;
        }

        if (state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerEarthAttack(playerUnit));
        }
        else if (state == BattleState.PARTY2TURN)
        {
            StartCoroutine(PlayerEarthAttack(Party2Unit));
        }
    }

}
