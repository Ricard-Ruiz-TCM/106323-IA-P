using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_FSM_Worker_FoodBuyer", menuName = "Finite State Machines/P1_FSM_Worker_FoodBuyer", order = 1)]
public class P1_FSM_Worker_FoodBuyer : FiniteStateMachine {

    /** Variables */
    private Arrive arrive;
    private float elapsedTime;
    private bool buying = false;
    P1_Worker_Blackboard blackboard;

    GameObject theCupboard;
    GameObject theFridge;
    /** SteeringContext */
    private SteeringContext context;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        arrive = GetComponent<Arrive>();
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
        State reachFoodShop = new State("reachFoodShop",
            () => {
                arrive.enabled = true;
                arrive.target = theCupboard;
            },
            () => { },
            () => { arrive.enabled = false; });

        State buyFood = new State("buyFood",
            () => {
                buying = true;
                elapsedTime = 0.0f;
            },
            () => { elapsedTime += Time.deltaTime; },
            () => { });

        State storeFood = new State("storeFood",
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
        Transition reachedFoodMachine = new Transition("reachedFoodMachine",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, theCupboard) < context.closeEnoughRadius || buying;
            }, () => { });

        Transition foodBuyed = new Transition("foodBuyed",
            () => {
                return elapsedTime >= blackboard.buyFoodTime || buying;
            }, () => { });

        Transition theFoodStore = new Transition("theFoodStore",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, theFridge) < context.closeEnoughRadius;
            }, () => { });


        /** FSM Set Up */
        AddStates(reachFoodShop, buyFood, storeFood);
        /** -------------------------------------- */
        AddTransition(reachFoodShop, reachedFoodMachine, buyFood);
        AddTransition(buyFood, foodBuyed, storeFood);
        AddTransition(storeFood, theFoodStore, reachFoodShop);
        /** ----------------------------------------------- */
        initialState = reachFoodShop;

    }
}