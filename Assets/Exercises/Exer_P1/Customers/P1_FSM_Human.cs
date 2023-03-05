using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_FSM_Human", menuName = "Finite State Machines/P1_FSM_Human", order = 1)]
public class P1_FSM_Human : FiniteStateMachine {
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/
    P1_Customer_Blackboard blackboard;
    Arrive arrive;
    public float elapsedTime;
    

    public override void OnEnter() {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        arrive = GetComponent<Arrive>();
        blackboard = GetComponent<P1_Customer_Blackboard>();

        blackboard.exitPoint = GameObject.FindGameObjectWithTag("CUSTOMER_SPAWN_POINT");
        blackboard.angryPoint = GameObject.FindGameObjectWithTag("CUSTOMER_SPAWN_POINT");

        base.OnEnter(); // do not remove
    }

    public override void OnExit() {
      
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction() {

        FiniteStateMachine Customer = ScriptableObject.CreateInstance<P1_FSM_Customer>();
        Customer.Name = "Customer";



        State getAngry = new State("GetAngry",
           () => {
               arrive.enabled = true;
               arrive.target = blackboard.angryPoint;
               gameObject.tag = "Untagged";
               blackboard.SetSprite(blackboard.angryEmoji, true);
           }, // write on enter logic inside {}
           () => { }, // write in state logic inside {}
           () => { arrive.enabled = false; }  // write on exit logic inisde {}  
       );

        State goOutside = new State("GoOutside",
            () => {
               
                blackboard.myChair.tag = "CHAIR_SPOT";
                blackboard.myChair = null;
                blackboard.orderPicked = false;
                blackboard.foodDelivered = false;
                elapsedTime = 0;
                blackboard.waitingTime = 0f; 
                blackboard.eatingFoodTime = 0f;
            }, // write on enter logic inside {}
            () => { blackboard.currentHungry += Time.deltaTime; }, // write in state logic inside {}
            () => { }  // write on exit logic inisde {}  
        );


        State getHappy = new State("GetHappy",
           () => {
               blackboard.DropMoney();
               blackboard.SetSprite(blackboard.happyEmoji, true);
               arrive.enabled = true;
               arrive.target = blackboard.angryPoint;
               gameObject.tag = "Untagged";
           }, // write on enter logic inside {}
           () => { }, // write in state logic inside {}
           () => { arrive.enabled = false; }  // write on exit logic inisde {}  
       );

  

        Transition antDetectedOnMyDish = new Transition("antDetectedOnMyDish",
           () => {
               if (blackboard.myDish == null) {
                  
                   return false;
               }
               blackboard.theAnt = SensingUtils.FindInstanceWithinRadius(gameObject, "ANT", blackboard.antDetectionRadious);
               if (blackboard.theAnt == null) {
                  
                   return false;
               }
              
               return Vector2.Distance(blackboard.myDish.transform.position, blackboard.theAnt.transform.position) <= blackboard.antAndDishDistance ;
           },
           () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
       );

        Transition outPointReached = new Transition("OutPointReached",
           () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.angryPoint) <= blackboard.maxDistanceToAngryPoint; }, // write the condition checkeing code in {}
           () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
       );

        Transition beingHungry = new Transition("BeingHungry",
          () => { return elapsedTime >= blackboard.hungryLevel; }, // write the condition checkeing code in {}
          () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
      );

        Transition longWaitTime = new Transition("longWaitTime",
          () => { return blackboard.waitingTime >= blackboard.maxWaitingTime; }, // write the condition checkeing code in {}
          () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
      );

        Transition foodEaten = new Transition("FoodEaten",
           () => { return blackboard.eatingFoodTime >= blackboard.maxEatingFoodTime; },
           () => { }


           );


        AddStates(Customer, goOutside, getAngry, getHappy);

        AddTransition(Customer, antDetectedOnMyDish, getAngry);
        AddTransition(Customer, longWaitTime, getAngry);
        AddTransition(Customer, foodEaten, getHappy);


        AddTransition(getAngry, outPointReached, goOutside);
        AddTransition(getHappy, outPointReached, goOutside);

        AddTransition(goOutside, beingHungry, Customer);

        initialState = Customer;

    }
}
