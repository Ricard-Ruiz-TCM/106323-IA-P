using UnityEngine;

namespace Steerings {

    public class P1_Ant_GroupManager : Steerings.GroupManager {

        public int numInstances = 20;
        public float delay = 0.5f;
        public GameObject prefab;
        public GameObject attractor;

        private int created = 0;
        private float elapsedTime = 0f;


        // Update is called once per frame
        void Update() {
            Spawn();
        }

        private void Spawn() {

            if (created == numInstances) return;

            if (elapsedTime < delay) {
                elapsedTime += Time.deltaTime;
                return;
            }

            // if this point is reached, it's time to spawn a new instance
            GameObject clone = Instantiate(prefab);
            AddBoid(clone);
            clone.GetComponent<FlockingAround>().attractor = attractor;
            created++;
            elapsedTime = 0.0f;
        }
    }
}