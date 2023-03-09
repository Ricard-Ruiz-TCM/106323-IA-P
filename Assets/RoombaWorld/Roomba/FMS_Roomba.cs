using FSMs;
using UnityEngine;

[CreateAssetMenu(fileName = "FMS_Roomba", menuName = "Finite State Machines/FMS_Roomba", order = 1)]
public class FMS_Roomba : FiniteStateMachine {

    /** Blackboard */
    private ROOMBA_Blackboard blackboard;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        blackboard = GetComponent<ROOMBA_Blackboard>();

        /** OnEnter */
        base.OnEnter();
    }

    /** OnExit */
    public override void OnExit() {
        /** DisableSteerings */
        base.DisableAllSteerings();
        /** OnExit */
        base.OnExit();
    }

    /** OnConstruction */
    public override void OnConstruction() {

        /** FSM's */
        FiniteStateMachine roombaBehaviour = ScriptableObject.CreateInstance<FMS_RoombBehaviour>();
        /** ------------------------------------------------------------------------------------ */
        roombaBehaviour.Name = "roombaBehaviour";
        /** ---------------------------------- */

        /** States */
        State findNearestChargeSpot = new State("findNearestChargeSpot",
            () => { },
            () => { },
            () => { }
        );

        State reachChargeSpot = new State("reachChargeSpot",
            () => { },
            () => { },
            () => { }
        );

        State charge = new State("charge",
            () => { },
            () => { },
            () => { }
        );

        /** Transitions */
        Transition lowBatteryDetected = new Transition("lowBatteryDetected",
            () => { return false; },
            () => { }
        );

        Transition chargeSpotFound = new Transition("chargeSpotFound",
            () => { return false; },
            () => { }
        );

        Transition chargeSpotReached = new Transition("chargeSpotReached",
            () => { return false; },
            () => { }
        );

        Transition chargeCompleted = new Transition("chargeCompleted",
            () => { return false; },
            () => { }
        );


        /** FSM Set Up */
        AddStates(roombaBehaviour, findNearestChargeSpot, reachChargeSpot, charge);
        /** -------------------------------------------------------------------- */
        AddTransition(roombaBehaviour, lowBatteryDetected, findNearestChargeSpot);
        /** ------------------------------------------------------------------- */
        AddTransition(findNearestChargeSpot, chargeSpotFound, reachChargeSpot);
        /** ---------------------------------------------------------------- */
        AddTransition(reachChargeSpot, chargeSpotReached, charge);
        /** --------------------------------------------------- */
        AddTransition(charge, chargeCompleted, roombaBehaviour);
        /** ------------------------------------------------- */
        initialState = roombaBehaviour;

    }
}
