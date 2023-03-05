using UnityEngine;

public class P1_Customer_Blackboard : MonoBehaviour
{
    // Customer
    [Foldout("Customer", styled = true)]
    public float waitingTime;
    public float maxWaitingTime;
    public float maxDistanceToConsiderSit;
    public float eatingFoodTime;
    public float maxEatingFoodTime;
    public string availableChairTag;
    public string occupiedChairTag;
    public bool orderPicked;
    public bool foodDelivered;
    public GameObject exitPoint;
    public GameObject myChair;
    public GameObject moneyPrefab;
    public GameObject myDish;

    public float dishDetectRadius = 10.0f;

    public float antAndDishDistance = 3.0f;

    [Foldout("Human", styled = true)]
    public float antDetectionRadious;
    public float maxDistanceToAngryPoint;
    public float hungryLevel;
    /** borrar */ public float currentHungry;
    public GameObject angryPoint;
    public GameObject theAnt;

    public GameObject GetFirstAvailableChairTransform()
    {

        GameObject[] _chairs = GameObject.FindGameObjectsWithTag(availableChairTag);
        
        if(_chairs.Length != 0)
        {
            GameObject _firstChair = _chairs[0];
            return _firstChair;
        }
        else
        {
            return null;
        }
       

        
    }

   

    public void DropMoney()
    {
        
       Instantiate(moneyPrefab, transform.position,Quaternion.identity);
    }

}