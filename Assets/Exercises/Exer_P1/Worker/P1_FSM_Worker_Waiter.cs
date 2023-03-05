using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_FSM_Worker_Waiter", menuName = "Finite State Machines/P1_FSM_Worker_Waiter", order = 1)]
public class P1_FSM_Worker_Waiter : FiniteStateMachine {

    /** Blackboard */
    private P1_Worker_Blackboard blackboard;

    /** Variables */
    private Arrive arrive;
    private float elapsedTime;
    private bool activeOrder;

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

        /** FSM's */
        FiniteStateMachine chef = ScriptableObject.CreateInstance<P1_FSM_Worker_Chef>();
        /** ------------ */
        chef.Name = "chef";
        /** ------------ */

        /** States */
        State reachCustomer = new State("reachCustomer",
            () => {
                elapsedTime = 0.0f;
                arrive.enabled = true;
                arrive.target = blackboard.theCustomer;
            },
            () => { },
            () => { arrive.enabled = false; });

        State pickOrder = new State("pickOrder",
            () => {
                elapsedTime = 0.0f;
                activeOrder = false;
            },
            () => { elapsedTime += Time.deltaTime; },
            () => {
                activeOrder = true;
                blackboard.theCustomer.GetComponent<P1_Customer_Blackboard>().orderPicked = true;
            });

        State deliverFood = new State("deliverFood",
            () => {
                arrive.enabled = true;
                arrive.target = blackboard.theCustomer;
                elapsedTime = 0.0f;
            },
            () => { elapsedTime += Time.deltaTime; },
            () => {
                arrive.enabled = false;
                blackboard.haveCookedFood = false;
                Transform spot = null; bool place = false;
                if (blackboard.theCustomer.tag != "Untagged") {
                    spot = SensingUtils.FindInstanceWithinRadius(gameObject, "TABLE_SPOT", blackboard.tableSpotRadious).transform;
                    place = true; }
                blackboard.theDish.transform.SetParent(null);
                blackboard.theDish.GetComponent<P1_DishController>().PlaceOn(spot, place);
                blackboard.theCustomer.tag = "Untagged";
                blackboard.theCustomer.GetComponent<P1_Customer_Blackboard>().foodDelivered = true;
                blackboard.theCustomer.GetComponent<P1_Customer_Blackboard>().myDish = blackboard.theDish;
                blackboard.theCustomer = null;
                activeOrder = false;
            });

        /** Transitions */
        Transition customerReached = new Transition("customerReached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.theCustomer) < blackboard.customerReachDistance;
            }, () => { });

        Transition haveOrder = new Transition("haveOrder",
            () => {
                return elapsedTime >= blackboard.pickOrderTime || activeOrder;
            }, () => { });

        Transition haveCookedFood = new Transition("haveCookedFood",
            () => {
                return blackboard.haveCookedFood;
            }, () => { });

        Transition foodDelivered = new Transition("foodDelivered",
            () => {
                return elapsedTime >= blackboard.deliverFoodTime && SensingUtils.DistanceToTarget(gameObject, blackboard.theCustomer) < blackboard.customerReachDistance;
            }, () => { });

        Transition angryCustomer = new Transition("angryCustomer",
            () => {
                return blackboard.theCustomer.tag == "Untagged";
            }, () => { });

        /** FSM Set Up */
        AddStates(reachCustomer, pickOrder, chef, deliverFood);
        /** ------------------------------------------------ */
        AddTransition(reachCustomer, haveCookedFood, deliverFood);
        AddTransition(reachCustomer, haveOrder, chef);
        AddTransition(reachCustomer, customerReached, pickOrder);
        /** -------------------------------------------------- */
        AddTransition(chef, haveCookedFood, deliverFood);
        /** ------------------------------------------ */
        AddTransition(pickOrder, haveOrder, chef);
        /** ----------------------------------- */
        AddTransition(deliverFood, foodDelivered, reachCustomer);
        AddTransition(deliverFood, angryCustomer, reachCustomer);
        /** -------------------------------------------------- */
        initialState = reachCustomer;

    }
}
