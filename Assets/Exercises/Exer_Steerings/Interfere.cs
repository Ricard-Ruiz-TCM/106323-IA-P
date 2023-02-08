using UnityEngine;

namespace Steerings {

    public class Interfere : SteeringBehaviour {
        public GameObject target;
        public float requiredDistance;
        /*
        // remove comments for steerings that must be provided with a target 
        // remove whole block if no explicit target required
        // (if FT or FTI policies make sense, then this method must be present)
        public GameObject target;

        public override GameObject GetTarget()
        {
            return target;
        }
        */

        public override Vector3 GetLinearAcceleration() {
            return Interfere.GetLinearAcceleration(Context, target, requiredDistance);
        }


        public static Vector3 GetLinearAcceleration(SteeringContext me, GameObject target, float requiredDistance) {
            //interferencePoint = target.transform.position + displacement;
            //displacement = (targetVelocity.normalize/ movement direction)* requiredDistance

            Vector3 targetVelocity = target.GetComponent<SteeringContext>().velocity;
            Vector3 lineOfMovement = targetVelocity.normalized;
            Vector3 displacementFromTarget = lineOfMovement * requiredDistance;
            Vector3 interferencePoint = target.transform.position + displacementFromTarget;

            SURROGATE_TARGET.transform.position = interferencePoint;


            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);
            /* COMPLETE this method. It must return the linear acceleration (Vector3) */
            //return Vector3.zero;
        }

    }
}