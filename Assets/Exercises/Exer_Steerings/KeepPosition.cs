using UnityEngine;

namespace Steerings {

    public class KeepPosition : SteeringBehaviour {

        public GameObject target;
        public float requiredDistance;
        public float requiredAngle;

        /* COMPLETE */
        public override GameObject GetTarget() {
            return target;
        }

        public override Vector3 GetLinearAcceleration() {
            /* COMPLETE */
            //return Vector3.zero; // remove this line when exercise completed
            return GetLinearAcceleration(Context, target, requiredDistance, requiredAngle);
        }


        public static Vector3 GetLinearAcceleration(SteeringContext me, GameObject target,
                                                     float distance, float angle) {

            float DesiredAngle = target.transform.rotation.eulerAngles.z + angle;
            Vector3 DirectionFromTarget = Utils.OrientationToVector(DesiredAngle); //surt normalitzat
            Vector3 displacement = DirectionFromTarget * distance;
            Vector3 DesiredPosition = target.transform.position + displacement;

            SURROGATE_TARGET.transform.position = DesiredPosition;

            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);

            /* COMPLETE */
            //return Vector3.zero; // remove this line when exercise completed
        }

        public override float GetAngularAcceleration() {
            //return base.GetAngularAcceleration();
            return GetAngularAcceleration(Context, target, requiredAngle);
        }

        public static float GetAngularAcceleration(SteeringContext me, GameObject target, float angle) {


            return Align.GetAngularAcceleration(me, SURROGATE_TARGET);
        }
    }
}