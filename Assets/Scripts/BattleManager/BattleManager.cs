using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
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

    private InventoryItem selectedWeapon;

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
    }

    private void InventoryItemSelect(InventoryItem item)
    {
        selectedWeapon = item;
    }

    void GetAnimationState()
    {
        foreach(BattleState state in (BattleState[]) System.Enum.GetValues(typeof(BattleState)))
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
                break;
            case BattleState.Intro:
                break;
            case BattleState.Player_Move:
                //battleStateManager.SetBool("PlayerReady", true);
                break;
            case BattleState.Player_Attack:
                //battleStateManager.SetBool("PlayerReady", false);
                if (!attacking)
                {
                    StartCoroutine(AttackTarget());
                }
                break;
            case BattleState.Change_Control:
                break;
            case BattleState.Enemy_Attack:
                break;
            case BattleState.Battle_Result:
                break;
            case BattleState.Battle_End:
                break;
            default: break;
        }
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
            EnemyProfile.Health = 2;
            EnemyProfile.name = EnemyProfile.Class + " " + i.ToString();

            controller.EnemyProfie = EnemyProfile;
        }

        battleStateManager.SetBool("BattleReady", true);
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
            GameState.CurrentPlayer.Attack(selectedTarget.EnemyProfie);
            selectedTarget.UpdateAI();
            Attacks++;
            if (selectedTarget.EnemyProfie.Health < 1 || Attacks > GameState.CurrentPlayer.NoOfAttacks)
            {
                attackComplete = true;
            }
            yield return new WaitForSeconds(1);
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
                    if (GUI.Button(new Rect((Screen.width / 2) - 50, 10, 100, 50), "Attack " + selectedTargetName))
                    {
                        canSelectEnemy = false;
                        battleStateManager.SetBool("PlayerReady", true);
                        MessagingManager.Instance.BroadcastUIEvent(true);
                    }
                }

                break;
            case BattleState.Player_Attack:
                break;
            case BattleState.Change_Control:
                break;
            case BattleState.Enemy_Attack:
                break;
            case BattleState.Battle_Result:
                break;
            case BattleState.Battle_End:
                break;
            default:
                break;
        }
    }
}
