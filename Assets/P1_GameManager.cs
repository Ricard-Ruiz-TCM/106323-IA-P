using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P1_GameManager : MonoBehaviour
{

    [Foldout("Costs", styled = true)]
    public int dishValue = 10;
    public GameObject dishPrefab;

    public P1_Worker_Blackboard _player;

    public void BTN_BuyDish() {
        if (_player.money >= dishValue) {
            _player.money -= dishValue;
            Instantiate(dishPrefab, GameObject.Find("DishContainer").transform);
        }
    }

}
