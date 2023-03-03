using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P1_GameManager : MonoBehaviour
{

    [Header("Costs")]
    public int dishValue = 10;
    public GameObject dishPrefab;

    [Space(10)]
    public int assistantValue = 100;
    public GameObject assistantObject;
    public Button buyAssisButton;

    public P1_Worker_Blackboard _player;

    public void BTN_BuyDish() {
        if (BuySomething(dishValue)) {
            GameObject dish = Instantiate(dishPrefab, GameObject.FindGameObjectWithTag("CLEAN_PILE").transform);
            dish.transform.localPosition = Vector3.zero;
            dish.transform.localEulerAngles = Vector3.zero;
        }
    }

    public void BTN_BuyAsistant() {
        if (assistantObject.activeSelf) 
            return;

        if (BuySomething(assistantValue)) {
            assistantObject.SetActive(true);
            buyAssisButton.interactable = false;
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
