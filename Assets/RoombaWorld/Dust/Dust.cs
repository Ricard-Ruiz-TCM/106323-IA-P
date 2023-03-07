using UnityEngine;

public class Dust : MonoBehaviour {

    /** Variables */
    private SpriteRenderer _renderer;

    // Unity Awake
    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Unity Start
    private void Start() {
        ChangeColor(Random.ColorHSV());
        SetPosition(RandomLocationGenerator.RandomWalkableLocation());
    }

    /** Change Color */
    public void ChangeColor(Color c) {
        _renderer.color = c;
    }

    /** Set Position */
    public void SetPosition(Vector2 position) {
        transform.position = position;
    }

}
