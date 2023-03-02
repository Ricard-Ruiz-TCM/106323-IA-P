using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_FSM_Worker_FoodBuyer", menuName = "Finite State Machines/P1_FSM_Worker_FoodBuyer", order = 1)]
public class P1_FSM_Worker_FoodBuyer : FiniteStateMachine {

    /** Variables */
    private Arrive arrive;
    private P1_Worker_Blackboard blackboard;
    private float elapsedTime;
    private bool buying = false;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        arrive = GetComponent<Arrive>();
        blackboard = GetComponent<P1_Worker_Blackboard>();

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
                arrive.target = blackboard.theShop;
            },
            () => { },
            () => {
                arrive.enabled = false;
            });

        State buyFood = new State("buyFood",
            () => {
                elapsedTime = 0.0f;
                buying = true;
            },
            () => {
                elapsedTime += Time.deltaTime;
            }, 
            () => { });

        State storeFood = new State("storeFood",
            () => {
                arrive.enabled = true;
                arrive.target = blackboard.theFridge;
            },
            () => {
                elapsedTime += Time.deltaTime;
            },
            () => {
                arrive.enabled = false;
                blackboard.totalFood += 4;
                buying = false;
            });

        /** Transitions */
        Transition reachedFoodMachine = new Transition("reachedFoodMachine",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.theShop) < blackboard.pointReachRadius || buying;
            }, () => { });

        Transition foodBuyed = new Transition("foodBuyed",
            () => {
                return elapsedTime >= blackboard.buyFoodTime || buying;
            }, () => { });

        Transition theFoodStore = new Transition("theFoodStore",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.theFridge) < blackboard.pointReachRadius;
            }, () => { });


        /** FSM Set Up */
        AddStates(reachFoodShop, buyFood, storeFood);

        AddTransition(reachFoodShop, reachedFoodMachine, buyFood);
        AddTransition(buyFood, foodBuyed, storeFood);
        AddTransition(storeFood, theFoodStore, reachFoodShop);

        initialState = reachFoodShop;
    }

}