using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "P1_FSM_Customer", menuName = "Finite State Machines/P1_FSM_Customer", order = 1)]
public class P1_FSM_Customer : FiniteStateMachine
{

    private P1_Customer_Blackboard blackboard;
    private Arrive arrive;
   
    


    public override void OnEnter()
    {

        blackboard = GetComponent<P1_Customer_Blackboard>();
        arrive = GetComponent<Arrive>();
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {

        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {


        State findSit = new State("findSit", 
            () => { blackboard.myChair = null; blackboard.SetSprite(null, false); }, 
            () => { }, 
            () => { }
            
            );

        State reachSit = new State("ReachSit",
            () => {
                arrive.enabled = true;

                arrive.target = blackboard.myChair; 
               
                
            },
            () => {  },
            () => { arrive.enabled = false; }
            );

        State waitWaiter = new State("WaitWaiter",
            () => { blackboard.myChair.tag = blackboard.occupiedChairTag; gameObject.tag = "CUSTOMER"; blackboard.waitingTime = 0f; },
            () => { blackboard.waitingTime += Time.deltaTime; },
            () => { }
            );

        State waitFood = new State("WaitFood",
            () => { blackboard.waitingTime = 0f; },
            () => { blackboard.waitingTime += Time.deltaTime; },
            () => {  }
            );

        State eatFood = new State("EatFood",
           () => { blackboard.eatingFoodTime = 0f;
           },
           () => { blackboard.eatingFoodTime += Time.deltaTime;  },
           () => { } );


        Transition sitFinded = new Transition("SitFinded",
            () => {
                if(blackboard.myChair == null) blackboard.myChair = blackboard.GetFirstAvailableChairTransform();
                return blackboard.myChair != null;
            },
            () => { }
        );

        Transition sitStillAvailable = new Transition("SitStillAvailable",
           () => {
               return blackboard.myChair.tag != blackboard.availableChairTag;
           },
           () => { }
       );

        Transition sitReached = new Transition("SitReached",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.myChair) <= blackboard.maxDistanceToConsiderSit; }, 
            () => { }  
        );

        Transition waiterPickedOrder = new Transition("WaiterPickedOrder",
            () => { return blackboard.orderPicked; },
            () => { }
        );

        Transition waiterBringFood = new Transition("WaiterBringFood",
            () => { return blackboard.foodDelivered; },
            () => { }
        );

       



       // States
        AddStates(findSit, reachSit, waitWaiter, waitFood, eatFood);
        
        // Transitions
        AddTransition(findSit, sitFinded, reachSit);
        AddTransition(reachSit, sitReached, waitWaiter);
        AddTransition(reachSit, sitStillAvailable, findSit);
        AddTransition(waitWaiter, waiterPickedOrder, waitFood);
        AddTransition(waitFood, waiterBringFood, eatFood);


        initialState = findSit;

    }

}
