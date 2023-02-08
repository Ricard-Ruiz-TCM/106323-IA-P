using UnityEngine;

namespace Steerings
{

    public class InterferePlusAvoid : SteeringBehaviour
    {
        
        public GameObject target;
        public float requiredDistance;

        public override GameObject GetTarget()
        {
            return target;
        }
       
        
        public override Vector3 GetLinearAcceleration()
        {
            return InterferePlusAvoid.GetLinearAcceleration(Context, target, requiredDistance);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float requiredDistance)
        {
            Vector3 OAAcceleration = ObstacleAvoidance.GetLinearAcceleration(me);
            if (OAAcceleration != Vector3.zero)
                return OAAcceleration;
            else
                return Interfere.GetLinearAcceleration(me, target, requiredDistance);
        }

    }
}