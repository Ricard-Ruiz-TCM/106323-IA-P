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
    private float elapsedTime; 
    


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
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */

        State reachSit = new State("ReachSit",
            () => { arrive.target = blackboard.GetFirstAvailableChairTransform(); arrive.enabled = true;  },
            () => { },
            () => { arrive.enabled = false; }
            );

        State waitWaiter = new State("WaitWaiter",
            () => { },
            () => { elapsedTime += Time.deltaTime; },
            () => { elapsedTime = 0f; }
            );

        State waitFood = new State("WaitFood",
            () => { },
            () => { elapsedTime += Time.deltaTime; },
            () => { elapsedTime = 0f; }
            );

        State eatFood = new State("EatFood",
           () => { },
           () => { elapsedTime += Time.deltaTime;  },
           () => {blackboard.DropMoney(); elapsedTime = 0f; }
           );
        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */

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

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */

        AddState(reachSit);
        AddState(waitWaiter);
        AddState(waitFood);

        AddTransition(reachSit, sitReached, waitWaiter);
        //AddTransition(waitWaiter, waiterPickedOrder, waitFood);
        //AddTransition(waitFood, waiterBringFood, eatFood);

        /* STAGE 4: set the initial state
         
        initialState = ... 

         */
        initialState = reachSit;

    }

}
