using UnityEngine;
using System.Collections;

public class MapMovement : MonoBehaviour
{
    Vector3 StartLocation;
    Vector3 TargetLocation;
    float timer = 0;
    bool inputActive = true;
    bool inputReady = true;
    bool startedTravelling = false;

    // battle variables
    int EncounterChance = 50;
    float EncounterDistance = 0;

    public AnimationCurve MovementCurve;

    void Awake()
    {
        this.collider2D.enabled = false;
        var lastPosition = GameState.GetLastScenePosition(Application.loadedLevelName);
        if (lastPosition != Vector3.zero)
            transform.position = lastPosition;
    }

    void Start()
    {
        MessagingManager.Instance.UISubscribeEvent(UpdateInputAction);
    }

    private void UpdateInputAction(bool uiVisible)
    {
        inputReady = !uiVisible;
    }

    void MoveToLocation(Vector3 targetLocation)
    {
        StartLocation = transform.position.ToVector3_2D();
        timer = 0;
        TargetLocation = WorldExtensions.GetScreenPositionFor2D(targetLocation);
        startedTravelling = true;
    }

    void CalculateEnemyEncounterProbability()
    {
        // work out if a battle is going to happen and if it's likely
        // then set the distance the player will travel before it happens
        // only encounter if the player is not currently running from a battle
        var EncounterProbability = Random.Range(1, 100);
        if (EncounterProbability < EncounterChance)// && !GameState.PlayerReturningHome)
        {
            EncounterDistance = (Vector3.Distance(StartLocation, TargetLocation) / 100) * Random.Range(10, 100);
        }
        else
        {
            EncounterDistance = 0;
        }
    }

    void Update()
    {
        // move on mouse click
        if (inputActive && Input.GetMouseButtonUp(0))
        {
            MoveToLocation(Input.mousePosition);
            CalculateEnemyEncounterProbability();
        }// else move on touch tap
        else if (inputActive && Input.touchCount == 1)
        {
            MoveToLocation(Input.GetTouch(0).position);
            CalculateEnemyEncounterProbability();
        }

        // move to location as long as 
        // the target location is not 0
        // the target location is not our current location
        // and the target location is not the start location
        if (TargetLocation != Vector3.zero && TargetLocation != transform.position && TargetLocation != StartLocation)
        {
            transform.position = Vector3.Lerp(StartLocation, TargetLocation, MovementCurve.Evaluate(timer));
            timer += Time.deltaTime;
        }
        // re-enable our collider when we started moving more than 0.5f meters
        if (startedTravelling && Vector3.Distance(StartLocation, transform.position.ToVector3_2D()) > 0.5f)
        {
            this.collider2D.enabled = true;
            startedTravelling = false;
        }

        if (EncounterDistance > 0)
        {
            if (Vector3.Distance(StartLocation, transform.position) > EncounterDistance)
            {
                TargetLocation = Vector3.zero;
                NavigationManager.NavigateTo("Battle");
            }
        }

        if (!inputReady && inputActive)
        {
            TargetLocation = this.transform.position;
            Debug.Log("Stopping Player");
        }

        inputActive = inputReady;
    }

    void OnDestroy()
    {
        if (MessagingManager.Instance != null)
            MessagingManager.Instance.UnSubscribeUIEvent(UpdateInputAction);
        GameState.SetLastScenePosition(Application.loadedLevelName, transform.position);
    }
}
