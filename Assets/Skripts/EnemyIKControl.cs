using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIKControl : MonoBehaviour
{
    protected Animator animator;

    public Transform leftHand;
    public Transform rightHand;
    public Transform lookObj;
    float rightHandWeight;
    float leftHandWeight;

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
                    rightHandWeight = Mathf.Lerp(rightHandWeight, 1, Time.deltaTime);
                    leftHandWeight = Mathf.Lerp(leftHandWeight, 1, Time.deltaTime);
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
                }
                else
                {
                    rightHandWeight = Mathf.Lerp(rightHandWeight, 0, Time.deltaTime);
                    leftHandWeight = Mathf.Lerp(leftHandWeight, 0, Time.deltaTime);
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);

                }
            }
        }
    }
    void Update()
    {

    }
}
