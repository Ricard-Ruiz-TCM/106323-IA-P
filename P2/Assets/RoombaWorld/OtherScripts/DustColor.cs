using UnityEngine;

public class DustColor : MonoBehaviour {

    private SpriteRenderer spriteRenderer;

    // Unity Awake
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Unity Start
    private void Start() {
        spriteRenderer.color = Random.ColorHSV();
    }

}
