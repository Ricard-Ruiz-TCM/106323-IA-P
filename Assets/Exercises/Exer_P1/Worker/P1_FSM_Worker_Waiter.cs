using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_FSM_Worker_Waiter", menuName = "Finite State Machines/P1_FSM_Worker_Waiter", order = 1)]
public class P1_FSM_Worker_Waiter : FiniteStateMachine {

    /** Variables */
    private Arrive arrive;
    private P1_Worker_Blackboard blackboard;
    private float elapsedTime;

    /** OnEnter */
    public override void OnEnter() {


        Time.timeScale = 3f;
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

        /** FSM's */
        FiniteStateMachine Chef = ScriptableObject.CreateInstance<P1_FSM_Worker_Chef>();
        Chef.Name = "chef";

        /** States */
        State reachCustomer = new State("reachCustomer",
            () => {
                arrive.enabled = true;
                blackboard.waiterWorkDone = false;
                arrive.target = blackboard.theCustomer;
                elapsedTime = 0.0f;
            },
            () => { },
            () => {
                arrive.enabled = false;
            });

        State pickOrder = new State("pickOrder",
            () => {
                elapsedTime = 0.0f;
            },
            () => {
                elapsedTime += Time.deltaTime;
            },
            () => {
                blackboard.haveOrder = true;
                blackboard.CustomerBlackboard().orderPicked = true;
            });

        State deliverFood = new State("deliverFood",
            () => {
                arrive.enabled = true;
                arrive.target = blackboard.theCustomer;
                elapsedTime = 0.0f;
                blackboard.haveOrder = false;
            },
            () => {
                elapsedTime += Time.deltaTime;
            },
            () => {
                arrive.enabled = false;
                blackboard.waiterWorkDone = true;
                blackboard.haveCookedFood = false;
                blackboard.theCustomer.tag = blackboard.unTag;
                blackboard.theDish.GetComponent<P1_DishController>().PlaceOn(SensingUtils.FindInstanceWithinRadius(gameObject, "TABLE_SPOT", blackboard.tableSpotRadious));
                blackboard.theCustomer.tag = "Untagged";
                blackboard.CustomerBlackboard().foodDelivered = true;
                // TODO Héctor // "ENTREGAR" el plato al costumer
                    /** BORRAR ESTO */ blackboard.theDishBB().Dirty();
                blackboard.theCustomer = null;
                blackboard.money += 666;
                blackboard.updateHUD();
            });

        /** Transitions */
        Transition customerReached = new Transition("customerReached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.theCustomer) < blackboard.customerReachDistance || blackboard.haveOrder;
            }, () => { });

        Transition haveOrder = new Transition("haveOrder",
            () => {
                return elapsedTime >= blackboard.pickOrderTime || blackboard.haveOrder;
            }, () => { });

        Transition haveCookedFood = new Transition("haveCookedFood",
            () => {
                return blackboard.haveCookedFood;
            }, () => { });

        Transition foodDelivered = new Transition("foodDelivered",
            () => { 
                return elapsedTime >= blackboard.deliverFoodTime && SensingUtils.DistanceToTarget(gameObject, blackboard.theCustomer) < blackboard.customerReachDistance; 
            }, () => { });

        /** FSM Set Up */
        AddStates(reachCustomer, pickOrder, Chef, deliverFood);

        AddTransition(reachCustomer, haveCookedFood, deliverFood);
        AddTransition(Chef, haveCookedFood, deliverFood);
        AddTransition(reachCustomer, haveOrder, Chef);
        AddTransition(reachCustomer, customerReached, pickOrder);
        AddTransition(pickOrder, haveOrder, Chef);
        AddTransition(deliverFood, foodDelivered, reachCustomer);

        initialState = reachCustomer;

    }
}
