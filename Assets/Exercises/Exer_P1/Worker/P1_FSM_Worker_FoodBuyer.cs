using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_FSM_Worker_FoodBuyer", menuName = "Finite State Machines/P1_FSM_Worker_FoodBuyer", order = 1)]
public class P1_FSM_Worker_FoodBuyer : FiniteStateMachine {

    /** Blackboard */
    private P1_Worker_Blackboard blackboard;

    /** Variables */
    private Arrive arrive;
    private float elapsedTime;
    private bool buying = false;

    private GameObject theCupboard;
    private GameObject theFridge;
    /** SteeringContext */
    private SteeringContext context;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        arrive = GetComponent<Arrive>();
        context = GetComponent<SteeringContext>();
        blackboard = GetComponent<P1_Worker_Blackboard>();

        /** Finder */
        theCupboard = GameObject.FindGameObjectWithTag("THE_CUPBOARD");
        theFridge = GameObject.FindGameObjectWithTag("THE_FRIDGE");

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
        State reachTheCupboard = new State("reachTheCupboard",
            () => {
                arrive.enabled = true;
                arrive.target = theCupboard;
            },
            () => { },
            () => { arrive.enabled = false; });

        State pickFood = new State("pickFood",
            () => {
                buying = true;
                elapsedTime = 0.0f;
            },
            () => { elapsedTime += Time.deltaTime; },
            () => { });

        State storeFoodOnFridge = new State("storeFoodOnFridge",
            () => {
                arrive.enabled = true;
                arrive.target = theFridge;
            },
            () => { elapsedTime += Time.deltaTime; },
            () => {
                buying = false;
                arrive.enabled = false;
                blackboard.StoreFood();
            });

        /** Transitions */
        Transition reachedTheCupboard = new Transition("reachedTheCupboard",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, theCupboard) < context.closeEnoughRadius || buying;
            }, () => { });

        Transition foodPicked = new Transition("foodPicked",
            () => {
                return elapsedTime >= blackboard.buyFoodTime || buying;
            }, () => { });

        Transition reachedTheFridge = new Transition("reachedTheFridge",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, theFridge) < context.closeEnoughRadius;
            }, () => { });


        /** FSM Set Up */
        AddStates(reachTheCupboard, pickFood, storeFoodOnFridge);
        /** -------------------------------------- */
        AddTransition(reachTheCupboard, reachedTheCupboard, pickFood);
        AddTransition(pickFood, foodPicked, storeFoodOnFridge);
        AddTransition(storeFoodOnFridge, reachedTheFridge, reachTheCupboard);
        /** ----------------------------------------------- */
        initialState = reachTheCupboard;

    }
}