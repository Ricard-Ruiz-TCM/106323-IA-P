using FSMs;
using Steerings;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_SearchWorms", menuName = "Finite State Machines/FSM_SearchWorms", order = 1)]
public class FSM_SearchWorms : FiniteStateMachine {

    private HEN_Blackboard blackboard;
    private WanderAround wanderAround;
    private Arrive arrive;
    private AudioSource audioSource;
    private GameObject theWorm;
    private float elapsedTime;

    public override void OnEnter() {
        blackboard = GetComponent<HEN_Blackboard>();
        arrive = GetComponent<Arrive>();
        wanderAround = GetComponent<WanderAround>();
        audioSource = GetComponent<AudioSource>(); ;
        base.OnEnter();
    }

    public override void OnExit() {
        audioSource.Stop();
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction() {
        State wander = new State("Wander",
            () => {
                audioSource.clip = blackboard.cluckingSound;
                audioSource.Play();
                wanderAround.enabled = true;
            },
            () => { },
            () => {
                audioSource.Stop();
                wanderAround.enabled = false;
            }
        );

        State reachWorm = new State("ReachWorm",
            () => {
                arrive.target = theWorm;
                arrive.enabled = true;
            },
            () => { },
            () => {
                arrive.enabled = false;
            }
        );

        State eat = new State("Eat",
            () => {
                audioSource.clip = blackboard.eatingSound;
                audioSource.Play();
                elapsedTime = 0.0f;
            },
            () => {
                elapsedTime += Time.deltaTime;
            },
            () => {
                audioSource.Stop();
                GameObject.Destroy(theWorm);
            }
        );

        Transition wormDetected = new Transition("wormDetected",
            () => {
                theWorm = SensingUtils.FindInstanceWithinRadius(gameObject, "WORM", blackboard.wormDetectableRadius);
                return theWorm != null;
            },
            () => { }
        );
        
        Transition wormReached = new Transition("wormReached",
            () => {
                return SensingUtils.DistanceToTarget(gameObject, theWorm) < blackboard.wormReachedRadius;
            },
            () => { }
        );

        Transition wormVanished = new Transition("wormVanished",
            () => {
                return (theWorm == null || theWorm.Equals(null));
            },
            () => { }
        );

        Transition timeOut = new Transition("timeOut",
            () => {
                return (elapsedTime >= blackboard.timeToEatWorm);
            },
            () => { }
        );

        AddStates(wander, reachWorm, eat);

        AddTransition(wander, wormDetected, reachWorm);
        AddTransition(reachWorm, wormVanished, wander);
        AddTransition(reachWorm, wormReached, eat);
        AddTransition(eat, timeOut, wander);

        initialState = wander;
    }
}
