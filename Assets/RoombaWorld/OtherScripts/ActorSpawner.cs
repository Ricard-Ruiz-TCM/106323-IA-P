using UnityEngine;
using System.Collections;

public class ActorSpawner : MonoBehaviour {
    
    [Header("Actor Container:")]
    public Transform actors;

    [Header("Spawn Timming:")]
    public int spawnDelay;
    public int spawnDelayRange = 0;

    [Header("Prefab:")]
    public GameObject item;

    // Unity Awake
    private void Awake() {
        actors = (actors == null ? transform : actors);
        new RandomLocationGenerator();
    }

    // Unity Start
    private void Start() {
        StartCoroutine(SpawnRoutine());
    }

    /** DustSpawnRoutine */
    public IEnumerator SpawnRoutine() {
        // Spawn
        GameObject g = Instantiate(item, actors);
        g.transform.position = (Vector2)RandomLocationGenerator.RandomWalkableLocation();
        // Wait
        yield return new WaitForSeconds(Random.Range(spawnDelay - spawnDelayRange, spawnDelay + spawnDelayRange));
        StartCoroutine(SpawnRoutine());
    }

}
