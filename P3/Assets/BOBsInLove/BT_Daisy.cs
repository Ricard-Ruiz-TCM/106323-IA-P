using UnityEngine;
using BTs;

[CreateAssetMenu(fileName = "BT_Daisy", menuName = "Behaviour Trees/BT_Daisy", order = 1)]
public class BT_Daisy : BehaviourTree
{
   

     // construtor
    public BT_Daisy()  { 
        /* Receive BT parameters and set them. Remember all are of type string */
    }
    
    public override void OnConstruction()
    {
        DAISY_Blackboard bbd = (DAISY_Blackboard)blackboard;

        RepeatForeverDecorator pace = new RepeatForeverDecorator(
            new Sequence (
                new LambdaAction(() => {
                    GameObject corner = bbd.GetRandomCorner();
                    bbd.Put("theCorner", corner);
                    return Status.SUCCEEDED; }
                ),
                new ACTION_Arrive("theCorner")
            )    
        );

        RepeatForeverDecorator mumble = new RepeatForeverDecorator(
            new Sequence(
                new LambdaAction(() => {
                    string utterance = bbd.GetRandomUtterance();
                    bbd.Put("theUtterance", utterance);
                    return Status.SUCCEEDED;
                }
                ),
                new ACTION_Speak("theUtterance"),
                new ACTION_WaitForSeconds("5")
                //new ACTION_Quiet()
            )
        );

        ParallelAnd paceAndMumble = new ParallelAnd(
            mumble,
            pace
        );


        DynamicSelector dyn = new DynamicSelector();
        dyn.AddChild(
            new CONDITION_InstanceNear("15", "BOUQUET", "true"),
            new Sequence(
                new ACTION_Speak("Finally here!"),
                new ACTION_WaitForSeconds("7"),
                new ACTION_Quiet(),
                new LambdaAction(() => { bbd.CloseDoor(); return Status.SUCCEEDED; }),
                new ACTION_Activate("hearts"),
                new ACTION_RunForever()
            )
        ) ;
        dyn.AddChild(
            new CONDITION_AlwaysTrue(),
            paceAndMumble
        );

        root = dyn;
    }
}
