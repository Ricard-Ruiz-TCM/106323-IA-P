using UnityEngine;
using BTs;

[CreateAssetMenu(fileName = "BT_MakeBouquet", menuName = "Behaviour Trees/BT_MakeBouquet", order = 1)]
public class BT_MakeBouquet : BehaviourTree
{
    /* If necessary declare BT parameters here. 
       All public parameters must be of type string. All public parameters must be
       regarded as keys in/for the blackboard context.
       Use prefix "key" for input parameters (information stored in the blackboard that must be retrieved)
       use prefix "keyout" for output parameters (information that must be stored in the blackboard)

       e.g.
       public string keyDistance;
       public string keyoutObject 

       NOTICE: BT's with parameters cannot be constructed using ScriptableObject.CreateInstance<>
       An explicit constructor with new must be used. Unity will complain...
       Whenever possible, use parameter-less BT's. Use blackboard to pass information.
       TOP-level BTs (those attached to the executor) cannot have parameters
       
       In future versions, BT parameters may cease to exit

     */

     // construtor
    public BT_MakeBouquet()  { 
        /* Receive BT parameters and set them. Remember all are of type string */
    }
    
    public override void OnConstruction()
    {
        DynamicSelector MakeBouket = new DynamicSelector();
        MakeBouket.AddChild(
                new CONDITION_EnoughFlowers(),
                new LambdaAction(() =>
                {
                    ((BOB_Blackboard)blackboard).ActivateBouquet();
                    return Status.SUCCEEDED;
                })
            );

        MakeBouket.AddChild(
            new CONDITION_AlwaysTrue(),
            ScriptableObject.CreateInstance<BT_PickAndPee>()
            );

        root = MakeBouket;
    }
}
