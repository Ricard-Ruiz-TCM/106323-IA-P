using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "P1_FSM_Customer", menuName = "Finite State Machines/P1_FSM_Customer", order = 1)]
public class P1_FSM_Customer : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    private P1_Customer_Blackboard blackboard;
    private Arrive arrive;
    private GameObject myChair;
    


    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        blackboard = GetComponent<P1_Customer_Blackboard>();
        arrive = GetComponent<Arrive>();
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {


        State findSit = new State("findSit", 
            () => { }, 
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
           () => {
               if (blackboard.myDish != null) {
                   blackboard.myDish.GetComponent<P1_DishController>().Dirty();
               }
               blackboard.myDish = null;
           }
           );


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

       



       
        AddState(findSit);
        AddState(reachSit);
        AddState(waitWaiter);
        AddState(waitFood);
        AddState(eatFood);

        AddTransition(findSit, sitFinded, reachSit);
        AddTransition(reachSit, sitReached, waitWaiter);
        AddTransition(reachSit, sitStillAvailable, findSit);
        AddTransition(waitWaiter, waiterPickedOrder, waitFood);
        AddTransition(waitFood, waiterBringFood, eatFood);


        initialState = findSit;

    }

}
