using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_P1_ANT", menuName = "Finite State Machines/FSM_P1_ANT", order = 1)]
public class FSM_P1_ANT : FiniteStateMachine {

    Arrive arrive;
    P1_ANT_Blackboard blackboard;
    FlockingAround flockingAround;

    GameObject theFood;

    float elapsedTime;

    public override void OnEnter() {
        arrive = GetComponent<Arrive>();
        blackboard = GetComponent<P1_ANT_Blackboard>();
        flockingAround = GetComponent<FlockingAround>();

        elapsedTime = 0.0f;

        base.OnEnter();
    }

    public override void OnExit() {
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction() {
       
         
        State wanderAround = new State("wanderAround",
            () => {
                flockingAround.enabled = true;
            }, 
            () => { },
            () => {
                flockingAround.enabled = false;
            }
        );

        State reachFood = new State("reachFood",
            () => {
                arrive.enabled = true;
                arrive.target = theFood;
            },
            () => { },
            () => {
                arrive.enabled = false;
            }
        );

        State eatFood = new State("eatFood",
            () => {
                theFood.tag = "NO_ITEM";
                elapsedTime = 0.0f;
            },
            () => {
                elapsedTime += Time.deltaTime;
            },
            () => {
                GameObject.Destroy(theFood);
            }
        );

        Transition foodDetected = new Transition("foodDetected",
            () => {
                theFood = SensingUtils.FindRandomInstanceWithinRadius(gameObject, "FOOD", blackboard.foodDetectionRadius);
                return theFood != null;
            }, 
            () => { }
        );
        Transition foodReached = new Transition("foodEaten",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, theFood) < blackboard.foodReachRadius;
            },
            () => { }
        );
        Transition foodEaten = new Transition("TransitionName",
            () => {
                return elapsedTime >= blackboard.foodEatTime;
            },
            () => { }
        );
            
        AddStates(wanderAround, reachFood, eatFood);

        AddTransition(wanderAround, foodDetected, reachFood);
        AddTransition(reachFood, foodReached, eatFood);
        AddTransition(eatFood, foodEaten, wanderAround);


        initialState = wanderAround;

    }
}
