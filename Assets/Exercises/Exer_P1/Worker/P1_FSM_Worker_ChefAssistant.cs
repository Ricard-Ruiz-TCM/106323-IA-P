using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_FSM_Worker_ChefAssistant", menuName = "Finite State Machines/P1_FSM_Worker_ChefAssistant", order = 1)]
public class P1_FSM_Worker_ChefAssistant : FiniteStateMachine {

    /** Variables */
    private Arrive arrive;
    private P1_Worker_Blackboard blackboard;
    private float elapsedTime;

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
        State reachDirtyPlate = new State("reachDirtyPlate",
            () => {
                arrive.enabled = true;
                arrive.target = blackboard.theDish;
            },
            () => { },
            () => {
                arrive.enabled = false;
                blackboard.theDish.transform.SetParent(gameObject.transform);
            });

        State washUpPlate = new State("washUpPlate",
            () => {
                arrive.enabled = true;
                arrive.target = blackboard.theSink;
                elapsedTime = 0.0f;
            },
            () => {
                elapsedTime += Time.deltaTime;
            },
            () => {
                arrive.enabled = false;
            });

        State storePlate = new State("storePlate",
            () => {
                arrive.enabled = true;
                arrive.target = blackboard.theCleanDishPile;
            },
            () => {
                elapsedTime += Time.deltaTime;
            },
            () => {
                arrive.enabled = false;
                blackboard.theDishBB().WashUpDish();
            });

        /** Transitions */
        Transition dirtyDishPicked = new Transition("dirtyDishPicked",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.theDish) < blackboard.pointReachRadius;
            }, () => { });

        Transition washedUpDish = new Transition("washedUpDish",
            () => {
                return elapsedTime >= blackboard.washUpTime;
            }, () => { });

        Transition cleanDishStored = new Transition("cleanDishStored",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, blackboard.theCleanDishPile) < blackboard.pointReachRadius;
            }, () => { });


        /** FSM Set Up */
        AddStates(reachDirtyPlate, washUpPlate, storePlate);

        AddTransition(reachDirtyPlate, dirtyDishPicked, washUpPlate);
        AddTransition(washUpPlate, washedUpDish, storePlate);
        AddTransition(storePlate, cleanDishStored, reachDirtyPlate);

        initialState = reachDirtyPlate;
    }

}