using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUI_TimeScale : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    public void OnButtonClick(float _scale)
    {
        Time.timeScale = _scale;
        textMeshPro.text = "Speed x" + _scale;
    }
}
