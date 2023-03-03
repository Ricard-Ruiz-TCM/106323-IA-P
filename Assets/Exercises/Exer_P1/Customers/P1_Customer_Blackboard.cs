using UnityEngine;

public class P1_Customer_Blackboard : MonoBehaviour
{
    // Customer
    [Foldout("Customer", styled = true)]
    public float waitingTime;
    public float maxDistanceToConsiderSit;
    public float eatingFoodTime;
    public string availableChairTag;
    public string occupiedChairTag;
    public bool orderPicked;
    public bool foodDelivered;
    public Transform exitPoint;
    public GameObject myChair;
    public GameObject moneyPrefab;

    [Foldout("Human", styled = true)]
    public float antDetectionRadious;
    public float maxDistanceToWatchTV;
    public float hungryLevel;
    public GameObject angryPoint;
    public GameObject theAnt;
    

    public GameObject GetFirstAvailableChairTransform()
    {

        GameObject[] _chairs = GameObject.FindGameObjectsWithTag(availableChairTag);
        GameObject _firstChair = _chairs[0];
        _firstChair.tag = occupiedChairTag;
        myChair = _firstChair;

        return _firstChair;
    }

   

    public void DropMoney()
    {
       Instantiate(moneyPrefab, transform.position,Quaternion.identity);
    }

}