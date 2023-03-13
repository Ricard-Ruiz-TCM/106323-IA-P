using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "FMS_Roomba", menuName = "Finite State Machines/FMS_Roomba", order = 1)]
public class FMS_Roomba : FiniteStateMachine {

    /** Blackboard */
    private ROOMBA_Blackboard blackboard;

    /** Variables */
    private GoToTarget goToTarget;
    private PathFollowing pathFollowing;

    private float maxSpeed;
    private float maxAcceleration;
    private GameObject theChargePoint;
    private float baseWayPointReachedRadius;

    /** SteeringContext */
    private SteeringContext context;



    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        blackboard = GetComponent<ROOMBA_Blackboard>();

        goToTarget = GetComponent<GoToTarget>();
        pathFollowing = GetComponent<PathFollowing>();

        context = GetComponent<SteeringContext>();

        baseWayPointReachedRadius = pathFollowing.wayPointReachedRadius;
        maxSpeed = context.maxSpeed;
        maxAcceleration = context.maxAcceleration;

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
        State findNearestChargeSpot = new State("findNearestChargeSpot", () => { }, () => { }, () => { });

        State reachChargeSpot = new State("reachChargeSpot",
            () => { goToTarget.target = theChargePoint; },
            () => { }, () => { });

        State charge = new State("charge",
            () => { },
            () => { blackboard.Recharge(Time.deltaTime); },
            () => { });

        /** Transitions */
        Transition lowBatteryDetected = new Transition("lowBatteryDetected",
            () => { return blackboard.currentCharge <= blackboard.minCharge; },
            () => { 
                // Set the base speeds and reached radius variables
                context.maxSpeed = maxSpeed;
                context.maxAcceleration = maxAcceleration; 
                pathFollowing.wayPointReachedRadius = blackboard.chargingStationReachedRadius;
            });

        Transition chargeSpotFound = new Transition("chargeSpotFound",
            () => {
                theChargePoint = SensingUtils.FindInstanceWithinRadius(gameObject, "ENERGY", blackboard.chargingStationDetectionRadius);
                return theChargePoint != null;
            }, () => { });

        Transition chargeSpotReached = new Transition("chargeSpotReached",
            () => { return goToTarget.routeTerminated(); },
            () => { });

        Transition chargeCompleted = new Transition("chargeCompleted",
            () => { return blackboard.currentCharge >= blackboard.maxCharge; },
            () => { pathFollowing.wayPointReachedRadius = baseWayPointReachedRadius; });

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
