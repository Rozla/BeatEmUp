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

    float attackDuration = .35f;
    float attackTimer;
    int attackCount;
    float attackCooldown;

    float speed;

    Vector2 dirInput;


    bool sprintInput;
    bool attack1Input;
    bool jumpInput;

    bool isJumping;
    bool isHolding;
    bool isOnAttack;


    [HideInInspector]
    public bool right;


    public enum PlayerState
    {
        IDLE,
        WALK,
        RUN,
        ATTACK1,
        ATTACK2,
        ATTACK3,
        ATTACK4,
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



        //FONCTION POUR RECUPERER LES INPUTS DU JOUEUR
        GetInputs();

        OnStateUpdate();

        //FONCTION DE SAUT

        Jump();


        attackCooldown += Time.deltaTime;


        


    }

    private void Jump()
    {


        //SI LE TIMER DE SAUT EST INFERIEUR AU TIMER DE DUREE, ET QUE LE JOUEUR EST EN COURS DE SAUT
        if (jumpTimer < jumpDuration && isJumping)
        {

            //ON INCREMENTE LE TIMER DE SAUT
            jumpTimer += Time.deltaTime;

            //ON DEFINI Y SUR L'ANIMATION CURVE EN FONCTION DU TEMPS DE SAUT SUR LA DUREE MAX  (PROGRESSION/MAX)
            float y = jumpCurve.Evaluate(jumpTimer / jumpDuration);

            //MODIFICATION DE LA POSITION DE GRAPHICS, EN FONCTION DE LA POSITION DE (Y*HAUTEUR DE SAUT) SUR L'ANIMATION CURVE
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

                //SI LE JOUEUR NE TIENT PAS D'OBJET, ON PASSE SUR LES ANIMATIONS SANS OBJET
                if (!isHolding)
                {
                    animator.runtimeAnimatorController = standardController as RuntimeAnimatorController;
                }

                break;
            case PlayerState.WALK:

                //SI LE JOUEUR TIENT UN OBJET, ON PASSE SUR LES ANIMATIONS AVEC OBJET
                if (!isHolding)
                {
                    animator.runtimeAnimatorController = standardController as RuntimeAnimatorController;
                }

                break;
            case PlayerState.RUN:
                break;
            case PlayerState.ATTACK1:

                if (attackCooldown <= 1f && attackCount < 3)
                {
                    attackCooldown = 0f;
                    attackCount += 1;
                }

                if(attackCooldown > .8f || attackCount > 3)
                {
                    attackCooldown = 0f;
                    attackCount = 0;
                }

                
                animator.SetTrigger("ATTACK1");
                animator.SetInteger("ATTACKCOUNT", attackCount);
                
                break;
            case PlayerState.JUMPUP:

                animator.SetTrigger("JUMP");
                //LE PLAYER EST EN SAUT
                isJumping = true;



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

                if (jumpInput && !isJumping)
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

                if (jumpInput && !isJumping)
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

                if (dirInput != Vector2.zero && !isHolding)
                {
                    TransitionToState(PlayerState.WALK);
                }

                if (dirInput == Vector2.zero && !isHolding)
                {
                    TransitionToState(PlayerState.IDLE);
                }

                //SI LE JOUEUR EST EN MOUVEMENT ET QU'IL TIENT UN OBJET
                if (isHolding && dirInput != Vector2.zero)
                {
                    attackTimer += Time.deltaTime;

                    //TIMER POUR ATTENDRE QUE L'ANIMATION TERMINE AVANT DE PASSER AU PROCHAIN ETAT
                    if (attackTimer > attackDuration)
                    {
                        //IL LANCE L'OBJET DONC ON RETOURNE SUR LES ANIMATIONS SANS OBJET
                        animator.runtimeAnimatorController = standardController as RuntimeAnimatorController;

                        //ON DIT QU'IL NE TIENT PLUS D'OBJET ET ON REPASSE EN WALK
                        isHolding = false;
                        attackTimer = 0;
                        TransitionToState(PlayerState.WALK);
                    }
                }


                //SI LE JOUEUR N'EST PAS EN MOUVEMENT ET QU'IL TIENT UN OBJET
                if (isHolding && dirInput == Vector2.zero)
                {
                    attackTimer += Time.deltaTime;

                    //TIMER POUR ATTENDRE QUE L'ANIMATION TERMINE AVANT DE PASSER AU PROCHAIN ETAT
                    if (attackTimer > attackDuration)
                    {
                        //IL LANCE L'OBJET DONC ON RETOURNE SUR LES ANIMATIONS SANS OBJET
                        animator.runtimeAnimatorController = standardController as RuntimeAnimatorController;

                        //ON DIT QU'IL NE TIENT PLUS D'OBJET ET ON REPASSE EN WALK
                        isHolding = false;
                        attackTimer = 0;
                        TransitionToState(PlayerState.WALK);
                    }
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

                if (dirInput != Vector2.zero && jumpTimer > jumpDuration)
                {
                    jumpTimer = 0f;
                    isJumping = false;
                    TransitionToState(PlayerState.WALK);
                }

                if (dirInput == Vector2.zero && jumpTimer > jumpDuration)
                {
                    jumpTimer = 0f;
                    isJumping = false;
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

        //ON RECUPERE LES INPUTS HORIZONTAL ET VERTICAL
        dirInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //SI LE PLAYER EST EN MOUVEMENT
        if (dirInput != Vector2.zero)
        {
            //BOOL WALK DE L'ANIMATOR PASSE VRAI
            animator.SetBool("WALK", true);
        }

        //SI LE PLAYER N'EST PAS EN MOUVEMENT
        if (dirInput.x != 0)
        {
            //LA BOOL RIGHT EST VRAI SI LA DIRECTION EN X EST EGALE A ZERO
            right = dirInput.x > 0;

            //ON MODIFIE LA ROTATION EN Y DES GRAPHICS DU PLAYER
            graphics.transform.rotation = right ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);
        }


        sprintInput = Input.GetButton("Sprint");
        animator.SetBool("SPRINT", sprintInput);


        attack1Input = Input.GetButtonDown("Attack1");


        jumpInput = Input.GetButtonDown("Jump");




    }

    private void FixedUpdate()
    {
        //SI L'ATTAQUE N'EST PAS EN COURS, ON PEUT BOUGER LE PLAYER ------------------------------- NE FONCTIONNE PAS !!!!!
        if (attackTimer >= 0)
        {
            rb2d.velocity = dirInput.normalized * speed;

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //QUAND ON EST DANS LE COLLIDER2D D'UN OBJET ET QU'ON APPUIE SUR LA TOUCHE POUR RAMASSER
        if (collision.gameObject.tag == "PICKUP" && Input.GetButtonDown("PickUp"))
        {
            //ON PRECISE QU'ON TIENT UN OBJET
            isHolding = true;

            //ON PASSE SUR LES ANIMATIONS AVEC OBJET
            animator.runtimeAnimatorController = holdCanController as RuntimeAnimatorController;

            //ON PASSE L'OBJET EN ENFANT DU PLAYER
            collision.transform.SetParent(graphics.transform);

            //ON LUI DONNE UNE NOUVELLE POSITION - SUR LA TETE DU PLAYER
            collision.transform.position = new Vector3(graphics.transform.position.x, graphics.transform.position.y + 1.2f, graphics.transform.position.z);

            //ON PRECISE DANS L'OBJET QU'IL EST ACTUELLEMENT DANS LES MAINS DE QUELQU'UN (PLAYER OU ENNEMY)
            collision.GetComponent<CanBehavior>().isHolded = true;
        }
    }
}
