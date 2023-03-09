using FSMs;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_MouseBehaviour", menuName = "Finite State Machines/FSM_MouseBehaviour", order = 1)]
public class FSM_MouseBehaviour : FiniteStateMachine {

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

        /** States */
        State findDestination = new State("findDestination",
            () => { },
            () => { },
            () => { }
        );

        State reachDestination = new State("reachDestination",
            () => { },
            () => { },
            () => { }
        );

        State poo = new State("poo",
            () => { },
            () => { },
            () => { }
        );

        State findExit = new State("findExit",
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
        Transition destinationFound = new Transition("destinationFound",
            () => { return true; },
            () => { }
        );

        Transition destinationReached = new Transition("destinationReached",
            () => { return true; },
            () => { }
        );

        Transition poopDone = new Transition("poopDone",
            () => { return true; },
            () => { }
        );

        Transition exitFound = new Transition("exitFound",
            () => { return true; },
            () => { }
        );

        Transition exitReached = new Transition("exitReached",
            () => { return true; },
            () => { }
        );


        /** FSM Set Up */
        AddStates(findDestination, reachDestination, poo, findExit, reachExit);
        /** ---------------------------------------------------------------- */
        AddTransition(findDestination, destinationFound, reachDestination);
        /** ------------------------------------------------------------ */
        AddTransition(reachDestination, destinationReached, poo);
        /** -------------------------------------------------- */
        AddTransition(poo, poopDone, findExit);
        /** -------------------------------- */
        AddTransition(findExit, exitFound, reachExit);
        /** --------------------------------------- */
        AddTransition(reachExit, exitReached, findDestination);
        /** ------------------------------------------------ */
        initialState = findDestination;

    }
}
