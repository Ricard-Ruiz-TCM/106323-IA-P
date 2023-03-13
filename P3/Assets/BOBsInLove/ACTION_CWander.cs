using UnityEngine;
using BTs;
using FSMs;
using Steerings;

public class ACTION_CWander : Action
{

    public string keyAttractor;
    public string keySafeRadius;
    public string keyExtraSafeRadius;
    public string keyLowSW;
    public string keyHighSW;


    // construtor(s)
    public ACTION_CWander(string keyAttractor, string keySafeRadius, string keyExtraSafeRadius,
                          string keyLowSW, string keyHighSW)  { 
        /* Receive action parameters and set them */
        this.keyAttractor = keyAttractor;
        this.keySafeRadius = keySafeRadius;
        this.keyExtraSafeRadius = keyExtraSafeRadius;
        this.keyLowSW = keyLowSW;
        this.keyHighSW = keyHighSW;
    }

    /* Declare private attributes here */

    private FSM_Cwander fsm;

    public override void OnInitialize()
    {
        /* write here the initialization code. Remember that initialization is executed once per ticking cycle */
        fsm = ScriptableObject.CreateInstance<FSM_Cwander>();
        fsm.Construct(gameObject);
        fsm.SetParameters(blackboard.Get<GameObject>(keyAttractor),
                          blackboard.Get<float>(keyLowSW),
                          blackboard.Get<float>(keyHighSW),
                          blackboard.Get<float>(keySafeRadius),
                          blackboard.Get<float>(keyExtraSafeRadius));
        fsm.OnEnter();
    }

    public override Status OnTick ()
    {
       fsm.Update();
       return Status.RUNNING;
    }

    public override void OnAbort()
    {
        fsm.OnExit();
    }
}


// ---------------------
// Action is based on the following (never terminating FSM)

class FSM_Cwander : FiniteStateMachine
{

    private GameObject attractor;
    private float lowSW, highSW;
    private float safeRadius, extraSafeRadius;

    private WanderAround wa;
    private SteeringContext sc;

    public void SetParameters (GameObject attractor, float lowSW, float highSW,
                               float safeRadius, float extraSafeRadius)
    {
        this.attractor = attractor;
        this.lowSW = lowSW;
        this.highSW = highSW;
        this.safeRadius = safeRadius;
        this.extraSafeRadius = extraSafeRadius; 
    }

    public override void OnEnter()
    {
        wa = GetComponent<WanderAround>();
        sc = GetComponent<SteeringContext>();
        wa.attractor = attractor;
        wa.enabled = true;
        base.OnEnter();
    }

    public override void OnExit()
    {
        wa.enabled = false;
        sc.seekWeight = lowSW;
        base.OnExit();
    }

    public override void OnConstruction()
    {
        State safe = new State("SAFE",
            () => { sc.seekWeight = lowSW; },
            () => {  },
            () => {  }
        );

        State notSafe = new State("UNSAFE",
            () => { sc.seekWeight = highSW; },
            () => { },
            () => { }
        );

        Transition tooFar = new Transition("Too Far",
            () => { return SensingUtils.DistanceToTarget(gameObject, attractor) > safeRadius; }    
        );

        Transition closeAgain = new Transition("Close Again",
            () => { return SensingUtils.DistanceToTarget(gameObject, attractor) <= extraSafeRadius; }
        );

        AddStates(safe, notSafe);
        AddTransition(safe, tooFar, notSafe);
        AddTransition(notSafe, closeAgain, safe);

        initialState = safe;
    }
}
