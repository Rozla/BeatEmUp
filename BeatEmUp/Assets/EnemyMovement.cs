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
        // Récupère le composant Animator de l'ennemi
        animator = GetComponent<Animator>();

        // Récupère le script de contrôle du joueur
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        // Initialise le timer d'attaque à la durée d'attaque
        attackTimer = attackDuration;
    }

    void Update()
    {
        // Mise à jour du timer d'attaque
        attackTimer -= Time.deltaTime;

        // Si l'ennemi a des points de vie inférieurs à 0, il meurt
        if (health <= 0)
        {
            // Fait apparaître des objets qui augmentent le score du joueur
            for (int i = 0; i < dropCount; i++)
            {
                Instantiate(dropObject, transform.position, Quaternion.identity);
            }

            // Détruit l'ennemi
            Destroy(gameObject);
        }

        // Si le timer d'attaque est écoulé, attaque le joueur
        if (attackTimer <= 0)
        {
            // Réinitialise le timer d'attaque
            attackTimer = attackDuration;

            // Déclenche l'animation d'attaque
            animator.SetTrigger("Attack");

            // Attaque le joueur
           // playerScript.TakeDamage();
        }

        // Si le joueur est à proximité, se dirige vers lui
        if (Vector2.Distance(transform.position, playerScript.transform.position) < 10f)
        {
            // Calcul la direction du joueur
            Vector2 direction = (playerScript.transform.position - transform.position).normalized;

            // Si le joueur est derrière l'ennemi, retourne l'animation de l'ennemi
            if (direction.x < 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x > 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

            // Déplace l'ennemi dans la direction du joueur
            //controller.move(direction * speed * Time.deltaTime, false);

            // Met à jour l'animation de déplacement en fonction de la direction
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

    // Fonction appelée lorsque l'ennemi prend un coup
    public void TakeDamage()
    {
        // Diminue les points de vie de l'ennemi
        health--;

        // Déclenche l'animation de dommage
        animator.SetTrigger("HURT");
    }
}

