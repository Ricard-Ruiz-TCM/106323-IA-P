using UnityEngine;

namespace Steerings {

    public class Interpose : SteeringBehaviour {

        public GameObject target;
        public GameObject secondTarget;

        public override GameObject GetTarget() {
            return target;
        }

        public override Vector3 GetLinearAcceleration() {
            return Interpose.GetLinearAcceleration(Context, target, secondTarget);
        }

        public static Vector3 GetLinearAcceleration(SteeringContext me, GameObject target, GameObject secondTarget) {

            Vector3 midPoint = (target.transform.position + secondTarget.transform.position) / 2.0f;

            SURROGATE_TARGET.transform.position = midPoint;

            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);
        }

    }
}