using UnityEngine;

public class P1_Customer_Blackboard : MonoBehaviour
{
    // Customer
    [Foldout("Customer", styled = true)]
    public float waitingTime;
    public string availableChairTag;
    public string occupiedChairTag;
    public float chairDetectionRadious;
    public Transform exitPoint;

    public Transform GetFirstAvailableChairTransform()
    {

        GameObject[] _chairs = GameObject.FindGameObjectsWithTag(availableChairTag);
        Transform _firstChair = _chairs[0].transform;
        _firstChair.tag = occupiedChairTag;

        return _firstChair;
    }

}