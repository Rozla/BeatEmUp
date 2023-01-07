using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] public GameObject player;
    Vector2 dirToMove;
   
    Rigidbody2D rb;

    SpriteRenderer spriteEnemy;

    void Start()
    {
        //POSITION DE DEPART DE L'ENEMY
       Vector2 startPos = transform.position;

        rb = GetComponent<Rigidbody2D>();

        spriteEnemy = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
       if (player == null)
        {
            return;
        }

       //DIRECTION DE L'ENNEMY VERS LE PLAYER
        dirToMove = player.transform.position - transform.position;

        //APPLIQUE LA VITESSE AU RIGIDBODY
        rb.velocity = dirToMove.normalized * speed;

       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        }

    }
   

}
