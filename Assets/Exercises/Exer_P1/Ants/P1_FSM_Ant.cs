using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_P1_ANT", menuName = "Finite State Machines/FSM_P1_ANT", order = 1)]
public class P1_FSM_Ant : FiniteStateMachine {

    /** Blackboard */
    private P1_Ant_Blackboard blackboard;

    /** Variables */
    private Arrive arrive;
    private float elapsedTime = 0.0f;
    private GameObject theDishWithFood = null;

    /** SteeringContext */
    private SteeringContext context;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        arrive = GetComponent<Arrive>();
        context = GetComponent<SteeringContext>();
        blackboard = GetComponent<P1_Ant_Blackboard>();

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
        FiniteStateMachine pointWandering = ScriptableObject.CreateInstance<P1_Ant_PointWandering>();
        /** -------------------------------- */
        pointWandering.Name = "pointWandering";
        /** -------------------------------- */

        /** States */
        State reachFood = new State("reachFood",
            () => {
                arrive.enabled = true;
                arrive.target = theDishWithFood;
            },
            () => { },
            () => { arrive.enabled = false; });

        State eatFood = new State("eatFood",
            () => { elapsedTime = 0.0f; },
            () => { elapsedTime += Time.deltaTime; },
            () => { theDishWithFood.GetComponent<P1_DishController>().Dirty(); });

        /** Transitions */
        Transition foodDetected = new Transition("foodDetected",
            () => {
                theDishWithFood = SensingUtils.FindRandomInstanceWithinRadius(gameObject, "DISH_IN_USE", blackboard.dishWithFoodDetectionRadius);
                return theDishWithFood != null;
            }, () => { });

        Transition foodReached = new Transition("foodReached",
            () => { return SensingUtils.DistanceToTarget(gameObject, theDishWithFood) < context.closeEnoughRadius; },
            () => { });

        Transition foodEatedBySome1 = new Transition("foodEatedBySome1",
            () => { return theDishWithFood.tag != "DISH_IN_USE"; },
            () => { });

        Transition foodEat = new Transition("foodEat",
            () => { return elapsedTime >= blackboard.foodEatTime; },
            () => { });


        AddStates(pointWandering, reachFood, eatFood);
        /** ---------------------------------------- */
        AddTransition(pointWandering, foodDetected, reachFood);
        /** ------------------------------------------------ */
        AddTransition(reachFood, foodEatedBySome1, pointWandering);
        AddTransition(reachFood, foodReached, eatFood);
        /** ---------------------------------------- */
        AddTransition(eatFood, foodEat, pointWandering);
        /** ----------------------------------------- */
        initialState = pointWandering;
    }

}
