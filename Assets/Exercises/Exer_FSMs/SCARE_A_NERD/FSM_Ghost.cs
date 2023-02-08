using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_Ghost", menuName = "Finite State Machines/FSM_Ghost", order = 1)]
public class FSM_Ghost : FiniteStateMachine {

    private GHOST_Blackboard blackboard;
    private Pursue pursue;
    private Arrive arrive;
    private SteeringContext context;
    private GameObject victim;
    private float elapsedTime;

    public override void OnEnter() {
        blackboard = GetComponent<GHOST_Blackboard>();
        arrive = GetComponent<Arrive>();
        pursue = GetComponent<Pursue>();
        context = GetComponent<SteeringContext>();
        base.OnEnter();
    }

    public override void OnExit() {
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction() {
        State goCastle = new State("goCastle",
            () => {
                context.maxSpeed *= 4.0f;
                arrive.target = blackboard.castle;
                arrive.enabled = true; }, 
            () => { }, 
            () => { 
                context.maxSpeed /= 4.0f;
                arrive.enabled = false; }
        );
        State hide = new State("hide",
            () => { elapsedTime = 0.0f; }, 
            () => { elapsedTime += Time.deltaTime; },  
            () => { }
        );
        State selectTarget = new State("selectTarget",
            () => { }, () => { }, () => { }
        );
        State approach = new State("approach",
            () => {
                pursue.target = victim; 
                pursue.enabled = true; }, 
            () => { }, 
            () => { }
        );
        State cryBoo = new State("cryBOO",
            () => { elapsedTime = 0.0f; blackboard.CryBoo(true); },
            () => { elapsedTime += Time.deltaTime; },
            () => { blackboard.CryBoo(false); pursue.enabled = false; }
        );


        Transition castleReached = new Transition("castleReached",
            () => {
                return (SensingUtils.DistanceToTarget(gameObject, blackboard.castle) < blackboard.castleReachedRadius);
            },
            () => { }
        );
        Transition hideTime = new Transition("hideTime",
            () => {
                return elapsedTime >= blackboard.hideTime;
            }, () => { }
        );
        Transition targetSelected = new Transition("targetSelected",
            () => {
                victim = SensingUtils.FindRandomInstanceWithinRadius(gameObject, blackboard.victimLabel, blackboard.nerdDetectionRadius);
                return victim != null;
            }, () => { }
        );
        Transition targetIsClose = new Transition("targetIsClose",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, victim) < blackboard.closeEnoughToScare;
            }, () => { }
        );
        Transition booTime = new Transition("booTime",
            () => {
                return elapsedTime >= blackboard.booDuration;
            }, () => { }
        );


        AddStates(goCastle, hide, selectTarget, approach, cryBoo);

        AddTransition(goCastle, castleReached, hide);
        AddTransition(hide, hideTime, selectTarget);
        AddTransition(selectTarget, targetSelected, approach);
        AddTransition(approach, targetIsClose, cryBoo);
        AddTransition(cryBoo, booTime, goCastle);

        initialState = goCastle;
    }
}
