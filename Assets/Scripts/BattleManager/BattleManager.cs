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
                break;
            case BattleState.Change_Control:
                break;
            case BattleState.Enemy_Attack:
                for (int i = 0; i < enemyCount-1; i++)
                {
                    Debug.Log(EnemyPrefabs[i].name + " " + i + " attacks"); 
                }

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
        }

        battleStateManager.SetBool("BattleReady", true);
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
                break;
            case BattleState.Player_Attack:
                if (selectedWeapon == null)
                {
                    GUI.Box(new Rect((Screen.width * 0.5f) - 50, 10, 100, 50), "Select Weapon");
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
            default:
                break;
        }
    }
}
