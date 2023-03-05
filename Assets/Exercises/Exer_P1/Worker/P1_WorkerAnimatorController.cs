using System.Collections;
using Steerings;
using System.Collections.Generic;
using UnityEngine;

public class P1_WorkerAnimatorController : MonoBehaviour
{
    /** Variables */
    private Animator animator;

    /** SteeringContext */
    private SteeringContext context;

    // Unity Awake
    private void Awake() {
        /** Get Component */
        animator = GetComponent<Animator>();
        context = GetComponent<SteeringContext>();
    }

    // Unity Awake
    private void Update() {
        animator.SetFloat("velX", context.velocity.x);
        animator.SetFloat("velY", context.velocity.y);
    }
}
