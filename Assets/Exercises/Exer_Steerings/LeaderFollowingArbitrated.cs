using UnityEngine;

// Leader following combines (blends) Keep position with linear repulsion
// (linear respulsion prevents de agent from colliding against the leader

namespace Steerings
{

    public class LeaderFollowingArbitrated : SteeringBehaviour
    {

        
        public GameObject target;
        public float requiredDistance;
        public float requiredAngle;

        public override GameObject GetTarget()
        {
            return target;
        }
      
        
        public override Vector3 GetLinearAcceleration()
        {
            return LeaderFollowingArbitrated.GetLinearAcceleration(Context, target, requiredDistance, requiredAngle );
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, 
                                                     float distance, float angle)
        {
            // Give priority to linear repulsion
            // (if linear repulsion is not Vector3.Zero return linear repulsion
            // else return Keep Position)
            /* COMPLETE */

            Vector3 lr = LinearRepulsion.GetLinearAcceleration(me);
            if (lr != Vector3.zero)
                return lr;
            else return 
                    KeepPosition.GetLinearAcceleration(me, target, distance, angle);
        }

    }
}