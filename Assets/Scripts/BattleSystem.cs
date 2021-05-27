using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, PARTY2TURN, ENEMYTURN, WON, LOST, NOATTACK }

public class BattleSystem : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject PartyMember2Prefab;
    public GameObject EnemyPrefab;
    public GameObject BossPrefab;

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

    public Animator PlayerAnimation;

    public CameraShake cameraShake;
    public Material PlayerSkyBox;
    public Material EnemySkyBox;

    public Image magicUI;

    public BattleState state;

    [HideInInspector]public InformationStorage PlayerInfo;
    [HideInInspector] public StartBattle LoadManagement;
    
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        PlayerInfo = GameObject.Find("InformationStorage").GetComponent<InformationStorage>();
        LoadManagement = PlayerInfo.gameObject.GetComponent<StartBattle>();
        StartCoroutine(SetUpBattle());
        magicUI.gameObject.SetActive(false);
    }

    IEnumerator SetUpBattle()
    {
        GameObject playerGo = Instantiate(PlayerPrefab, PlayerBattleStation);
        playerUnit = playerGo.GetComponent<Unit>();
        PlayerAnimation = playerGo.GetComponentInChildren<Animator>();

        GameObject Party2Go = Instantiate(PartyMember2Prefab, PartyMember2Station);
        Party2Unit = Party2Go.GetComponent<Unit>();

        if (PlayerInfo.EnemyName == "Big Bad Tree")
        {
            GameObject enemyGo = Instantiate(BossPrefab, EnemyBattleStattion);
            enemyUnit = enemyGo.GetComponent<Unit>();
        }
        else
        {
            GameObject enemyGo = Instantiate(EnemyPrefab, EnemyBattleStattion);
            enemyUnit = enemyGo.GetComponent<Unit>();
        }

        dialogueText.text ="Monster " + enemyUnit.unitName + " attacks!";

        if(PlayerInfo.BattleNumber > 0)
        {
            SetStats();
        }

        SetEnemyStats();

        playerHud.SetHud(playerUnit);
        Party2Hud.SetHud(Party2Unit);
        enemyHud.SetHud(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void SetStats()
    {
        //Set player information
        playerUnit.unitLevel = PlayerInfo.PlayerLevel; playerUnit.currentHP = PlayerInfo.PlayerCurrentHP; playerUnit.maxHP = PlayerInfo.PlayerMaxHP; 
        playerUnit.damage = PlayerInfo.PlayerDamage; playerUnit.EXP = PlayerInfo.PlayerEXP; playerUnit.EXPToLevel = PlayerInfo.PlayerEXPToNext;
        //Set player 2 information
        Party2Unit.unitLevel = PlayerInfo.Party2Level; Party2Unit.currentHP = PlayerInfo.Party2CurrentHP; Party2Unit.maxHP = PlayerInfo.Party2MaxHP;
        Party2Unit.damage = PlayerInfo.Party2Damage; Party2Unit.EXP = PlayerInfo.Party2EXP; Party2Unit.EXPToLevel = PlayerInfo.Party2EXPToNext;
    }

    void SetEnemyStats()
    {
        //Set enemy information
        enemyUnit.unitName = PlayerInfo.EnemyName; enemyUnit.unitLevel = PlayerInfo.EnemyLevel; enemyUnit.damage = PlayerInfo.EnemyDamage; 
        enemyUnit.maxHP = PlayerInfo.EnemyMaxHP; enemyUnit.currentHP = PlayerInfo.EnemyMaxHP; enemyUnit.EXPToGive = PlayerInfo.EnemyEXPToGive;
        enemyUnit.element = PlayerInfo.EnemyElement;
    }

    IEnumerator PlayerAttack(Unit CurrentUnit)
    {
        bool isdead = enemyUnit.TakeDamage(CurrentUnit.damage);

        enemyHud.SetHP(enemyUnit.currentHP);
        dialogueText.text = enemyUnit.unitName + " took " + CurrentUnit.damage + " points of damage";

        if(CurrentUnit.unitName == "Player")
            PlayerAnimation.Play("Player-Attack");

        StartCoroutine(cameraShake.Shake(0.1f, 5f));

        BattleState currentstate = state;
        state = BattleState.NOATTACK;
        yield return new WaitForSeconds(2f);
        state = currentstate;
        
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
        if(enemyUnit.element == Element.Wind)
        {
            isdead = enemyUnit.TakeDamage(CurrentUnit.damage * 2);
            dialogueText.text = enemyUnit.unitName + " took " + (CurrentUnit.damage * 2) + " points of damage";
        }
        else if(enemyUnit.element == Element.Earth)
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

        StartCoroutine(cameraShake.Shake(0.5f, 2f));

        BattleState currentstate = state;
        state = BattleState.NOATTACK;
        yield return new WaitForSeconds(2f);
        state = currentstate;

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
        if (enemyUnit.element == Element.Earth)
        {
            isdead = enemyUnit.TakeDamage(CurrentUnit.damage * 2);
            dialogueText.text = enemyUnit.unitName + " took " + (CurrentUnit.damage * 2) + " points of damage";
        }
        else if (enemyUnit.element == Element.Electricity)
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

        StartCoroutine(cameraShake.Shake(0.5f, 3f));

        BattleState currentstate = state;
        state = BattleState.NOATTACK;
        yield return new WaitForSeconds(2f);
        state = currentstate;


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
        if (enemyUnit.element == Element.Electricity)
        {
            isdead = enemyUnit.TakeDamage(CurrentUnit.damage * 2);
            dialogueText.text = enemyUnit.unitName + " took " + (CurrentUnit.damage * 2) + " points of damage";
        }
        else if (enemyUnit.element == Element.Wind)
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

        StartCoroutine(cameraShake.Shake(0.5f, 8f));

        BattleState currentstate = state;
        state = BattleState.NOATTACK;
        yield return new WaitForSeconds(2f);
        state = currentstate;


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
        RenderSettings.skybox = EnemySkyBox;
        
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(2f);

        Unit UnitTakingDamage;
        BattleHud HudForUnit;

        if(Random.Range(0,2) == 1)
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

        int DamageTaken = EnemyElementalDamage(UnitTakingDamage);

        bool isDead = UnitTakingDamage.TakeDamage(DamageTaken);

        HudForUnit.SetHP(UnitTakingDamage.currentHP);

        StartCoroutine(cameraShake.Shake(0.1f, 5f));

        yield return new WaitForSeconds(2f);

        dialogueText.text = UnitTakingDamage.unitName + " took " + DamageTaken + " points of damage";

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

    int EnemyElementalDamage(Unit UnitTakingDamage)
    {
        if(enemyUnit.element == Element.Earth)
        {
            //Check if unit being attacked is weak, resistent or neutral to the magic type
            if (UnitTakingDamage.element == Element.Electricity)
            {
                return enemyUnit.damage * 2;
            }
            else if (UnitTakingDamage.element == Element.Wind)
            {
                return enemyUnit.damage / 2;
            }
            else
            {
                return enemyUnit.damage;
            }
        }
        else if(enemyUnit.element == Element.Electricity)
        {
            if (UnitTakingDamage.element == Element.Wind)
            {
                return enemyUnit.damage * 2;
            }
            else if (UnitTakingDamage.element == Element.Earth)
            {
                return enemyUnit.damage / 2;
            }
            else
            {
                return enemyUnit.damage;
            }
        }
        else
        {
            //Check if unit is weak, resistent or neutral to the magic type
            if (UnitTakingDamage.element == Element.Earth)
            {
                return enemyUnit.damage * 2;
            }
            else if (UnitTakingDamage.element == Element.Electricity)
            {
                return enemyUnit.damage / 2;
            }
            else
            {
                return enemyUnit.damage;
            }

        }
    }

    void EndBattle()
    {
        if(state == BattleState.WON)
        {
            PlayerInfo.BattleNumber += 1;

            dialogueText.text = "All enemies defeated! Party gained " + enemyUnit.EXPToGive + " exp";
            
            //AddExperience/LevelUp
            int PlayerDamageIncrease, PlayerHealthIncrease;
            int Party2DamageIncrease, Party2HealthIncrease;
            bool PlayerLeveledUp = false, Party2LeveledUp = false;


            playerUnit.CheckForLevelUp(enemyUnit.EXPToGive, out PlayerLeveledUp, out PlayerHealthIncrease, out PlayerDamageIncrease);
            Party2Unit.CheckForLevelUp(enemyUnit.EXPToGive, out Party2LeveledUp, out Party2HealthIncrease, out Party2DamageIncrease);

            float WaitTime = 3f;

            if (PlayerLeveledUp || Party2LeveledUp)
            {
                if (PlayerLeveledUp)
                {
                    StartCoroutine(LevelUp(playerUnit, PlayerHealthIncrease, PlayerDamageIncrease, WaitTime));
                    WaitTime = 9f;
                    if (Party2LeveledUp)
                    {
                        StartCoroutine(LevelUp(Party2Unit, Party2HealthIncrease, Party2DamageIncrease, WaitTime));
                        WaitTime = 18f;
                    }

                    StartCoroutine(ChangeScene(WaitTime));
                }
                else if (Party2LeveledUp)
                {
                    StartCoroutine(LevelUp(Party2Unit, Party2HealthIncrease, Party2DamageIncrease, WaitTime));
                    WaitTime = 9f;
                    StartCoroutine(ChangeScene(WaitTime));
                }
            }
            else
            {
                StartCoroutine(ChangeScene(WaitTime));
            }
        }
        else if(state == BattleState.LOST)
        {
            dialogueText.text = "You where defeated";
        }
    }

    IEnumerator LevelUp(Unit PartyMember, int HealthIncrease, int DamgeIncrease, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        dialogueText.text = PartyMember.unitName + " leveled up!";
        yield return new WaitForSeconds(2f);
        dialogueText.text = "Damage increase by " + DamgeIncrease + "\n Health increased by " + HealthIncrease;
        yield return new WaitForSeconds(2f);
    }

    IEnumerator ChangeScene(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        savestats();
        LoadManagement.ChangeToDungeon();
    }

    void savestats()
    {
        //Save player stats
        PlayerInfo.PlayerLevel = playerUnit.unitLevel; PlayerInfo.PlayerCurrentHP = playerUnit.currentHP; PlayerInfo.PlayerMaxHP = playerUnit.maxHP; PlayerInfo.PlayerDamage = playerUnit.damage; PlayerInfo.PlayerEXP = playerUnit.EXP; PlayerInfo.PlayerEXPToNext = playerUnit.EXPToLevel;
        //Save party member 2 stats
        PlayerInfo.Party2Level = Party2Unit.unitLevel; PlayerInfo.Party2CurrentHP = Party2Unit.currentHP; PlayerInfo.Party2MaxHP = Party2Unit.maxHP; PlayerInfo.Party2Damage = Party2Unit.damage; PlayerInfo.Party2EXP = Party2Unit.EXP; PlayerInfo.Party2EXPToNext = Party2Unit.EXPToLevel;
    }

    void PlayerTurn()
    {
        RenderSettings.skybox = PlayerSkyBox;
        dialogueText.text = "Make a move: ";
    }

    void Party2Turn()
    {
        RenderSettings.skybox = PlayerSkyBox;
        dialogueText.text = Party2Unit.unitName + " makes a move";
    }

    IEnumerator PlayerHeal(Unit CurrentUnit)
    {
        CurrentUnit.Heal(CurrentUnit.damage);

        if (state == BattleState.PLAYERTURN)
        {
            playerHud.SetHP(CurrentUnit.currentHP);
        }
        else
        {
            Party2Hud.SetHP(CurrentUnit.currentHP);
        }

        dialogueText.text = CurrentUnit.unitName + " healed";

        BattleState currentstate = state;
        state = BattleState.NOATTACK;
        yield return new WaitForSeconds(2f);
        state = currentstate;


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
