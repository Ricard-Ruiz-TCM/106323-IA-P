
using UnityEngine;

public class RingControl : MonoBehaviour
{

    public GameObject what;
  
    void Update()
    {
    if (what != null)
       if (Input.GetKeyDown(KeyCode.R))
            what.SetActive(!what.activeSelf);
    }
}
