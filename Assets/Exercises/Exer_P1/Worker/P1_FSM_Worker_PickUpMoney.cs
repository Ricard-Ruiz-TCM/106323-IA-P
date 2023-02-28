using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_FSM_Worker_PickUpMoney", menuName = "Finite State Machines/P1_FSM_Worker_PickUpMoney", order = 1)]
public class P1_FSM_Worker_PickUpMoney : FiniteStateMachine {

    /** Variables */
    private Arrive arrive;
    private P1_Worker_Blackboard blackboard;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        arrive = GetComponent<Arrive>();
        blackboard = GetComponent<P1_Worker_Blackboard>();

        blackboard.theCashier = GameObject.FindGameObjectWithTag(blackboard.cashierTag);

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
                blackboard.theMoney.transform.SetParent(transform);
            });

        State storeMoney = new State("storeMoney",
            () => {
                arrive.target = blackboard.theCashier;
            },
            () => { },
            () => {
                GameObject.Destroy(blackboard.theMoney);
                arrive.enabled = false;
            });

        /** Transitions */
        Transition moneyPicked = new Transition("moneyPicked",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.theMoney) < blackboard.moneyReachDistance;
            }, () => { });

        Transition cashierReached = new Transition("cashierReached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.theCashier) < blackboard.cashierReachDistance;
            }, () => { });

        /** FSM Set Up */
        AddStates(reachMoney, storeMoney);

        AddTransition(reachMoney, moneyPicked, storeMoney);
        AddTransition(storeMoney, cashierReached, reachMoney);

        initialState = reachMoney;

    }
}
