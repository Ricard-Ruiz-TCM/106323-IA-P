using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUI_TimeScale : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    public void OnButtonClick(int _scale)
    {
        Time.timeScale = _scale;
        textMeshPro.text = "x " + _scale;
    }
}
