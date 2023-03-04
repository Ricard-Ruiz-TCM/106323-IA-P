using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_FSM_Worker_Chef", menuName = "Finite State Machines/P1_FSM_Worker_Chef", order = 1)]
public class P1_FSM_Worker_Chef : FiniteStateMachine {

    /** Blackboard */
    private P1_Worker_Blackboard blackboard;

    /** Variables */
    private Arrive arrive;
    private float elapsedTime;
    private bool cooking = false;

    private GameObject theFridge;
    /** SteeringContext */
    private SteeringContext context;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        arrive = GetComponent<Arrive>();
        context = GetComponent<SteeringContext>();
        blackboard = GetComponent<P1_Worker_Blackboard>();

        /** Finder */
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
        
        /** FSM's */
        FiniteStateMachine ChefAssistant = ScriptableObject.CreateInstance<P1_FSM_Worker_ChefAssistant>();
        FiniteStateMachine FoodBuyer = ScriptableObject.CreateInstance<P1_FSM_Worker_FoodBuyer>();
        /** ------------------------------------------------------------ */
        ChefAssistant.Name = "chefAssistant"; FoodBuyer.Name = "foodBuyer";
        /** ------------------------------------------------------------ */

        /** States */
        State findDish = new State("findDish", () => { }, () => { }, () => { });

        State reachPlate = new State("reachPlate",
            () => { 
                arrive.enabled = true;
                arrive.target = blackboard.theDish;
            },
            () => { },
            () => {
                arrive.enabled = false;
                blackboard.theDish.GetComponent<P1_DishController>().Pick(transform);
             });

        State reachFood = new State("reachFood",
            () => {
                arrive.enabled = true;
                arrive.target = theFridge;
            },
            () => { },
            () => { arrive.enabled = false; });

        State cookFood = new State("cookFood",
            () => {
                arrive.enabled = true;
                arrive.target = blackboard.theCook;
                elapsedTime = 0.0f;
                cooking = true;
            },
            () => {
                elapsedTime += Time.deltaTime;
            },
            () => {
                cooking = false;
                arrive.enabled = false;
                blackboard.haveCookedFood = true;
                blackboard.theDishBB().PlaceFood();
            });

        /** Transitions */
        Transition havePlates = new Transition("havePlates",
            () => {
                return SensingUtils.FindInstanceWithinRadius(gameObject, "DISH_CLEAN", blackboard.dishDetectionRadius) != null;
            }, () => {
                blackboard.theDish = SensingUtils.FindInstanceWithinRadius(gameObject, "DISH_CLEAN", blackboard.dishDetectionRadius);
            } );

        Transition haveNoPlates = new Transition("haveNoPlates",
            () => {
                blackboard.theDish = SensingUtils.FindInstanceWithinRadius(gameObject, "DISH_DIRTY", blackboard.dishDetectionRadius);
                return blackboard.theDish != null;
            }, () => { });

        Transition plateReached = new Transition("plateReached",
        () => {
            return SensingUtils.DistanceToTarget(gameObject, blackboard.theDish) < blackboard.pointReachRadius;
        }, () => {} );

        Transition haveFood = new Transition("haveFood",
            () => {
                return blackboard.totalFood > 0 && SensingUtils.DistanceToTarget(gameObject, theFridge) < blackboard.pointReachRadius || cooking;
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
        AddStates(findDish, reachPlate, reachFood, cookFood, ChefAssistant, FoodBuyer);

        AddTransition(findDish, havePlates, reachPlate);
        AddTransition(findDish, haveNoPlates, ChefAssistant);
        AddTransition(ChefAssistant, havePlates, reachFood);

        AddTransition(reachPlate, plateReached, reachFood);

        AddTransition(reachFood, haveFood, cookFood);
        AddTransition(reachFood, haveNoFood, FoodBuyer);
        AddTransition(FoodBuyer, haveFood, cookFood);

        AddTransition(cookFood, foodCooked, findDish);

        initialState = findDish;
    }

}