using FSMs;
using UnityEngine;
using Steerings;
using System;

[CreateAssetMenu(fileName = "FSM_Work", menuName = "Finite State Machines/FSM_Work", order = 1)]
public class FSM_Work : FiniteStateMachine
{

    private BOB_Blackboard blackboard;
    private Arrive arrive;

    private GameObject theBox;
    private int transported;

    private bool inSuccess;
    private bool inFailure;

    public bool InSuccess () { return this.inSuccess; }
    public bool InFailure () { return this.inFailure; }

    public override void OnEnter()
    {
        blackboard = GetComponent<BOB_Blackboard>();
        arrive = GetComponent<Arrive>();
        transported = 0;
        inSuccess = false;
        inFailure = false;
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        State goingBoxArea = new State("GOING TO BOX AREA",
            () => { arrive.target = blackboard.boxesArea;
                arrive.enabled = true;
            },
            () => { },
            () => { arrive.enabled = false; }
        );

        State selectABox = new State("SELECT A BOX",
          () => { },
          () => { theBox = SensingUtils.FindRandomInstanceWithinRadius(gameObject, "BOX", 1000); },
          () => { }
        );

        State goingToBox = new State("GOING TO BOX",
            () => { arrive.target = theBox; arrive.enabled = true; },
            () => { },
            () => { arrive.enabled = false; }
        );

        State transportingBox = new State("TRANSPORTING BOX",
            () => {
                theBox.transform.parent = transform;
                arrive.target = blackboard.theWarehouse;
                arrive.enabled = true;
            },
            () => { },
            () => { 
                arrive.enabled = false;
                theBox.transform.parent = null;
                   }
        );

      

        State SUCCESS = new State("SUCCESS",
            () => { inSuccess = true; },
            () => { },
            () => { }
        );

        State FAILURE = new State("FAILURE",
            () => { inFailure = true; },
            () => { },
            () => { }
        );


        Transition boxesAreaReached = new Transition("Boxes Area Reached",
           () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.boxesArea) <= 5;}, 
           () => {  }  
        );

        Transition boxFound = new Transition("Box Found",
          () => { return theBox != null; },
          () => { }
        );

        Transition boxNotFound = new Transition("Box Not Found",
          () => { return theBox == null; },
          () => { }
        );

        Transition boxReached = new Transition("Box Reached",
           () => { return SensingUtils.DistanceToTarget(gameObject, theBox) <= 5; },
           () => { }
        );

        Transition warehouseReached = new Transition("Wharehouse reached",
           () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.theWarehouse) <= 5; },
           () => { transported++; }
        );

        Transition warehouseReachedWithLastBox = new Transition("Wharehouse reached with last box",
           () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.theWarehouse) <= 5 && transported == 2; },
           () => { transported++; /* not strictly necessary */ }
        );

        AddStates(goingBoxArea, selectABox, goingToBox, transportingBox, SUCCESS, FAILURE);

        AddTransition(goingBoxArea, boxesAreaReached, selectABox);

        AddTransition(selectABox, boxFound, goingToBox);
        AddTransition(selectABox, boxNotFound, FAILURE);

        AddTransition(goingToBox, boxReached, transportingBox);

        AddTransition(transportingBox, warehouseReachedWithLastBox, SUCCESS);
        AddTransition(transportingBox, warehouseReached, goingBoxArea);

        initialState = goingBoxArea;

    }
}
