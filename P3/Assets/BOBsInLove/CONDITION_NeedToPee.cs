using BTs;

public class CONDITION_NeedToPee : Condition
{
    // Constructor
    public CONDITION_NeedToPee()  {
       
    }

    public override bool Check ()
    {
        return ((BOB_Blackboard)blackboard).PeeAlarmIsOn();
    }
}
