using UnityEngine;
using BTs;

public class CONDITION_EnoughFlowers : Condition
{

    // Constructor
    public CONDITION_EnoughFlowers()  {
        /* Receive function parameters and set them */
    }

    public override bool Check ()
    {
        return ((BOB_Blackboard)blackboard).EnoughFlowers();

    }

}
