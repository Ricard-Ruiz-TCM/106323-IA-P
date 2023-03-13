
using UnityEngine;

public class BOB_Blackboard : DynamicBlackboard {

    public int money;
    public int salary = 20;
    public float flowerDetectionRadius = 20;
    public int requiredFlowersForBouquet = 25;
    public int jewelPrice = 55;
    public int flowers = 0;

    public bool peeAlarm = false; // when true, it may be urgent to find a toilet...

    public GameObject thePark; // the gameobject representing the center of the park
    public GameObject boxesArea; // the gameobject representing the area where the agent can find boxes
    public GameObject theWarehouse; // where boxes are left
    public GameObject wc; // where the toilet is
    public GameObject theToilet; // where...
    public GameObject theJewellery; // where Bob buys...
    public GameObject daisys; // Daisy's place

    private TextMesh moneyLine;
    private TextMesh flowersLine;

    // Use this for initialization
    void Start () {
        moneyLine = GameObject.Find("MoneyLine").GetComponent<TextMesh>();
        if (moneyLine != null) moneyLine.text = "Money: " + money;

        flowersLine = GameObject.Find("FlowersLine").GetComponent<TextMesh>();
        if (flowersLine != null) flowersLine.text = "Flowers: " + flowers;

    }
	
	// Update is called once per frame
	void Update () {
        if (flowersLine != null) flowersLine.text = "Flowers: " + flowers;

        // check if p (for pee) has been pressed...
        if (Input.GetKeyDown(KeyCode.P))
        {
            peeAlarm = true;
        }
    }

    public void ActivateBouquet()
    {
        FindChildWithTag(gameObject, "BOUQUET").SetActive(true);
    }

    public void ActivateRing()
    {
        FindChildWithTag(gameObject, "RING").SetActive(true);
    }

    public void ActivateNecklace() 
    {
        FindChildWithTag(gameObject, "NECKLACE").SetActive(true);
    }

    public void CountFlower() { flowers++; }

    public void GetPaid () {
        money += salary;
        Debug.Log("being paid");
        if (moneyLine != null) moneyLine.text = "Money: " + money;
    }

    public bool PeeAlarmIsOn() { 
        return peeAlarm; 
    }

    public void PeeAlarmOff()
    {
        peeAlarm = false;
    }

    public bool EnoughFlowers ()
    {
        return flowers >= requiredFlowersForBouquet;
    }

    public bool EnoughMoney()
    {
        return money >= jewelPrice;
    }

    public void PayJewel ()
    {
        money -= jewelPrice;
    }

    public void CloseDoor ()
    {
        GameObject door = FindChildWithTag(wc, "DOOR");
        if (door != null) door.SetActive(true);
    }
    
    public void OpenDoor ()
    {
        GameObject door = FindChildWithTag(wc, "DOOR");
        if (door != null) door.SetActive(false);
    }

    private static GameObject FindChildWithTag(GameObject go, string tag)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).tag == tag)
                return go.transform.GetChild(i).gameObject;
        }
        return null;
    }
}
