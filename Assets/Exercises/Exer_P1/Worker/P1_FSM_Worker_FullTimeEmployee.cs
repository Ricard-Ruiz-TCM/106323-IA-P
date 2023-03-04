using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_FSM_Worker_FullTimeEmployee", menuName = "Finite State Machines/P1_FSM_Worker_FullTimeEmployee", order = 1)]
public class P1_FSM_Worker_FullTimeEmployee : FiniteStateMachine {

    /** Blackboard */
    private P1_Worker_Blackboard blackboard;

    /** Variables */
    private WanderAroundPlusAvoid wanderAround;

    /** OnEnter */
    public override void OnEnter() {

        Time.timeScale = 5.0f;

        /** GetComponent */
        blackboard = GetComponent<P1_Worker_Blackboard>();
        wanderAround = GetComponent<WanderAroundPlusAvoid>();

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
        FiniteStateMachine antKiller = ScriptableObject.CreateInstance<P1_FSM_Worker_AntKiller>();
        FiniteStateMachine pickUpMoney = ScriptableObject.CreateInstance<P1_FSM_Worker_PickUpMoney>();
        FiniteStateMachine waiter = ScriptableObject.CreateInstance<P1_FSM_Worker_Waiter>();
        /** -------------------------------------------------------------------------------- */
        antKiller.Name = "antKiller"; pickUpMoney.Name = "pickUpMoney"; waiter.Name = "waiter";
        /** -------------------------------------------------------------------------------- */

        /** States */
        State wanderAroundState = new State("wanderAroundState",
            () => { wanderAround.enabled = true; },
            () => { },
            () => { wanderAround.enabled = false; });

        /** Transitions */
        Transition antDetected = new Transition("antDetected",
            () => {
                blackboard.theAnt = SensingUtils.FindInstanceWithinRadius(gameObject, "ANT", blackboard.antDetectionRadius);
                return blackboard.theAnt != null;
            }, () => { });

        Transition antNOTDetected = new Transition("antNOTDetected",
            () => {
                return blackboard.theAnt == null || blackboard.theAnt.Equals(null);
            }, () => { });

        Transition moneyDetected = new Transition("moneyDetected",
            () => {
                blackboard.theMoney = SensingUtils.FindRandomInstanceWithinRadius(gameObject, "MONEY", blackboard.moneyDetectionRadius);
                return blackboard.theMoney != null;
            }, () => { });

        Transition moneyNOTDetected = new Transition("moneyNOTDetected",
            () => {
                return blackboard.theMoney == null || blackboard.theMoney.Equals(null);
            }, () => { });

        Transition customerNOTDetected = new Transition("customerNOTDetected",
            () => {
                return blackboard.theCustomer == null || blackboard.theCustomer.Equals(null);
            }, () => { });

        Transition customerDetected = new Transition("customerDetected",
            () => {
                if (blackboard.theCustomer != null) { return true; }
                blackboard.theCustomer = SensingUtils.FindRandomInstanceWithinRadius(gameObject, "CUSTOMER", blackboard.customerDetectionRadius);
                return blackboard.theCustomer;
            }, () => { });

        /** FSM Set Up */
        AddStates(wanderAroundState, antKiller, pickUpMoney, waiter);
        /** ------------------------------------------------------ */
        AddTransition(wanderAroundState, antDetected, antKiller);
        AddTransition(wanderAroundState, moneyDetected, pickUpMoney);
        AddTransition(wanderAroundState, customerDetected, waiter);
        /** -------------------------------------------------- - */
        AddTransition(antKiller, antNOTDetected, wanderAroundState);
        /** ----------------------------------------------------- */
        AddTransition(pickUpMoney, antDetected, antKiller);
        AddTransition(pickUpMoney, moneyNOTDetected, wanderAroundState);
        /** --------------------------------------------------------- */
        AddTransition(waiter, antDetected, antKiller);
        AddTransition(waiter, moneyDetected, pickUpMoney);
        AddTransition(waiter, customerNOTDetected, wanderAroundState);
        /** -------------------------------------------------- */
        initialState = wanderAroundState;
    }

}
