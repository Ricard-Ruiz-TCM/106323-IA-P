using UnityEngine;

public class P1_Customer_Blackboard : MonoBehaviour
{
    // Customer
    [Foldout("Customer", styled = true)]
    public float waitingTime;
    public string availableChairTag;
    public string occupiedChairTag;
    public Transform exitPoint;

    public GameObject GetFirstAvailableChairTransform()
    {

        GameObject[] _chairs = GameObject.FindGameObjectsWithTag(availableChairTag);
        GameObject _firstChair = _chairs[0];
        _firstChair.tag = occupiedChairTag;

        return _firstChair;
    }

}