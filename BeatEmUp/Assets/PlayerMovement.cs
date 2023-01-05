using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] Animator animator;
    [SerializeField] PlayerState currentState;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] GameObject graphics;

    float speed;

    Vector2 dirInput;

    bool sprintInput;

    [HideInInspector]
    public bool right;

    public enum PlayerState
    {
        IDLE,
        WALK,
        RUN
    }


    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();

        OnStateUpdate();
    }

    void OnStateEnter()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:
                animator.SetBool("WALK", false);
                rb2d.velocity = Vector2.zero;
                break;
            case PlayerState.WALK:
               
                break;
            case PlayerState.RUN:
                break;
            default:
                break;
        }
    }

    void OnStateUpdate()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:

                if(dirInput != Vector2.zero && !sprintInput)
                {
                    TransitionToState(PlayerState.WALK);
                }

                if (dirInput != Vector2.zero && sprintInput)
                {
                    TransitionToState(PlayerState.RUN);
                }

                break;
            case PlayerState.WALK:

                speed = walkSpeed;

                if (sprintInput)
                {
                    TransitionToState(PlayerState.RUN);
                }

                if (dirInput == Vector2.zero)
                {
                    TransitionToState(PlayerState.IDLE);
                }

                break;
            case PlayerState.RUN:

                speed = sprintSpeed;

                if(!sprintInput)
                {
                    TransitionToState(PlayerState.WALK);
                }

                if(dirInput == Vector2.zero)
                {
                    TransitionToState(PlayerState.IDLE);
                }

                break;
            default:
                break;
        }
    }

    void OnStateExit()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.WALK:
                break;
            case PlayerState.RUN:
                break;
            default:
                break;
        }
    }

    void TransitionToState(PlayerState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();

    }

    void GetInputs()
    {
        dirInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (dirInput != Vector2.zero)
        {
            animator.SetBool("WALK", true);
        }

        if (dirInput.x != 0)
        {
            right = dirInput.x > 0;
            graphics.transform.rotation = right ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);
        }

        sprintInput = Input.GetButton("Sprint");
        animator.SetBool("SPRINT", sprintInput);

    }

    private void FixedUpdate()
    {
        rb2d.velocity = dirInput.normalized * speed;
    }
}
