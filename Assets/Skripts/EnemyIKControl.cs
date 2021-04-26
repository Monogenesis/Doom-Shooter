using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIKControl : MonoBehaviour
{
    protected Animator animator;

    public Transform leftHand;
    public Transform rightHand;
    public Transform lookObj;

    void Start()
    {
        animator = GetComponent<Animator>();
        lookObj = Camera.main.transform;
    }

    private void OnAnimatorIK()
    {


        if (animator)
        {
            if (lookObj != null)
            {
                animator.SetLookAtWeight(1);
                animator.SetLookAtPosition(lookObj.position);
                float distanceToPlayer = Vector3.Distance(transform.position, lookObj.position);
                if (distanceToPlayer < 8f && animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
                }
                else
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);

                }
            }
        }
    }
    void Update()
    {

    }
}
