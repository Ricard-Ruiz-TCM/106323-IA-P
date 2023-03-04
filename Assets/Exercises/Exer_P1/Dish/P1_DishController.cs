using UnityEngine;

public enum DishState {
    clean, dirty, withFood, picked }

public class P1_DishController : MonoBehaviour {

    [Header("State:")]
    public DishState state = DishState.clean;

    [Header("Sprites:")]
    public Sprite dirtySprite;
    public Sprite cleanSprite;

    public void PlaceFood() {
        changeState(DishState.withFood); }

    public void Dirty() {
        changeState(DishState.dirty); }

    public void Wash() {
        changeState(DishState.clean); }

    public void Pick() {
        changeState(DishState.picked); }

    private void changeState(DishState newState) {
        state = newState; }

    public void PlaceOn(Transform parent) {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }

    public void PlaceOn(GameObject parent) { 
        PlaceOn(parent.transform);
    }

}
