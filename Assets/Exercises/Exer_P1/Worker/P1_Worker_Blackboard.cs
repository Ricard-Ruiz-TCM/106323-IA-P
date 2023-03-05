using TMPro;
using Steerings;
using UnityEngine;

public class P1_Worker_Blackboard : MonoBehaviour {

    [Header("Game:")]
    public int money = 0;
    public int totalFood = 0;

    [Header("Costs:")]
    public int serviceCost = 5;
    public int serviceFood = 3;

    [Foldout("Distances & Radius", styled = true)]
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
    /* Tables */
    public float tableSpotRadious = 20.0f;
    /* Dish */
    public float dishDetectionRadius = 100.0f;

    /** Ant Killer STM */
    [Foldout("AntKiller", styled = true)]
    public GameObject theAnt = null;
    public float killAntTime = 0.1f;

    public void KillAnt() {
        theAnt.GetComponent<SteeringContext>().groupManager.RemoveBoid(theAnt);
        GameObject.Destroy(theAnt);
    }

    /** Pick Up Money STM */
    [Foldout("PickUpMoney", styled = true)]
    public GameObject theMoney = null;

    public void StoreMoney() {
        GameObject.Destroy(theMoney);
        money += serviceCost; updateHUD();
    }

    /** Waiter STM */
    [Foldout("Waiter", styled = true)]
    public GameObject theCustomer = null;
    public float pickOrderTime = 2.5f;
    public float deliverFoodTime = 5.0f;

    /** Chef */
    [Foldout("Chef", styled = true)]
    public bool haveCookedFood = false;
    public float cookFoodTime = 5.0f;

    /** Chef Assistant ---------------------- */
    [Foldout("Chef Assistant", styled = true)]
    public GameObject theDish;
    public float washUpTime = 2.5f;
    /** ------------------------------------- */

    /** Food Buyer -------------------------- */
    [Foldout("Food Buyer", styled = true)]
    public float buyFoodTime = 2.5f;

    public void StoreFood() {
        totalFood += serviceFood; updateHUD();
    }
    /** ------------------------------------- */
    
    /** HUD --------------------------------- */
    [Foldout("HUD", styled = true)]
    public TextMeshProUGUI _moneyAmount;

    public void updateHUD() {
        _moneyAmount.text = money.ToString();
    }
    /** ------------------------------------- */

}
