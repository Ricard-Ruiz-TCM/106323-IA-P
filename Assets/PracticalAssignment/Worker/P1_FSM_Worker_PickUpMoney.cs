using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_FSM_Worker_PickUpMoney", menuName = "Finite State Machines/P1_FSM_Worker_PickUpMoney", order = 1)]
public class P1_FSM_Worker_PickUpMoney : FiniteStateMachine {

    /** Blackboard */
    private P1_Worker_Blackboard blackboard;

    /** Variables */
    private Arrive arrive;

    private GameObject theCashier;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        arrive = GetComponent<Arrive>();
        blackboard = GetComponent<P1_Worker_Blackboard>();

        /** Finder */
        theCashier = GameObject.FindGameObjectWithTag("CASHIER");

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
        State reachMoney = new State("reachMoney",
            () => {
                arrive.enabled = true;
                arrive.target = blackboard.theMoney;
            },
            () => { },
            () => {
                arrive.enabled = false;
                blackboard.theMoney.transform.SetParent(transform);
            });

        State storeMoney = new State("storeMoney",
            () => {
                arrive.enabled = true;
                arrive.target = theCashier;
            },
            () => { },
            () => {
                arrive.enabled = false;
                blackboard.StoreMoney();
            });

        /** Transitions */
        Transition moneyPicked = new Transition("moneyPicked",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.theMoney) < blackboard.moneyReachDistance;
            }, () => { });

        Transition cashierReached = new Transition("cashierReached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, theCashier) < blackboard.cashierReachDistance;
            }, () => { });

        /** FSM Set Up */
        AddStates(reachMoney, storeMoney);
        /** --------------------------- */
        AddTransition(reachMoney, moneyPicked, storeMoney);
        /** -------------------------------------------- */
        AddTransition(storeMoney, cashierReached, reachMoney);
        /** ----------------------------------------------- */
        initialState = reachMoney;
    }

}
