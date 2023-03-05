using Steerings;
using UnityEngine;
using System.Collections;

public class P1_Ant_GroupManager : GroupManager {

    [Header("Spawn Settings:")]
    public int numInstances = 20;
    public float delay = 0.5f;
    public GameObject prefab;
    public Transform spawn;
    public bool holeLocked = false;
    public SpriteRenderer holeSprite;
    public float unlockHoleTime = 10.0f;
    public float radiousLockHole;

    /** Related GroupManager Variables */
    private int created = 0;
    private float elapsedTime = 0f;


    // Unity Update
    private void Update() {
        Spawn();
        CheckHoleState();
    }

    private void CheckHoleState()
    {
        GameObject _employee = SensingUtils.FindInstanceWithinRadius(spawn.gameObject, "Player", radiousLockHole);
        if (_employee != null) LockHole();
        
    }

    private void Spawn() {
        // Update Amount
        created = members.Count;

        // InstanceAmount Control
        if (created == numInstances || holeLocked)
            return;

        // Time Control
        if (elapsedTime < delay) {
            elapsedTime += Time.deltaTime;
            return;
        }

        // Spawn 
        elapsedTime = 0.0f;
        GameObject ant = Instantiate(prefab, spawn);
        ant.name = "ANT"; AddBoid(ant);
    }

    public void LockHole() {
        holeLocked = true;
        holeSprite.color = Color.black;
        StartCoroutine(UnlockHoleCoroutine());
    }

    private IEnumerator UnlockHoleCoroutine() {
        yield return new WaitForSeconds(unlockHoleTime);
        holeLocked = false;
        holeSprite.color = Color.white;
    }
}
