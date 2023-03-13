using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "FMS_RoombBehaviour", menuName = "Finite State Machines/FMS_RoombBehaviour", order = 1)]
public class FMS_RoombBehaviour : FiniteStateMachine {

    /** Blackboard */
    private ROOMBA_Blackboard blackboard;

    /** Variables */
    private GoToTarget goToTarget;

    private GameObject thePoo;
    private GameObject theDust;

    /** SteeringContext */
    private SteeringContext context;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        blackboard = GetComponent<ROOMBA_Blackboard>();
        goToTarget = GetComponent<GoToTarget>();

        context = GetComponent<SteeringContext>();

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
        State patrolling = new State("patrolling",
            () => { goToTarget.target = SensingUtils.FindRandomInstanceWithinRadius(gameObject, "PATROLPOINT", blackboard.patrolPointDetectionRadius); },
            () => { }, () => { });

        State reachDust = new State("reachDust",
            () => { goToTarget.target = theDust; },
            () => { }, () => { });

        State reachPoo = new State("reachPoo",
            () => { goToTarget.target = thePoo; },
            () => {
                theDust = SensingUtils.FindInstanceWithinRadius(gameObject, "DUST", blackboard.dustDetectionRadius);
                // Adding Dust on Memory if necessary
                if (theDust != null) { blackboard.AddToMemory(theDust); theDust = null; }
            }, () => { });

        /** Transitions */
        Transition patrolPointReached = new Transition("patrolPointReached",
            () => { return goToTarget.routeTerminated(); },
            () => { });

        Transition pooDetected = new Transition("pooDetected",
            () => {
                thePoo = SensingUtils.FindInstanceWithinRadius(gameObject, "POO", blackboard.pooDetectionRadius);
                return thePoo != null;
            }, () => {
                context.maxSpeed *= 1.3f;
                context.maxAcceleration *= 2.6f;
                // Adding Dust on Memory if necessary
                if (theDust != null) { blackboard.AddToMemory(theDust); theDust = null; }
            });

        Transition closerPooDetected = new Transition("closerPooDetected",
            () => {
                GameObject poo = SensingUtils.FindInstanceWithinRadius(gameObject, "POO", blackboard.pooDetectionRadius);
                if ((thePoo != poo) && (poo != null)) {
                    thePoo = poo;
                    return true;
                }
                return false;
            }, () => { });

        Transition pooReached = new Transition("pooReached",
            () => { return SensingUtils.DistanceToTarget(gameObject, thePoo) <= blackboard.pooReachedRadius; },
            () => {
                context.maxSpeed /= 1.3f;
                context.maxAcceleration /= 2.6f;
                // Destroying thePoo when reached
                GameObject.Destroy(thePoo);
            });

        Transition dustOnMemory = new Transition("dustOnMemory",
            () => { return blackboard.somethingInMemory(); },
            () => { theDust = blackboard.RetrieveNearestFromMemory(gameObject); });

        Transition dustDetected = new Transition("dustDetected",
            () => {
                theDust = SensingUtils.FindInstanceWithinRadius(gameObject, "DUST", blackboard.dustDetectionRadius);
                return theDust != null;
            }, () => { });

        Transition closerDustDetected = new Transition("closerDustDetected",
            () => {
                GameObject dust = SensingUtils.FindInstanceWithinRadius(gameObject, "DUST", blackboard.dustDetectionRadius);
                if ((theDust != dust) && (dust != null)) {
                    float findedDDistance = SensingUtils.DistanceToTarget(gameObject, dust);
                    float DDistnace = SensingUtils.DistanceToTarget(gameObject, theDust);
                    if (findedDDistance < DDistnace) {
                        blackboard.AddToMemory(theDust);
                        theDust = dust;
                        return true;
                    }
                }
                return false;
            }, () => { });

        Transition dustReached = new Transition("dustReached",
            () => { return SensingUtils.DistanceToTarget(gameObject, theDust) <= blackboard.dustReachedRadius; },
            // Dstroying theDust when reached
            () => { GameObject.Destroy(theDust); });

        /** FSM Set Up */
        AddStates(patrolling, reachPoo, reachDust);
        /** ------------------------------------ */
        AddTransition(patrolling, pooDetected, reachPoo);
        AddTransition(patrolling, dustDetected, reachDust);
        AddTransition(patrolling, dustOnMemory, reachDust);
        AddTransition(patrolling, patrolPointReached, patrolling);
        /** --------------------------------------------------- */
        AddTransition(reachPoo, closerPooDetected, reachPoo);
        AddTransition(reachPoo, pooReached, patrolling);
        /** ----------------------------------------- */
        AddTransition(reachDust, pooDetected, reachPoo);
        AddTransition(reachDust, closerDustDetected, reachDust);
        AddTransition(reachDust, dustReached, patrolling);
        /** ------------------------------------------- */
        initialState = patrolling;

    }
}
