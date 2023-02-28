using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_FSM_Worker_FullTimeEmployee", menuName = "Finite State Machines/P1_FSM_Worker_FullTimeEmployee", order = 1)]
public class P1_FSM_Worker_FullTimeEmployee : FiniteStateMachine {

    /** Variables */
    private P1_Worker_Blackboard blackboard;
    private WanderAround wanderAround;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        blackboard = GetComponent<P1_Worker_Blackboard>();
        wanderAround = GetComponent<WanderAround>();

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
        FiniteStateMachine AntKiller = ScriptableObject.CreateInstance<P1_FSM_Worker_AntKiller>();
        FiniteStateMachine PickUpMoney = ScriptableObject.CreateInstance<P1_FSM_Worker_PickUpMoney>();
        FiniteStateMachine Waiter = ScriptableObject.CreateInstance<P1_FSM_Worker_Waiter>();

        AntKiller.Name = "antKiller";
        PickUpMoney.Name = "pickUpMoney";
        Waiter.Name = "waiter";

        /** States */
        State wanderAroundState = new State("wanderArount",
            () => {
                wanderAround.enabled = true;
            },
            () => { },
            () => {
                wanderAround.enabled = false;
            }
        );

        /** Transitions */
        Transition antDetected = new Transition("antDetected",
            () => {
                blackboard.theAnt = SensingUtils.FindInstanceWithinRadius(gameObject, blackboard.antTag, blackboard.antDetectionRadius);
                return blackboard.theAnt != null;
            }, () => { });

        Transition antNotDetected = new Transition("antNotDetected",
            () => {
                return blackboard.theAnt == null || blackboard.theAnt.Equals(null);
            }, () => { });

        Transition moneyDetected = new Transition("moneyDetected",
            () => {
                blackboard.theMoney = SensingUtils.FindInstanceWithinRadius(gameObject, blackboard.moneyTag, blackboard.moneyDetectionRadius);
                return blackboard.theMoney != null;
            }, () => { });

        Transition moneyNotDetected = new Transition("moneyNotDetected",
            () => {
                return blackboard.theMoney == null || blackboard.theMoney.Equals(null);
            }, () => { });

        Transition customerDetected = new Transition("customerDetected",
            () => {
                blackboard.theCustomer = SensingUtils.FindRandomInstanceWithinRadius(gameObject, blackboard.customerTag, blackboard.customerDetectionRadius);
                return blackboard.theCustomer;
            }, () => { });

        Transition waiterWorkDone = new Transition("waiterWorkDone",
            () => {
                return blackboard.waiterWorkDone || (blackboard.theCustomer == null || blackboard.theCustomer.Equals(null));
            }, () => { });

        /** FSM Set Up */
        AddStates(wanderAroundState, AntKiller, PickUpMoney, Waiter);

        AddTransition(wanderAroundState, antDetected, AntKiller);
        AddTransition(AntKiller, antNotDetected, wanderAroundState);

        AddTransition(wanderAroundState, moneyDetected, PickUpMoney);
        AddTransition(PickUpMoney, moneyNotDetected, wanderAroundState);
        AddTransition(PickUpMoney, antDetected, AntKiller);

        AddTransition(wanderAroundState, customerDetected, Waiter);
        AddTransition(Waiter, waiterWorkDone, wanderAroundState);
        AddTransition(Waiter, antDetected, AntKiller);
        AddTransition(Waiter, moneyDetected, PickUpMoney);

        initialState = wanderAroundState;

    }
}
