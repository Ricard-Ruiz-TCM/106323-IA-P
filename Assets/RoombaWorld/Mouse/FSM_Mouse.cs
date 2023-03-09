using FSMs;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_Mouse", menuName = "Finite State Machines/FSM_Mouse", order = 1)]
public class FSM_Mouse : FiniteStateMachine {

    /** Blackboard */
    private MOUSE_Blackboard blackboard;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        blackboard = GetComponent<MOUSE_Blackboard>();

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
        FiniteStateMachine mouseBehaviour = ScriptableObject.CreateInstance<FSM_MouseBehaviour>();
        /** ----------------------------------------------------------------------------------- */
        mouseBehaviour.Name = "mouseBehaviour";
        /** -------------------------------- */

        /** States */
        State findNearestExit = new State("findNearestExit",
            () => { },
            () => { },
            () => { }
        );

        State reachExit = new State("reachExit",
            () => { },
            () => { },
            () => { }
        );

        /** Transitions */
        Transition roombaDetected = new Transition("roombaDetected",
            () => { return true; },
            () => { }
        );

        Transition nearestExitFound = new Transition("nearestExitFound",
            () => { return true; },
            () => { }
        );

        Transition exitReached = new Transition("exitReached",
            () => { return true; },
            () => { }
        );


        /** FSM Set Up */
        AddStates(mouseBehaviour, findNearestExit, reachExit);
        /** ---------------------------------------------------------------- */
        AddTransition(mouseBehaviour, roombaDetected, findNearestExit);
        /** -------------------------------------------------------- */
        AddTransition(findNearestExit, nearestExitFound, reachExit);
        /** ----------------------------------------------------- */
        AddTransition(reachExit, exitReached, mouseBehaviour);
        /** ----------------------------------------------- */
        initialState = mouseBehaviour;

    }
}
