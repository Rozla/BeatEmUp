using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController standardController;
    [SerializeField] RuntimeAnimatorController holdCanController;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] Animator animator;
    [SerializeField] PlayerState currentState;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] GameObject graphics;


    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float jumpDuration = 3f;
    float jumpTimer;


    float speed;

    Vector2 dirInput;



    bool sprintInput;
    bool attack1Input;
    bool jumpInput;
    [SerializeField] bool isJumping;

    [HideInInspector]
    public bool right;

    public enum PlayerState
    {
        IDLE,
        WALK,
        RUN,
        ATTACK1,
        JUMPUP,
        JUMPMAX,
        JUMPDOWN,
        JUMPLAND
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

        Jump();

        if(jumpTimer <= 0f)
        {
            isJumping = false;
        }

        // -------------------- Phase de test changement d'Animator Controller ----------------//
        if(Input.GetKeyDown("p")) // à mettre quand on attrape une canette/objet à lancer
        {
            animator.runtimeAnimatorController = standardController as RuntimeAnimatorController;
        }

        if(Input.GetKeyDown("m")) // à mettre lorsque l'on jette l'objet. Sur la sortie de l'état ATTACK1 ?
        {
            animator.runtimeAnimatorController = holdCanController as RuntimeAnimatorController;
        }
        // -------------------- Phase de test changement d'Animator Controller ----------------//



    }

    private void Jump()
    {
        if (jumpTimer < jumpDuration && isJumping)
        {
            jumpTimer += Time.deltaTime;

            float y = jumpCurve.Evaluate(jumpTimer / jumpDuration);

            Debug.Log(y);

            graphics.transform.localPosition = new Vector3(graphics.transform.localPosition.x, y * jumpHeight, graphics.transform.localPosition.z);
        }

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
            case PlayerState.ATTACK1:
                animator.SetTrigger("ATTACK1");
                break;
            case PlayerState.JUMPUP:

                isJumping = true;
                animator.SetTrigger("JUMP");

                break;
            case PlayerState.JUMPMAX:
                break;
            case PlayerState.JUMPDOWN:
                break;
            case PlayerState.JUMPLAND:
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

                
                if (dirInput != Vector2.zero && !sprintInput)
                {
                    TransitionToState(PlayerState.WALK);
                }

                if (dirInput != Vector2.zero && sprintInput)
                {
                    TransitionToState(PlayerState.RUN);
                }

                if (attack1Input)
                {
                    TransitionToState(PlayerState.ATTACK1);
                }

                if (jumpInput)
                {
                    TransitionToState(PlayerState.JUMPUP);
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

                if (attack1Input)
                {
                    TransitionToState(PlayerState.ATTACK1);
                }

                if (jumpInput)
                {
                    TransitionToState(PlayerState.JUMPUP);
                }


                break;
            case PlayerState.RUN:
                
                speed = sprintSpeed;

                if (!sprintInput)
                {
                    TransitionToState(PlayerState.WALK);
                }

                if (dirInput == Vector2.zero)
                {
                    TransitionToState(PlayerState.IDLE);
                }

                break;

            case PlayerState.ATTACK1:

                if (dirInput != Vector2.zero)
                {
                    TransitionToState(PlayerState.WALK);
                }

                if (dirInput == Vector2.zero)
                {
                    TransitionToState(PlayerState.IDLE);
                }


                break;

            case PlayerState.JUMPUP:

                TransitionToState(PlayerState.JUMPMAX);

                break;
            case PlayerState.JUMPMAX:

                TransitionToState(PlayerState.JUMPDOWN);

                break;
            case PlayerState.JUMPDOWN:

                TransitionToState(PlayerState.JUMPLAND);

                break;
            case PlayerState.JUMPLAND:

                if (dirInput != Vector2.zero)
                {
                    jumpTimer = 0f;
                    TransitionToState(PlayerState.WALK);
                }

                if (dirInput == Vector2.zero)
                {
                    jumpTimer = 0f;
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
            case PlayerState.ATTACK1:
                break;
            case PlayerState.JUMPUP:
                break;
            case PlayerState.JUMPMAX:
                break;
            case PlayerState.JUMPDOWN:
                break;
            case PlayerState.JUMPLAND:
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

        attack1Input = Input.GetButtonDown("Attack1");


        jumpInput = Input.GetButtonDown("Jump");
        

    }

    private void FixedUpdate()
    {
        rb2d.velocity = dirInput.normalized * speed;
    }

}
