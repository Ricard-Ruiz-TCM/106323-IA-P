using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_FSM_Worker_Chef", menuName = "Finite State Machines/P1_FSM_Worker_Chef", order = 1)]
public class P1_FSM_Worker_Chef : FiniteStateMachine {

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
        
        /** FSM's */
        FiniteStateMachine ChefAssistant = ScriptableObject.CreateInstance<P1_FSM_Worker_ChefAssistant>();
        ChefAssistant.Name = "chefAssistant";

        FiniteStateMachine FoodBuyer = ScriptableObject.CreateInstance<P1_FSM_Worker_FoodBuyer>();
        FoodBuyer.Name = "foodBuyer";

        /** States */
        State reachCleanPlate = new State("reachCleanPlate",
            () => {
                arrive.enabled = true;
                arrive.target = blackboard.theCleanDishPile;
            },
            () => { },
            () => {
                arrive.enabled = false;
            });

        State reachFood = new State("reachFood",
            () => {
                arrive.enabled = true;
                arrive.target = blackboard.theFridge;
            },
            () => {
            },
            () => {
                arrive.enabled = false;
            });

        State cookFood = new State("cookFood",
            () => {
                arrive.enabled = true;
                arrive.target = blackboard.theCook;
                elapsedTime = 0.0f;
            },
            () => {
                elapsedTime += Time.deltaTime;
            },
            () => {
                arrive.enabled = false;
                blackboard.haveCookedFood = true;
                blackboard.totalPlatesInUse++;
                blackboard.totalCleanPlates--;
                blackboard.totalFood--;
            });

        /** Transitions */
        Transition havePlates = new Transition("havePlates",
            () => {
                return blackboard.totalCleanPlates > 0 && SensingUtils.DistanceToTarget(gameObject, blackboard.theCleanDishPile) < blackboard.pointReachRadius;
            }, () => { });

        Transition haveNoPlates = new Transition("haveNoPlates",
            () => {
                return blackboard.totalCleanPlates <= 0;
            }, () => { });

        Transition haveFood = new Transition("haveFood",
            () => {
                return blackboard.totalFood > 0 && SensingUtils.DistanceToTarget(gameObject, blackboard.theFridge) < blackboard.pointReachRadius;
            }, () => { });

        Transition haveNoFood = new Transition("haveNoFood",
            () => {
                return blackboard.totalFood <= 0;
            }, () => { });

        Transition foodCooked = new Transition("foodCooked",
            () => {
                return elapsedTime >= blackboard.cookFoodTime;
            }, () => { });


        /** FSM Set Up */
        AddStates(reachCleanPlate, reachFood, cookFood, ChefAssistant, FoodBuyer);

        AddTransition(reachCleanPlate, havePlates, reachFood);
        AddTransition(reachCleanPlate, haveNoPlates, ChefAssistant);
        AddTransition(ChefAssistant, havePlates, reachFood);

        AddTransition(reachFood, haveFood, cookFood);
        AddTransition(reachFood, haveNoFood, FoodBuyer);
        AddTransition(FoodBuyer, haveFood, cookFood);

        AddTransition(cookFood, foodCooked, reachCleanPlate);

        initialState = reachCleanPlate;
    }

}