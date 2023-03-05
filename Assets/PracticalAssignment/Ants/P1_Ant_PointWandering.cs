using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_Ant_PointWandering", menuName = "Finite State Machines/P1_Ant_PointWandering", order = 1)]
public class P1_Ant_PointWandering : FiniteStateMachine {

    /** Variables */
    private FlockingAroundPlusAvoidance flockingAround;

    private int currentDestiny = 0;
    private GameObject[] wanderPoints;

    /** SteeringContext */
    private SteeringContext context;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        context = GetComponent<SteeringContext>();
        flockingAround = GetComponent<FlockingAroundPlusAvoidance>();

        /** Finder */
        wanderPoints = GameObject.FindGameObjectsWithTag("ANT_ATTRACTOR_POINT");

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

    public override void OnConstruction() {

        /** States */
        State findAttractors = new State("findAttractors", () => { }, () => { }, () => { });

        State reachAttractor = new State("reachAttractor",
           () => {
               flockingAround.enabled = true;
               flockingAround.attractor = wanderPoints[currentDestiny];
           },
           () => { }, () => { });

        /** Transitions */
        Transition attractorsFind = new Transition("attractorsFind",
            () => {
                if (wanderPoints.Length == 0) {
                    wanderPoints = GameObject.FindGameObjectsWithTag("ANT_ATTRACTOR_POINT");
                }
                return wanderPoints.Length != 0;
            }, () => { });

        Transition locationReached = new Transition("locationReached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, flockingAround.attractor) < context.wanderRadius / 2.0f;
            }, () => {
                currentDestiny++;
                if (currentDestiny >= wanderPoints.Length) {
                    currentDestiny = 0;
                }
            });


        AddStates(findAttractors, reachAttractor);
        /** ----------------------------------- */
        AddTransition(findAttractors, attractorsFind, reachAttractor);
        /** ------------------------------------------------------- */
        AddTransition(reachAttractor, locationReached, reachAttractor);
        /** -------------------------------------------------------- */
        initialState = findAttractors;
    }

}