using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_SeedCollecting", menuName = "Finite State Machines/FSM_SeedCollecting", order = 1)]
public class FSM_SeedCollecting : FiniteStateMachine {

    private ANT_Blackboard blackboard;
    private Arrive arrive;
    private GameObject theSeed;


    public override void OnEnter() {
        blackboard = GetComponent<ANT_Blackboard>();
        arrive = GetComponent<Arrive>();
        base.OnEnter(); 
    }

    public override void OnExit() {
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction() {

        FiniteStateMachine twoPointWandering = ScriptableObject.CreateInstance<FSM_TwoPointWandering>();

        State goingToSeed = new State("goingToSeed",
            () => {
                arrive.target = theSeed;
                arrive.enabled = true;
            },
            () => { },
            () => { arrive.enabled = false; }
        );

        State transportingSeedToNest = new State("transportingSeedToNest",
            () => {
                theSeed.transform.parent = transform;
                arrive.target = blackboard.theNest;
                theSeed.tag = "NO_SEED";
                arrive.enabled = true;
            },
            () => { },
            () => {
                arrive.enabled = false;
                theSeed.transform.parent = null;
            }
        );

        Transition nearbySeedDetected = new Transition("nearbySeedDetected",
            () => {
                theSeed = SensingUtils.FindInstanceWithinRadius(gameObject, "SEED", blackboard.seedDetectionRadius);
                return theSeed != null;
            }, 
            () => { } 
        );

        Transition seedReached = new Transition("seedReached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, theSeed) < blackboard.seedReachedRadius;
            },
            () => { }
        );

        Transition nestReached = new Transition("nearbySeedDetected",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.theNest) < blackboard.nestReachedRadius;
            },
            () => { }
        );

        Transition seedPicked = new Transition("seedPicked",
            () => {
                return theSeed.tag != "SEED";
            },
            () => { }
        );

        AddStates(twoPointWandering, goingToSeed, transportingSeedToNest);

        AddTransition(twoPointWandering, nearbySeedDetected, goingToSeed);
        AddTransition(goingToSeed, seedPicked, twoPointWandering);
        AddTransition(goingToSeed, seedReached, transportingSeedToNest);
        AddTransition(transportingSeedToNest, nestReached, twoPointWandering);

        initialState = twoPointWandering;
    }
}
