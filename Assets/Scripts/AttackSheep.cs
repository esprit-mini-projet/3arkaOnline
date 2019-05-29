using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackSheep : MonoBehaviour
{
    private NavMeshAgent mAgent;

    private Animator mAnimator;

    public GameObject Player;

    public float EnemyDistanceRun = 4.0f;

    private bool mIsDead = false;

    public GameObject[] ItemsDeadState = null;

    public bool isAttacking;

    // Use this for initialization
    void Start()
    {
        mAgent = GetComponent<NavMeshAgent>();

        mAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if(isAttacking)
        {
            mAnimator.SetBool("walk", true);
            mAnimator.SetTrigger("walk");
        }
    }
}
