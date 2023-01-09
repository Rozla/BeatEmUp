using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{

    public Sprite[] sprites;
    [SerializeField] float speed;
    [SerializeField] float health;
    [SerializeField] int dropCount;
    GameObject dropObject;
    Vector2 dirEnemy;
    PlayerMovement playerScript;
   [SerializeField] Animator animator;
    float attackTimer;
    float attackDuration;

    void Start()
    {
        // R�cup�re le composant Animator de l'ennemi
        animator = GetComponent<Animator>();

        // R�cup�re le script de contr�le du joueur
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        // Initialise le timer d'attaque � la dur�e d'attaque
        attackTimer = attackDuration;
    }

    void Update()
    {
        // Mise � jour du timer d'attaque
        attackTimer -= Time.deltaTime;

        // Si l'ennemi a des points de vie inf�rieurs � 0, il meurt
        if (health <= 0)
        {
            // Fait appara�tre des objets qui augmentent le score du joueur
            for (int i = 0; i < dropCount; i++)
            {
                Instantiate(dropObject, transform.position, Quaternion.identity);
            }

            // D�truit l'ennemi
            Destroy(gameObject);
        }

        // Si le timer d'attaque est �coul�, attaque le joueur
        if (attackTimer <= 0)
        {
            // R�initialise le timer d'attaque
            attackTimer = attackDuration;

            // D�clenche l'animation d'attaque
            animator.SetTrigger("Attack");

            // Attaque le joueur
           // playerScript.TakeDamage();
        }

        // Si le joueur est � proximit�, se dirige vers lui
        if (Vector2.Distance(transform.position, playerScript.transform.position) < 10f)
        {
            // Calcul la direction du joueur
            Vector2 direction = (playerScript.transform.position - transform.position).normalized;

            // Si le joueur est derri�re l'ennemi, retourne l'animation de l'ennemi
            if (direction.x < 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x > 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

            // D�place l'ennemi dans la direction du joueur
            //controller.move(direction * speed * Time.deltaTime, false);

            // Met � jour l'animation de d�placement en fonction de la direction
            if (direction.x > 0)
            {
                animator.SetTrigger("WALK");
            }
            else if (direction.x < 0)
            {
                animator.SetTrigger("WALK");
            }
            else if (direction.y > 0)
            {
                animator.SetTrigger("WALK");
            }
            else if (direction.y < 0)
            {
                animator.SetTrigger("WALK");
            }
        }
    }

    // Fonction appel�e lorsque l'ennemi prend un coup
    public void TakeDamage()
    {
        // Diminue les points de vie de l'ennemi
        health--;

        // D�clenche l'animation de dommage
        animator.SetTrigger("HURT");
    }
}

