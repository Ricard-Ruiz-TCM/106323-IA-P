using TMPro;
using UnityEngine;

public class P1_Worker_Blackboard : MonoBehaviour {

    /** Gizmos X Debug */
    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, antDetectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, moneyDetectionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, customerDetectionRadius);
    }

    /** Tags */
    [Foldout("Tags", styled = true)]
    public string customerTag = "CUSTOMER";
    public string antTag = "ANT";
    public string moneyTag = "MONEY";
    public string cashierTag = "CASHIER";
    public string unTag = "Untagged";

    [Foldout("Distances & Radius", styled = true)]
    public float pointReachRadius = 0.1f;
    /** FullTimeEmplyee */
        /* Customers */
    public float customerDetectionRadius = 20.0f;
    public float customerReachDistance = 4.0f;
        /* Ants */
    public float antDetectionRadius = 8.0f;
    public float antReachDistance = 2.0f;
        /* Money */
    public float moneyDetectionRadius = 12.5f;
    public float moneyReachDistance = 2.0f;
    public float cashierReachDistance = 2.0f;

    /** Ant Killer */
    [Foldout("AntKiller", styled = true)]
    public GameObject theAnt = null;
    public float killAntTime = 0.1f;

    /** Pick Up Money */
    [Foldout("PickUpMoney", styled = true)]
    public int money;
    public int serviceCost = 5;
    public GameObject theMoney;
    public void StoreMoney() {
        GameObject.Destroy(theMoney);
        money += serviceCost;
        updateHUD();
    }

    /** Waiter */
    [Foldout("Waiter", styled = true)]
    public GameObject theCustomer;
    public float pickOrderTime = 2.5f;
    public bool haveOrder = false;
    public bool waiterWorkDone = false;
    public float deliverFoodTime = 5.0f;
    public float tableSpotRadious;

    /** Chef */
    [Foldout("Chef", styled = true)]
    public bool haveCookedFood = false;
    public int totalFood = 0;
    public GameObject theCook;
    public float cookFoodTime = 5.0f;

    /** Chef Assistant */
    [Foldout("Chef Assistant", styled = true)]
    public float dishDetectionRadius = 100.0f;
    public GameObject theDish;
    public float washUpTime = 2.5f;
    public GameObject theCleanDishPile;
    public GameObject theSink;

    public P1_DishController theDishBB() { 
        return theDish.GetComponent<P1_DishController>();
    }

    public P1_Customer_Blackboard CustomerBlackboard()
    {
        return theCustomer.GetComponent<P1_Customer_Blackboard>();
    }
    /** Food Buyer */
    [Foldout("Food Buyer", styled = true)]
    public float buyFoodTime = 2.5f;
    public int serviceFood = 3;

    public void StoreFood()
    {
        totalFood += serviceFood;
        updateHUD();
    }

    [Foldout("HUD", styled = true)]
    public TextMeshProUGUI _moneyAmount;

    public void updateHUD() {
        _moneyAmount.text = money.ToString();
    }

}
