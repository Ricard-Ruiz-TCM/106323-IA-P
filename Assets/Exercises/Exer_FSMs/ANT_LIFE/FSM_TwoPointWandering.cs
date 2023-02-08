using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_TwoPointWandering", menuName = "Finite State Machines/FSM_TwoPointWandering", order = 1)]
public class FSM_TwoPointWandering : FiniteStateMachine {

    private WanderAround wanderAround;
    private SteeringContext steeringContext;
    private ANT_Blackboard blackboard;

    private float elapsedTime = 0;


    public override void OnEnter() {
        wanderAround = GetComponent<WanderAround>();
        steeringContext = GetComponent<SteeringContext>();
        blackboard = GetComponent<ANT_Blackboard>();
        base.OnEnter();
    }

    public override void OnExit() {
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction() {

        State goingA = new State("goingA",
           () => {
               elapsedTime = 0f;
               wanderAround.attractor = blackboard.locationA;
               wanderAround.enabled = true;
           },
           () => { elapsedTime += Time.deltaTime; },
           () => { wanderAround.enabled = false; }
       );

        State goingB = new State("goingB",
           () => {
               elapsedTime = 0f;
               wanderAround.attractor = blackboard.locationB;
               wanderAround.enabled = true;
           },
           () => { elapsedTime += Time.deltaTime; },
           () => { wanderAround.enabled = false; }
       );

        Transition locationAReached = new Transition("locationAReached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.locationA) < blackboard.locationReachedRadius;
            },
            () => { steeringContext.seekWeight = blackboard.initialSeekWeight; }
        );

        Transition locationBReached = new Transition("locationBReached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.locationB) < blackboard.locationReachedRadius;
            },
            () => { steeringContext.seekWeight = blackboard.initialSeekWeight; }
        );

        Transition timeOut = new Transition("timeOut",
            () => {
                return elapsedTime >= blackboard.intervalBetweenTimeouts;
            },
            () => {
                float sk = Mathf.Min(1, steeringContext.seekWeight + blackboard.seekIncrement);
                steeringContext.seekWeight = sk;
                elapsedTime = 0.0f;
            }
        );

        AddStates(goingA, goingB);

        AddTransition(goingA, locationAReached, goingB);
        AddTransition(goingB, locationBReached, goingA);
        AddTransition(goingA, timeOut, goingA);
        AddTransition(goingB, timeOut, goingB);

        initialState = goingA;
    }
}
