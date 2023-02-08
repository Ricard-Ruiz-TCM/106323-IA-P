using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_DriveChicksAway", menuName = "Finite State Machines/FSM_DriveChicksAway", order = 1)]
public class FSM_DriveChicksAway : FiniteStateMachine {

    private Seek seek;
    private HEN_Blackboard blackboard;
    private AudioSource audioSource;
    private SteeringContext steeringContext;
    private GameObject theChick;

    public override void OnEnter() {
        seek = GetComponent<Seek>();
        steeringContext = GetComponent<SteeringContext>();
        blackboard = GetComponent<HEN_Blackboard>();
        audioSource = GetComponent<AudioSource>(); ;
        base.OnEnter();
    }

    public override void OnExit() {
        audioSource.Stop();
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction() {

        FiniteStateMachine searchWorms = ScriptableObject.CreateInstance<FSM_SearchWorms>();

        State driveAwayChick = new State("driveAwayChick",
            () => {
                audioSource.clip = blackboard.angrySound;
                audioSource.Play();
                transform.localScale *= 1.4f;
                steeringContext.maxSpeed *= 2.0f;
                steeringContext.maxAcceleration *= 2.0f;
                seek.target = theChick;
                seek.enabled = true;
            },
            () => { },
            () => {
                audioSource.Stop();
                seek.enabled = false;
                transform.localScale /= 1.4f;
                steeringContext.maxSpeed /= 2.0f;
                steeringContext.maxAcceleration /= 2.0f;
            }
        );

        Transition chickTooClose = new Transition("chickTooClose",
            () => {
                theChick = SensingUtils.FindInstanceWithinRadius(gameObject, "CHICK", blackboard.chickDetectionRadius);
                return theChick != null;
            },
            () => { }
        );

        Transition chickFarEnough = new Transition("chickFarEnough",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, theChick) >= blackboard.chickFarEnoughRadius;
            },
            () => { }
        );

        AddStates(searchWorms, driveAwayChick);

        AddTransition(searchWorms, chickTooClose, driveAwayChick);
        AddTransition(driveAwayChick, chickFarEnough, searchWorms);

        initialState = searchWorms;
    }
}
