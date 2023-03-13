using UnityEngine;
using BTs;

[CreateAssetMenu(fileName = "BT_PickFlowers", menuName = "Behaviour Trees/BT_PickFlowers", order = 1)]
public class BT_PickFlowers : BehaviourTree
{
    

     // construtor
    public BT_PickFlowers()  { 
        /* Receive BT parameters and set them. Remember all are of type string */
    }
    
    public override void OnConstruction()
    {
        DynamicSelector BobPickFlowers = new DynamicSelector();
        // instance of flower near
        BobPickFlowers.AddChild(
            new CONDITION_InstanceNear("flowerDetectionRadius", "FLOWER", "false", "theFlower"),
            new Sequence(
                    new ACTION_Arrive("theFlower"),
                    new ACTION_Deactivate("theFlower"),
                    new LambdaAction(() =>
                        {
                            ((BOB_Blackboard)blackboard).CountFlower();
                            return Status.SUCCEEDED;
                        })
                )
        );

        // always true
        BobPickFlowers.AddChild(
            new CONDITION_AlwaysTrue(),
            new ACTION_CWander("thePark","80", "40" , "0.2", "0.8")
        );

        root = new RepeatForeverDecorator(BobPickFlowers);

        /* Write here (method OnConstruction) the code that constructs the Behaviour Tree 
           Remember to set the root attribute to a proper node
           e.g.
            ...
            root = new Sequence();
            ...

          A behaviour tree can use other behaviour trees.  
      */
    }
}
