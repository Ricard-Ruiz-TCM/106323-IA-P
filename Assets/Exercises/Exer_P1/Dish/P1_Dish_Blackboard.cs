using UnityEngine;

public enum DishState {
    clean, dirty, withFood
}

public class P1_Dish_Blackboard : MonoBehaviour {

    [Header("State:")]
    public DishState state = DishState.clean;

    [Header("Sprites:")]
    public Sprite dirtySprite;
    public Sprite cleanSprite;

    public void PlaceFoodOnDish() {
        ChangeState(DishState.withFood);
    }

    public void DirtyTheDish() {
        ChangeState(DishState.dirty);
    }

    public void WashUpDish() {
        ChangeState(DishState.clean);
    }

    private void ChangeState(DishState newState) {
        state = newState;
    }

}
