using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement: MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float walkSpeed = 5f;  
    [SerializeField] EnemyState currentState;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] GameObject graphics;
    public bool right;
    Vector2 enemyDir;
    public enum EnemyState
    {
        IDLE,
        WALK
        
        
    }

   
   


    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.IDLE;
        OnStateEnter();
    }

    // Update is called once per frame
    void Update()
    {
        

        GetInputs();
        OnStateUpdate();


    }

    private void GetInputs()
    {
        // RECUPERER LES INPUTS 

        enemyDir = new Vector2(Input.GetAxisRaw("HorizontalEnemy"), Input.GetAxisRaw("VerticalEnemy"));

        // SEULEMENT SI J'APPUI SUR UNE DIRECTION
        if (enemyDir.magnitude != 0)
        {
               animator.SetBool("WALK_ENEMY01",true);
            
        }

        if (enemyDir.x != 0)
        {
            right = enemyDir.x > 0;
            graphics.transform.rotation = right ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);
        }
    }

    void OnStateEnter()
    {
        switch (currentState)
        {
            case EnemyState.IDLE:
                animator.SetBool("WALK_ENEMY01", false);
                rb2d.velocity = Vector2.zero;
                break;
            case EnemyState.WALK:
                break;

        
               
            default:
                break;
        }
    }
    void OnStateUpdate()
    {
        switch (currentState)
        {
            case EnemyState.IDLE:
                // TO WALK
                if (enemyDir != Vector2.zero )
                {
                    TransitionToState(EnemyState.WALK);
                }
              
                break;
            case EnemyState.WALK:
                rb2d.velocity = enemyDir.normalized * walkSpeed;
               
                // TO IDLE
                if (enemyDir == Vector2.zero)
                {
                    TransitionToState(EnemyState.IDLE);
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
            case EnemyState.IDLE:
                break;
            case EnemyState.WALK:
                break;
           
               
        }
    }
    void TransitionToState(EnemyState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }
}

