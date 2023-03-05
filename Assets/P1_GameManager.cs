using UnityEngine;
using UnityEngine.UI;

public class P1_GameManager : MonoBehaviour {

    [Header("Costs")]
    public int dishValue = 10;
    public GameObject dishPrefab;
    public Button buyDishButton;

    [Space(10)]
    public int assistantValue = 100;
    public GameObject assistantObject;
    public Button buyAssisButton;

    [Space(10)]
    public int holeLockValue = 50;
    public P1_Ant_GroupManager antGroupManager;

    public P1_Worker_Blackboard _player;

    // Unity Start
    private void Start() {
        _player.updateHUD();
    }

    public void BTN_BuyDish() {
        if (BuySomething(dishValue)) {
            Instantiate(dishPrefab).GetComponent<P1_DishController>().PlaceOn(GameObject.FindGameObjectWithTag("CLEAN_PILE"));
            buyDishButton.interactable = false;
        }
    }

    public void BTN_BuyAsistant() {
        if (BuySomething(assistantValue)) {
            assistantObject.SetActive(true);
            buyAssisButton.interactable = false;
        }
    }

    public void BTN_LockHole() {
        if (BuySomething(holeLockValue)) {
            antGroupManager.LockHole();
        }
    }

    private bool BuySomething(int value) {
        if (_player.money >= value) {
            _player.money -= value;
            _player.updateHUD();
            return true;
        }
        return false;
    }

}
