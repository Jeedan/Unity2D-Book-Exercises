using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    // my stuff
    public List<EnemyController> Enemies = new List<EnemyController>();
    public int enemyKillCount = 0;

    public GameObject[] EnemySpawnPoints;
    public GameObject[] EnemyPrefabs;
    public AnimationCurve SpawnanimationCurve;

    private int enemyCount;
    private Animator battleStateManager;

    private string selectedTargetName;
    private EnemyController selectedTarget;
    public GameObject selectionCircle;
    private bool canSelectEnemy;
    private bool attacking = false;

    public bool CanSelectEnemy { get { return canSelectEnemy; } }
    public int EnemyCount { get { return enemyCount; } }


    public enum BattleState
    {
        Begin_Battle,
        Intro,
        Player_Move,
        Player_Attack,
        Change_Control,
        Enemy_Attack,
        Battle_Result,
        Battle_End
    }

    private Dictionary<int, BattleState> battleStateHash = new Dictionary<int, BattleState>();
    private BattleState currentBattleState;

    public InventoryItem selectedWeapon;

    void Start()
    {
        battleStateManager = (Animator)GetComponent(typeof(Animator));
        // set the beginning battle phase
        GetAnimationState();

        MessagingManager.Instance.SubscribeInventoryEvent(InventoryItemSelect);

        // calculate how many enemies
        enemyCount = Random.Range(1, EnemySpawnPoints.Length);
        Debug.Log(enemyCount);

        // spawn enemies
        StartCoroutine(SpawnEnemies());

        GameState.CurrentPlayer.Health = 20;
    }

    private void InventoryItemSelect(InventoryItem item)
    {
        selectedWeapon = item;
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            var newEnemy = (GameObject)Instantiate(EnemyPrefabs[0]);
            newEnemy.transform.position = new Vector3(10, -1, 0);
            yield return StartCoroutine(MoveCharacterToPoint(EnemySpawnPoints[i], newEnemy));

            newEnemy.transform.parent = EnemySpawnPoints[i].transform;

            var controller = (EnemyController)newEnemy.GetComponent(typeof(EnemyController));
            controller.BattleManager = this;

            var EnemyProfile = ScriptableObject.CreateInstance<Enemy>();
            EnemyProfile.Class = EnemyClass.Goblin;
            EnemyProfile.Level = 1;
            EnemyProfile.Damage = 1;
            EnemyProfile.Health = 5;
            EnemyProfile.NoOfAttacks = 1;
            EnemyProfile.Name = EnemyProfile.Class + " " + i.ToString();

            controller.EnemyProfie = EnemyProfile;
            Enemies.Add(controller);
        }

        battleStateManager.SetBool("BattleReady", true);
    }

    void GetAnimationState()
    {
        foreach (BattleState state in (BattleState[])System.Enum.GetValues(typeof(BattleState)))
        {
            battleStateHash.Add(Animator.StringToHash("Base Layer." + state.ToString()), state);
        }
    }

    void Update()
    {
        currentBattleState = battleStateHash[battleStateManager.GetCurrentAnimatorStateInfo(0).nameHash];
        switch (currentBattleState)
        {
            case BattleState.Begin_Battle:
                MessagingManager.Instance.BroadcastUIEvent(true);
                break;
            case BattleState.Intro:
                MessagingManager.Instance.BroadcastUIEvent(true);
                break;
            case BattleState.Player_Move:
                MessagingManager.Instance.BroadcastUIEvent(false);
                attacking = false;
                EndBattle();
                break;
            case BattleState.Player_Attack:
                if (!attacking)
                {
                    StartCoroutine(AttackTarget());
                }
                break;
            case BattleState.Change_Control:
                attacking = false;
                MessagingManager.Instance.BroadcastUIEvent(true);
                battleStateManager.SetBool("EnemiesDone", false);
                break;
            case BattleState.Enemy_Attack:
                ClearSelectedEnemy();
                selectedWeapon = null;
                foreach (var gob in Enemies)
                {
                    if (gob.EnemyProfie.Health >= 1 && !attacking)
                    {
                        StartCoroutine(EnemyAttack());
                    }
                }
                break;
            case BattleState.Battle_Result:
                selectedWeapon = null;
                MessagingManager.Instance.BroadcastUIEvent(true);
                break;
            case BattleState.Battle_End:
                break;
            default: break;
        }
    }

    void OnGUI()
    {
        switch (currentBattleState)
        {
            case BattleState.Begin_Battle:
                break;
            case BattleState.Intro:
                GUI.Box(new Rect((Screen.width * 0.5f) - 150, 50, 300, 50), "Battle between Player and Goblins");
                break;
            case BattleState.Player_Move:
                GUI.Box(new Rect(20, 80, 200, 50), "Player Health: " + GameState.CurrentPlayer.Health);
                if (GUI.Button(new Rect(10, 10, 100, 50), "Run Away"))
                {
                    GameState.PlayerReturningHome = true;
                    NavigationManager.NavigateTo("World");
                }

                if (selectedWeapon == null)
                {
                    GUI.Box(new Rect((Screen.width * 0.5f) - 50, 10, 100, 50), "Select Weapon");
                }
                else if (selectedTarget == null)
                {

                    GUI.Box(new Rect((Screen.width * 0.5f) - 50, 10, 100, 50), "Select Target");
                    canSelectEnemy = true;
                }
                else
                {
                    if (GUI.Button(new Rect((Screen.width / 2) - 50, 10, 300, 50), "Attack " + selectedTargetName))
                    {
                        canSelectEnemy = false;
                        battleStateManager.SetBool("PlayerReady", true);
                        MessagingManager.Instance.BroadcastUIEvent(true); // freeze input
                    }
                }

                // make a nice grid and display their names screw their positions
                var posY = 160;
                var posX = ((Screen.width / 2) - 200);
                var offSetX = 140;
                for (int i = 0; i < Enemies.Count; i++)
                {
                    if (Enemies[i].EnemyProfie.Health > 0)
                    {
                        GUI.Box(new Rect(posX, posY, 130, 20),
                                     Enemies[i].EnemyProfie.Name + " health: " + Enemies[i].EnemyProfie.Health);

                        posX += offSetX;
                        if (posX + offSetX > Screen.width)
                        {
                            posX = ((Screen.width / 2) - 200);
                            posY += 25;
                        }
                    }
                }
                break;
            case BattleState.Player_Attack:
                GUI.Box(new Rect(20, 80, 200, 50), "Player Health: " + GameState.CurrentPlayer.Health);
                break;
            case BattleState.Change_Control:
                GUI.Box(new Rect(20, 80, 200, 50), "Player Health: " + GameState.CurrentPlayer.Health);
                GUI.Box(new Rect((Screen.width * 0.5f) - 50, 10, 300, 50), "Enemies turn to attack!");

                break;
            case BattleState.Enemy_Attack:
                GUI.Box(new Rect(20, 80, 200, 50), "Player Health: " + GameState.CurrentPlayer.Health);
                break;
            case BattleState.Battle_Result:
                GUI.Box(new Rect(20, 80, 200, 50), "Player Health: " + GameState.CurrentPlayer.Health);
                if (GameState.CurrentPlayer.Health > 1)
                {
                    GUI.Box(new Rect((Screen.width * 0.5f) - 150, (Screen.height * 0.5f) - 150, 300, 100), "Enemies killed " + enemyKillCount);
                }
                else
                {
                    GUI.Box(new Rect((Screen.width * 0.5f) - 150, (Screen.height * 0.5f) - 150, 300, 100), "You died! \n You took " + enemyKillCount + " enemies with you!");
                }
                break;
            case BattleState.Battle_End:
                GUI.Box(new Rect(20, 80, 200, 50), "Player Health: " + GameState.CurrentPlayer.Health);
                GUI.Box(new Rect((Screen.width * 0.5f) - 150, (Screen.height * 0.5f) - 150, 300, 50), "Battle Over, Change scene or something");
                break;
            default:
                break;
        }
    }

    public void SelectEnemy(EnemyController enemy, string name)
    {
        selectedTarget = enemy;
        selectedTargetName = name;
    }

    public void ClearSelectedEnemy()
    {
        if (selectedTarget != null)
        {
            var enemyController = (EnemyController)selectedTarget.GetComponent(typeof(EnemyController));
            enemyController.ClearSelection();
            selectedTarget = null;
            selectedTargetName = string.Empty;
        }
    }

    IEnumerator MoveCharacterToPoint(GameObject destination, GameObject character)
    {
        float timer = 0f;
        var StartPosition = character.transform.position;
        if (SpawnanimationCurve.length > 0)
        {
            while (timer < SpawnanimationCurve.keys[SpawnanimationCurve.length - 1].time)
            {
                character.transform.position = Vector3.Lerp(StartPosition, destination.transform.position, SpawnanimationCurve.Evaluate(timer));
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            character.transform.position = destination.transform.position;
        }
    }

    IEnumerator AttackTarget()
    {
        int Attacks = 0;
        attacking = true;
        bool attackComplete = false;
        while (!attackComplete)
        {
            //////////////////////////////////////// new section for abilities work on this
            var curStr = GameState.CurrentPlayer.Strength;
            GameState.CurrentPlayer.Strength += selectedWeapon.Strength;
            for (int i = 0; i < GameState.CurrentPlayer.Inventory.Count; i++)
            {
                if (selectedWeapon == GameState.CurrentPlayer.Inventory[i])
                {
                    if (GameState.CurrentPlayer.abilities[i] != null)
                        GameState.CurrentPlayer.abilities[i].UseAbility();
                    else
                    {
                        Debug.LogError("Ability not found");
                    }
                }
            }
            /////////////////////////////////////// new section end

            GameState.CurrentPlayer.Attack(selectedTarget.EnemyProfie);
            selectedTarget.UpdateAI();
            selectedTarget.HitEnemyAnim();
            Attacks++;

           // yield return new WaitForSeconds(0.2f);
            if (selectedTarget.EnemyProfie.Health < 1 || Attacks >= GameState.CurrentPlayer.NoOfAttacks)
            {
                attackComplete = true;
            }

            if (selectedTarget.EnemyProfie.Health < 1)
            {
                enemyKillCount++;
                attackComplete = true;
                Enemies.Remove(selectedTarget);
                EndBattle();
            }

            yield return new WaitForSeconds(1);
            //////////////////////////////////////// new section for abilities work on this
            Debug.Log("player attack dealt " + GameState.CurrentPlayer.Strength + " damage!");
            GameState.CurrentPlayer.Strength = curStr;
            /////////////////////////////////////// new section end
            battleStateManager.SetBool("PlayerReady", false);
        }
    }

    IEnumerator EnemyAttack()
    {
        int Attacks = 0;
        attacking = true;
        bool attackComplete = false;
        while (!attackComplete)
        {
            foreach (var gob in Enemies)
            {
                if (GameState.CurrentPlayer.Health > 0)
                {
                    gob.EnemyProfie.Attack(GameState.CurrentPlayer);
                    Attacks++;
                    Debug.Log(gob.EnemyProfie.Name + " attack!!");
                    yield return new WaitForSeconds(0.3f);
                    if (GameState.CurrentPlayer.Health < 1 || Attacks >= gob.EnemyProfie.NoOfAttacks)
                    {
                        attackComplete = true;
                    }
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    attackComplete = true;
                    EndBattle();
                    yield return new WaitForSeconds(0.5f);
                }
            }
            battleStateManager.SetBool("EnemiesDone", true);
            //for (int i = 0; i < Enemies.Count; i++)
            //{
            //    if (Enemies[i].EnemyProfie.Health >= 1)
            //    {
            //        Enemies[i].EnemyProfie.Attack(GameState.CurrentPlayer);
            //        Attacks++;
            //        Debug.Log(Enemies[i].EnemyProfie.Name + " attack");
            //        yield return new WaitForSeconds(0.5f);
            //        if (GameState.CurrentPlayer.Health <1 || Attacks >= Enemies[i].EnemyProfie.NoOfAttacks)
            //        {
            //            attackComplete = true;
            //        }
            //    }
               
            //    if (Enemies[i].EnemyProfie.Health < 1)
            //    {
            //        attackComplete = true;
            //    }
            //    yield return new WaitForSeconds(1);
            //}
            //battleStateManager.SetBool("EnemiesDone", true);
        }
    }

    void EndBattle()
    {
        if (GameState.CurrentPlayer.Health < 1 || enemyKillCount >= enemyCount)
        {
            battleStateManager.SetBool("BattleReady", false);
        }
    }

}
