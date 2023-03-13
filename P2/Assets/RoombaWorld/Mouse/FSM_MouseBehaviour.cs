using FSMs;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_MouseBehaviour", menuName = "Finite State Machines/FSM_MouseBehaviour", order = 1)]
public class FSM_MouseBehaviour : FiniteStateMachine {

    /** Blackboard */
    private MOUSE_Blackboard blackboard;

    /** Variables */
    private GoToTarget goToTarget;

    private GameObject destiny;
    private float elapsedTime;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        blackboard = GetComponent<MOUSE_Blackboard>();
        goToTarget = GetComponent<GoToTarget>();

        transform.position = blackboard.RandomExitPoint().transform.position;

        /** OnEnter */
        base.OnEnter();
    }

    /** OnExit */
    public override void OnExit() {

        // Destroy for the desetinationHandler
        if (destiny != null) { GameObject.Destroy(destiny); }

        /** DisableSteerings */
        base.DisableAllSteerings();
        /** OnExit */
        base.OnExit();
    }

    /** OnConstruction */
    public override void OnConstruction() {

        /** States */
        State reachDestination = new State("reachDestination",
            () => {
                if (destiny == null || destiny.Equals(null)) { destiny = new GameObject(); }
                // Set destination to a RandomWalkableLocation
                destiny.transform.position = RandomLocationGenerator.RandomWalkableLocation();
                goToTarget.target = destiny;
            }, () => { }, () => { });

        State poo = new State("poo",
            () => { elapsedTime = 0.0f; },
            () => { elapsedTime = elapsedTime + Time.deltaTime; },
            () => { blackboard.Crap(transform.position); });

        State findExit = new State("findExit",
            () => { goToTarget.target = blackboard.RandomExitPoint(); },
            () => { }, () => { });

        /** Transitions */
        Transition destinationReached = new Transition("destinationReached",
            () => { return goToTarget.routeTerminated(); },
            () => { });

        Transition poopDone = new Transition("poopDone",
            () => { return elapsedTime >= blackboard.crapTimeDuration; },
            () => { });

        Transition exitReached = new Transition("exitReached",
            () => { return goToTarget.routeTerminated(); },
            () => { GameObject.Destroy(gameObject); });


        /** FSM Set Up */
        AddStates(reachDestination, poo, findExit);
        /** ------------------------------------ */
        AddTransition(reachDestination, destinationReached, poo);
        /** -------------------------------------------------- */
        AddTransition(poo, poopDone, findExit);
        /** -------------------------------- */
        AddTransition(findExit, exitReached, reachDestination);
        /** ------------------------------------------------ */
        initialState = reachDestination;

    }
}
