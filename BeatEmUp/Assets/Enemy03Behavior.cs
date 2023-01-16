using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy03Behavior : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float walkEnemySpeed = 5f;

    [SerializeField] Enemy03State currentState;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] GameObject graphics;
    [SerializeField] float playerDist = 1f;

    Coroutine attackCoroutine;

    public Transform player;

    public bool right, isOnRange;

    Vector2 enemy3Dir;

    public enum Enemy03State
    {
        IDLE,
        WALK,
        ATTACK1,
        HURT
    }
    // Start is called before the first frame update
    void Start()
    {
        currentState = Enemy03State.IDLE;
        OnStateEnter();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        OnStateUpdate();

        rb2d.velocity = enemy3Dir.normalized * walkEnemySpeed;

    }
    void OnStateEnter()
    {
        switch (currentState)
        {
            case Enemy03State.IDLE:
                animator.SetBool("IsIdle", true);
                break;
            case Enemy03State.WALK:
                animator.SetBool("IsWalking", true);
                break;
            case Enemy03State.ATTACK1:
                animator.SetTrigger("Attack1");
                break;
            case Enemy03State.HURT:
                animator.SetTrigger("AirHurt");
                break;
        }
                 


    }
    void OnStateUpdate()
    {
        switch (currentState)
        {
            case Enemy03State.IDLE:
                if (enemy3Dir != Vector2.zero)
                {
                    TransitionToState(Enemy03State.WALK);
                }
                break;
            case Enemy03State.WALK:
                if(enemy3Dir == Vector2.zero)
                {
                    TransitionToState(Enemy03State.IDLE);
                }

                break;
            case Enemy03State.ATTACK1:
                if (enemy3Dir == Vector2.zero)
                {
                    TransitionToState(Enemy03State.IDLE);
                }
                if (enemy3Dir != Vector2.zero)
                {
                    TransitionToState(Enemy03State.WALK);
                }
                break;
            case Enemy03State.HURT:
                if (enemy3Dir == Vector2.zero)
                {
                    TransitionToState(Enemy03State.IDLE);
                }
                if (enemy3Dir != Vector2.zero)
                {
                    TransitionToState(Enemy03State.WALK);
                }
                break;
        }

    }
    void OnStateExit()
    {
        switch (currentState)
        {
            case Enemy03State.IDLE:
                break;
            case Enemy03State.WALK:
                break;
            case Enemy03State.ATTACK1:
                break;
            case Enemy03State.HURT:
                break;
        }
    }
    void TransitionToState(Enemy03State nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }
}
