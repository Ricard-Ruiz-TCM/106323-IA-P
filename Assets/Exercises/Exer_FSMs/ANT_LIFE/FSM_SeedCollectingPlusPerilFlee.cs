using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_SeedCollectingPLUSPerilFlee", menuName = "Finite State Machines/FSM_SeedCollectingPLUSPerilFlee", order = 1)]
public class FSM_SeedCollectingPlusPerilFlee : FiniteStateMachine {

    private ANT_Blackboard blackboard;
    private Flee flee;

    public override void OnEnter() {
        blackboard = GetComponent<ANT_Blackboard>();
        flee = GetComponent<Flee>();
        base.OnEnter();
    }

    public override void OnExit() {
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction() {

        FiniteStateMachine seedCollecting = ScriptableObject.CreateInstance<FSM_SeedCollecting>();

        State feelingFromPeril = new State("feelingFromPeril",
            () => { flee.enabled = true; },
            () => { },
            () => { flee.enabled = false; }
        );

        Transition predatorNearby = new Transition("predatorNearby",
            () => {
                return false;
            },
            () => { }
        );

        Transition predatorFarAway = new Transition("predatorFarAway",
            () => {
                return false;
            },
            () => { }
        );


        AddStates(seedCollecting, feelingFromPeril);

        AddTransition(seedCollecting, predatorNearby, feelingFromPeril);
        AddTransition(feelingFromPeril, predatorFarAway, seedCollecting);

        initialState = seedCollecting;

    }
}
