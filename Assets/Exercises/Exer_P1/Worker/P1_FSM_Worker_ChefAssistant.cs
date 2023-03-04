using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_FSM_Worker_ChefAssistant", menuName = "Finite State Machines/P1_FSM_Worker_ChefAssistant", order = 1)]
public class P1_FSM_Worker_ChefAssistant : FiniteStateMachine {

    /** Public Variables */
    public float washUpTime = 10.0f;

    /** Variables */
    private Arrive arrive;
    private float elapsedTime;

    private GameObject theDish;
    private GameObject theSink;
    private GameObject thePile;
    /** SteeringContext */
    private SteeringContext context;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        arrive = GetComponent<Arrive>();
        context = GetComponent<SteeringContext>();

        /** Finder */
        theSink = GameObject.FindGameObjectWithTag("SINK");
        thePile = GameObject.FindGameObjectWithTag("CLEAN_PILE");

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
        State findDirtyPlate = new State("findDirtyPlate", () => { }, () => { }, () => { });

        State reachDirtyPlate = new State("reachDirtyPlate",
            () => {
                arrive.enabled = true;
                arrive.target = theDish;
            },
            () => { },
            () => { theDish.GetComponent<P1_DishController>().Pick(transform); });

        State reachTheSink = new State("reachTheSink",
            () => { arrive.target = theSink; },
            () => { },
            () => { });

        State washUpPlate = new State("washUpPlate",
            () => { elapsedTime = 0.0f; },
            () => { elapsedTime += Time.deltaTime; },
            () => { });

        State storePlate = new State("storePlate",
            () => { arrive.target = thePile; },
            () => { },
            () => {
                theDish.GetComponent<P1_DishController>().Wash();
                theDish.GetComponent<P1_DishController>().PlaceOn(thePile);
                arrive.enabled = false;
            });

        /** Transitions */
        Transition dirtyPlateDetected = new Transition("dirtyDishPicked",
            () => {
                theDish = GameObject.FindGameObjectWithTag("DISH_DIRTY");
                return theDish != null;
            }, () => { });

        Transition sinkReached = new Transition("targetReached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, theSink) < context.closeEnoughRadius;
            }, () => { });

        Transition dishReached = new Transition("targetReached",
            () => { return SensingUtils.DistanceToTarget(gameObject, theDish) < context.closeEnoughRadius; }, () => { });

        Transition pileReached = new Transition("targetReached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, thePile) < context.closeEnoughRadius;
            }, () => { });

        Transition washedUpDish = new Transition("washedUpDish",
            () => {
                return elapsedTime >= washUpTime;
            }, () => { });


        /** FSM Set Up */
        AddStates(findDirtyPlate, reachDirtyPlate, reachTheSink, washUpPlate, storePlate);
        /** ---------------------------------------------------------------------------- */
        AddTransition(findDirtyPlate, dirtyPlateDetected, reachDirtyPlate);
        AddTransition(reachDirtyPlate, dishReached, reachTheSink);
        AddTransition(reachTheSink, sinkReached, washUpPlate);
        AddTransition(washUpPlate, washedUpDish, storePlate);
        AddTransition(storePlate, pileReached, findDirtyPlate);
        /** ------------------------------------------------ */
        initialState = findDirtyPlate;

    }
}