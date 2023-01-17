using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] GameObject record;

    [Header("State Machine")]
    [SerializeField] RuntimeAnimatorController standardController;
    [SerializeField] RuntimeAnimatorController holdCanController;
    [SerializeField] Animator animator;
    [SerializeField] PlayerState currentState;
    [SerializeField] GameObject shadow;
    [SerializeField] bool isHolding;
    Animator shadowAnimator;

    [Header("Basic Moves Settings")]
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] GameObject graphics;

    Vector2 dirInput;
    bool sprintInput;

    [HideInInspector]
    public bool right;




    [Header("Jump Settings")]
    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float jumpDuration = 3f;
    float jumpTimer;
    bool jumpInput;
    bool isJumping;


    [Header("Attack Settings")]
    [SerializeField] GameObject punchCollider;
    float attackSpeed = .1f;
    int attackCount;
    bool isAttacking;
    bool isResetting;


    [Header("Particles Settings")]
    [SerializeField] GameObject dustJumpParticles;
    [SerializeField] GameObject dustLandParticles;
    [SerializeField] GameObject dustSprintParticles;
    [SerializeField] GameObject punchParticles;
    ParticleSystemRenderer psrDustSprint;


    [Header("Hurt and Death Settings")]
    [SerializeField] public bool isHurt;
    [SerializeField] SpriteRenderer graphicsSR;
    public bool isDead;
    bool isInvincible;
    Vector2 dirHurt;

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
        JUMPLAND,
        HURT,
        DEAD
    }



    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.IDLE;
        psrDustSprint = dustSprintParticles.GetComponent<ParticleSystemRenderer>();
        shadowAnimator = shadow.GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {

        dirHurt = new Vector2(transform.position.x - 1f, transform.position.y);

        //FONCTION POUR RECUPERER LES INPUTS DU JOUEUR
        GetInputs();

        OnStateUpdate();

        //FONCTION DE SAUT

        Jump();

        AttackCombo();

        if (Input.GetKeyDown("w"))
        {
            Instantiate(record, transform.position, transform.rotation);
        }

        if (isDead)
        {
            StartCoroutine(LoseLife());
        }

        if(isHurt)
        {
            StartCoroutine(IsHurt());
        }

        
        
    }

    private void AttackCombo()
    {
        if (isAttacking && !isHolding)
        {
            animator.SetInteger("ATTACKCOUNT", attackCount);

            if (attackCount == 4)
            {
                attackCount = 0;
            }
        }
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

                StartCoroutine(DustSprintParticles());

                break;
            case PlayerState.ATTACK1:

                if (!isHolding)
                {

                    punchCollider.gameObject.SetActive(true);

                    if (!isResetting)
                    {
                        isResetting = true;
                        StartCoroutine(AttackReset());
                    }
                    StartCoroutine(AttackCD());
                }

                if (isHolding)
                {
                    StartCoroutine(ThrowTimer());
                }


                break;
            case PlayerState.JUMPUP:

                animator.SetTrigger("JUMP");
                shadowAnimator.SetTrigger("JUMP");
                dustJumpParticles.gameObject.SetActive(true);
                //LE PLAYER EST EN SAUT
                isJumping = true;



                break;
            case PlayerState.JUMPMAX:
                break;
            case PlayerState.JUMPDOWN:
                break;
            case PlayerState.JUMPLAND:

                break;
            case PlayerState.HURT:

                animator.SetTrigger("HURT");
                punchParticles.gameObject.SetActive(true);
                break;
            case PlayerState.DEAD:

                animator.SetTrigger("DEAD");

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

                if (isAttacking)
                {
                    TransitionToState(PlayerState.ATTACK1);
                }

                if (jumpInput && !isJumping)
                {
                    TransitionToState(PlayerState.JUMPUP);
                }

                break;
            case PlayerState.WALK:

                rb2d.velocity = dirInput.normalized * walkSpeed;

                if (sprintInput)
                {
                    TransitionToState(PlayerState.RUN);
                }

                if (dirInput == Vector2.zero)
                {
                    TransitionToState(PlayerState.IDLE);
                }

                if (isAttacking)
                {
                    TransitionToState(PlayerState.ATTACK1);
                }

                if (jumpInput && !isJumping)
                {
                    TransitionToState(PlayerState.JUMPUP);
                }

                break;
            case PlayerState.RUN:


                rb2d.velocity = dirInput.normalized * sprintSpeed;

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

                rb2d.velocity = dirInput.normalized * attackSpeed;

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
                    dustJumpParticles.gameObject.SetActive(false);
                    StartCoroutine(DustLandParticles());
                    TransitionToState(PlayerState.WALK);
                }

                if (dirInput == Vector2.zero && jumpTimer > jumpDuration)
                {
                    jumpTimer = 0f;
                    isJumping = false;
                    dustJumpParticles.gameObject.SetActive(false);
                    StartCoroutine(DustLandParticles());
                    TransitionToState(PlayerState.IDLE);
                }

                break;
            case PlayerState.HURT:
                
                break;
            case PlayerState.DEAD:

                break;

            default:
                break;
        }
    }

    IEnumerator LoseLife()
    {
        rb2d.velocity = Vector2.zero;
        isDead = false;
        isInvincible = true;
        TransitionToState(PlayerState.DEAD);
        yield return new WaitForSeconds(1f);
        graphicsSR.gameObject.SetActive(false);
        shadow.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        graphicsSR.gameObject.SetActive(true);
        shadow.gameObject.SetActive(true);

        if (GetComponent<PlayerHealth>().lifeCount < 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            isInvincible = false;
            TransitionToState(PlayerState.IDLE);
        }
    }
    IEnumerator IsHurt()
    {
        rb2d.velocity = Vector2.zero;
        isHurt = false;
        isInvincible = true;
        TransitionToState(PlayerState.HURT);
        graphicsSR.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(1f);
        rb2d.AddForce(dirHurt, ForceMode2D.Impulse);
        graphicsSR.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        isInvincible = false;
        punchParticles.gameObject.SetActive(false);
        TransitionToState(PlayerState.IDLE);
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

                isAttacking = false;
                break;
            case PlayerState.JUMPUP:

                break;
            case PlayerState.JUMPMAX:
                break;
            case PlayerState.JUMPDOWN:

                break;
            case PlayerState.JUMPLAND:
                break;
            case PlayerState.HURT:
                graphicsSR.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
                break;
            case PlayerState.DEAD:
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
        if (!isJumping && dirInput != Vector2.zero)
        {
            //BOOL WALK DE L'ANIMATOR PASSE VRAI
            animator.SetBool("WALK", true);
        }

        //SI LE PLAYER N'EST PAS EN MOUVEMENT
        if (dirInput.x != 0 && !isInvincible)
        {
            //LA BOOL RIGHT EST VRAI SI LA DIRECTION EN X EST EGALE A ZERO
            right = dirInput.x > 0;

            //ON MODIFIE LA ROTATION EN Y DES GRAPHICS DU PLAYER
            graphics.transform.rotation = right ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);

            psrDustSprint.flip = right ? new Vector3(0, 0, 0) : new Vector3(1, 0, 0);
        }


        sprintInput = Input.GetButton("Sprint");
        animator.SetBool("SPRINT", sprintInput);


        if (Input.GetButtonDown("Attack1") && !sprintInput)
        {
            isAttacking = true;
            attackCount += 1;
            animator.SetTrigger("ATTACK1");
        }


        jumpInput = Input.GetButtonDown("Jump");




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


    IEnumerator DustLandParticles()
    {
        dustLandParticles.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        dustLandParticles.gameObject.SetActive(false);
    }

    IEnumerator DustSprintParticles()
    {
        dustSprintParticles.GetComponent<ParticleSystemRenderer>();
        dustSprintParticles.gameObject.SetActive(true);
        yield return new WaitForSeconds(.3f);
        dustSprintParticles.gameObject.SetActive(false);
    }


    IEnumerator AttackReset()
    {
        float t = 0;
        float attackDuration = 1.5f;

        while (t < attackDuration)
        {
            t += Time.deltaTime;

            if (Input.GetButtonDown("Attack1"))
            {
                t = 0;
            }

            yield return null;
        }

        attackCount = 0;
        isResetting = false;

    }
    IEnumerator AttackCD()
    {
        yield return new WaitForSeconds(.3f);

        //TO IDLE
        if (dirInput == Vector2.zero)
        {
            punchCollider.gameObject.SetActive(false);
            TransitionToState(PlayerState.IDLE);
        }

        //TO WALK
        if (dirInput != Vector2.zero)
        {
            punchCollider.gameObject.SetActive(false);
            TransitionToState(PlayerState.WALK);
        }
    }


    IEnumerator ThrowTimer()
    {
        yield return new WaitForSeconds(0.3f);
        isHolding = false;
        TransitionToState(PlayerState.IDLE);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EnemyDamageTest" && !isInvincible)
        {
            GetComponent<PlayerHealth>().TakeDamage();
        }
    }
}

