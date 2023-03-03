        using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "P1_FSM_Human", menuName = "Finite State Machines/P1_FSM_Human", order = 1)]
public class P1_FSM_Human : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    P1_Customer_Blackboard blackboard;
    Flee flee;
    Arrive arrive;
    private float elapsedTime;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        flee = GetComponent<Flee>();
        arrive = GetComponent<Arrive>();
        blackboard = GetComponent<P1_Customer_Blackboard>();

        blackboard.exitPoint = GameObject.FindGameObjectWithTag("CUSTOMER_SPAWN_POINT");
        blackboard.angryPoint = GameObject.FindGameObjectWithTag("CUSTOMER_SPAWN_POINT");

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

        // FMS
        FiniteStateMachine Customer = ScriptableObject.CreateInstance<P1_FSM_Customer>();
        Customer.Name = "Customer";

        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         
        State varName = new State("StateName",
            () => { }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );

         */
        State fleeFromAnts = new State("FleeFromAnts",
            () => { flee.enabled = true; flee.target = blackboard.theAnt; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { flee.enabled = false; }  // write on exit logic inisde {}  
        );


        State getAngry = new State("GetAngry",
           () => { arrive.enabled = true; arrive.target = blackboard.angryPoint; }, // write on enter logic inside {}
           () => { }, // write in state logic inside {}
           () => { arrive.enabled = false; }  // write on exit logic inisde {}  
       );

        State watchTV = new State("WatchTV",
            () => {  }, // write on enter logic inside {}
            () => { elapsedTime += Time.deltaTime; }, // write in state logic inside {}
            () => { elapsedTime = 0; }  // write on exit logic inisde {}  
        );


        State getHappy = new State("GetHappy",
           () => { arrive.enabled = true; arrive.target = blackboard.angryPoint; }, // write on enter logic inside {}
           () => { }, // write in state logic inside {}
           () => { arrive.enabled = false; }  // write on exit logic inisde {}  
       );

        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------

        Transition varName = new Transition("TransitionName",
            () => { }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        */

        Transition antDetected = new Transition("AntDetected",
           () => { return SensingUtils.FindInstanceWithinRadius(gameObject, "ANT", blackboard.antDetectionRadious) != null;  }, // write the condition checkeing code in {}
           () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
       );

        Transition antNotDetected = new Transition("AntNotDetected",
           () => { return SensingUtils.FindInstanceWithinRadius(gameObject, "ANT", blackboard.antDetectionRadious) == null; }, // write the condition checkeing code in {}
           () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
       );

        Transition tvReached = new Transition("TVReached",
           () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.angryPoint) <= blackboard.maxDistanceToWatchTV; }, // write the condition checkeing code in {}
           () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
       );

        Transition beingHungry = new Transition("BeingHungry",
          () => { return elapsedTime >= blackboard.hungryLevel; }, // write the condition checkeing code in {}
          () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
      );

        Transition longWaitTime = new Transition("longWaitTime",
          () => { return elapsedTime >= blackboard.hungryLevel; }, // write the condition checkeing code in {}
          () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
      );


        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
            
        AddStates(...);

        AddTransition(sourceState, transition, destinationState);

         */
        AddState(Customer);
        AddState(fleeFromAnts);
        AddState(watchTV);
        AddState(getAngry);
        AddState(getHappy);


        AddTransition(Customer, antDetected, fleeFromAnts);
        AddTransition(Customer, longWaitTime, getAngry);


        AddTransition(fleeFromAnts, antNotDetected, Customer);
        AddTransition(getAngry, tvReached, watchTV);
        AddTransition(watchTV, beingHungry, Customer);
        AddTransition(getHappy, tvReached, watchTV);

        /* STAGE 4: set the initial state
         
        initialState = ... 

         */

        initialState = Customer;

    }
}
