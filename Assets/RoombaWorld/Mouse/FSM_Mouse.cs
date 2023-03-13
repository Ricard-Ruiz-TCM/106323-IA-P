using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_Mouse", menuName = "Finite State Machines/FSM_Mouse", order = 1)]
public class FSM_Mouse : FiniteStateMachine {

    /** Blackboard */
    private MOUSE_Blackboard blackboard;
    private GameObject roomba;
    private GoToTarget goToTarget;

    /** SteeringContext */
    private SteeringContext context;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        blackboard = GetComponent<MOUSE_Blackboard>();
        goToTarget = GetComponent<GoToTarget>();
        context = GetComponent<SteeringContext>();

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

    /** OnConstruction */
    public override void OnConstruction() {

        /** FSM's */
        FiniteStateMachine mouseBehaviour = ScriptableObject.CreateInstance<FSM_MouseBehaviour>();
        /** ----------------------------------------------------------------------------------- */
        mouseBehaviour.Name = "mouseBehaviour";
        /** -------------------------------- */

        /** States */

        State reachExit = new State("reachExit",
            () => {
                goToTarget.target = blackboard.NearestExitPoint();
            },
            () => { },
            () => { }
        );

        /** Transitions */
        Transition roombaDetected = new Transition("roombaDetected",
            () => {
                roomba = SensingUtils.FindInstanceWithinRadius(gameObject, "ROOMBA", blackboard.roombaDetectionRadius);                
                return roomba != null; },
            () => {
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                context.maxSpeed *= 2;
                context.maxAcceleration *= 4;
            }
        );


        Transition exitReached = new Transition("exitReached",
            () => { return goToTarget.routeTerminated(); },
            () => {
                GameObject.Destroy(gameObject);
            }
        );


        /** FSM Set Up */
        AddStates(mouseBehaviour, reachExit);
        /** ---------------------------------------------------------------- */
        AddTransition(mouseBehaviour, roombaDetected, reachExit);
        /** ----------------------------------------------------- */
        AddTransition(reachExit, exitReached, mouseBehaviour);
        /** ----------------------------------------------- */
        initialState = mouseBehaviour;

    }
}
