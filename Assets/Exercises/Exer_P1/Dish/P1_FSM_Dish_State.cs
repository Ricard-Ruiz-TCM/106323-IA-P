using FSMs;
using UnityEngine;

[CreateAssetMenu(fileName = "P1_FSM_Dish_State", menuName = "Finite State Machines/P1_FSM_Dish_State", order = 1)]
public class P1_FSM_Dish_State : FiniteStateMachine {

    /** Variables */
    private SpriteRenderer renderer;
    private P1_Dish_Blackboard blackboard;

    /** OnEnter */
    public override void OnEnter() {

        /** GetComponent */
        renderer = GetComponent<SpriteRenderer>();
        blackboard = GetComponent<P1_Dish_Blackboard>();

        /** OnEnter */
        base.OnEnter();
    }

    /** OnExit */
    public override void OnExit() {

        /** DisableSteerings */
        base.DisableAllSteerings();

        /** OnExit */
        base.OnExit();
    }

    public override void OnConstruction() {

        /** States */
        State clean = new State("clean",
            () => {
                gameObject.tag = "DISH_CLEAN";
                renderer.sprite = blackboard.cleanSprite;
            },
            () => { },
            () => { });

        State withFood = new State("withFood",
            () => {
                gameObject.tag = "DISH_IN_USE";
                renderer.sprite = blackboard.dirtySprite;
            },
            () => { },
            () => { });

        State dirty = new State("dirty",
            () => {
                gameObject.tag = "DISH_DIRTY";
            },
            () => { },
            () => { });

        /** Transitions */
        Transition some1UsedMe = new Transition("reachedFoodMachine",
            () => {
                return blackboard.state.Equals(DishState.withFood);
            }, () => { });

        Transition some1EatAll = new Transition("reachedFoodMachine",
            () => {
                return blackboard.state.Equals(DishState.dirty);
            }, () => { });

        Transition some1CleanedMe = new Transition("foodBuyed",
            () => {
                return blackboard.state.Equals(DishState.clean);
            }, () => { });


        /** FSM Set Up */
        AddStates(clean, withFood, dirty);

        AddTransition(clean, some1UsedMe, withFood);
        AddTransition(withFood, some1EatAll, dirty);
        AddTransition(dirty, some1CleanedMe, clean);

        initialState = clean;
    }

    public void CleanOnEnter() {

    }

}