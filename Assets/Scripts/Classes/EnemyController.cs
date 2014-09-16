using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private BattleManager battleManager;
    public Enemy EnemyProfie;
    private Animator enemyAI;

    public BattleManager BattleManager { get { return battleManager; } set { battleManager = value; } }

    private bool selected;
    GameObject selectionCircle;

    private ParticleSystem bloodsplatterParticles;

    void Awake()
    {
        enemyAI = (Animator)GetComponent(typeof(Animator));
        if (enemyAI == null)
        {
            Debug.LogError("No AI System Found");
        }

        bloodsplatterParticles = (ParticleSystem)GetComponentInChildren(typeof(ParticleSystem));
        if (bloodsplatterParticles == null)
        {
            Debug.LogError("No Particle System Found");
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateAI();
    }

    public void UpdateAI()
    {
        if (enemyAI != null && EnemyProfie != null)
        {
            enemyAI.SetInteger("EnemyHealth", EnemyProfie.Health);
            enemyAI.SetInteger("PlayerHealth", GameState.CurrentPlayer.Health);
            enemyAI.SetInteger("EnemiesInBattle", battleManager.EnemyCount);
        }
    }

    void ShowBloodSplatter()
    {
        bloodsplatterParticles.Play();
        ClearSelection();
        if (battleManager != null)
        {
            battleManager.ClearSelectedEnemy();
        }
        else
        {
            Debug.LogError("No BattleManager");
        }
    }

    IEnumerator SpinObject(GameObject target)
    {
        while (true)
        {
            target.transform.Rotate(0, 0, 180 * Time.deltaTime);
            yield return null;
        }
    }

    void OnMouseDown()
    {
        if (battleManager.CanSelectEnemy)
        {
            var selection = !selected;
            battleManager.ClearSelectedEnemy();
            selected = selection;

            if (selected)
            {
                selectionCircle = (GameObject)GameObject.Instantiate(battleManager.selectionCircle);
                selectionCircle.transform.parent = transform;

                selectionCircle.transform.localPosition = Vector3.zero;
                StartCoroutine("SpinObject", selectionCircle);
                battleManager.SelectEnemy(this, EnemyProfie.Name);
            }
        }
    }

    public void ClearSelection()
    {
        if (selected)
        {
            selected = false;
            if (selectionCircle != null)
            {
                DestroyObject(selectionCircle);
                StopCoroutine("SpinObject");
            }
        }
    }
}
