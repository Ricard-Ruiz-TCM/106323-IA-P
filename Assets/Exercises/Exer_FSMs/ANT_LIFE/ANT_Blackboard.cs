
using UnityEngine;

public class ANT_Blackboard : MonoBehaviour {

    [Header("Two point wandering")]
    public GameObject locationA;
    public GameObject locationB;
    public float intervalBetweenTimeouts = 10.0f;
    [Range(0, 1)] public float initialSeekWeight = 0.2f;
    [Range(0, 1)] public float seekIncrement = 0.2f;
    public float locationReachedRadius = 10.0f;

    [Header("Seed colecting")]
    public GameObject theNest;
    public float seedDetectionRadius = 100.0f;
    public float seedReachedRadius = 5.0f;
    public float nestReachedRadius = 20.0f;

    // [Header("Peril Fleeing")]
    // public float perilDetectionRadius =

}
