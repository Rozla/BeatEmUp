using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBehavior : MonoBehaviour
{


    Rigidbody2D rb2d;
    BoxCollider2D bc2d;

    float throwHeight = 1f;
    float throwDistance = 5f;

    float t;
    float rotationFix;

    bool inMotion;

    [SerializeField] float isRight;

    //[HideInInspector]
    public bool isHolded;


    Vector2 stopPosition;

    // Start is called before the first frame update
    void Start()
    {

        //ON RECUPERE LE RIGIDBODY DE L'OBJET
        rb2d = GetComponent<Rigidbody2D>();

        //ON RECUPERE LE COLLIDER2D DE L'OBJET
        bc2d = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {

        //SI L'OBJET EST TENU ET QU'IL A UN PARENT
        if (isHolded && transform.parent != null)
        {

            //SON ORIENTATION EST EGALE A CELLE DU PARENT
            isRight = transform.parent.rotation.y;
        }


        //FONCTION POUR FIXER LA DIRECTION DU LANCER
        if(isRight == 1)
        {
            //POUR ENVOYER A GAUCHE
            rotationFix = -1f;
        }

        if (isRight == 0)
        {
            //POUR ENVOYER A DROITE
            rotationFix = 1f;
        }


        //FONCTION POUR AJOUTER UNE FORCE QUAND ON JETE L'OBJET
        Throw();

        //SI L'OBJET EST EN MOUVEMENT ET QU'IL PASSE SOUS LES PIEDS DU PLAYER AU MOMENT DU LANCER
        if (inMotion && transform.position.y < stopPosition.y)
        {
            
            t = 0f;
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            rb2d.velocity = Vector2.zero;
            //bc2d.isTrigger = false;
            isHolded = false;

        }

    }

    private void Throw()
    {

        t += Time.deltaTime;

        if (isHolded && Input.GetButtonDown("Attack1"))
        {
            t = 0f;
            stopPosition = new Vector2(transform.parent.position.x, transform.parent.position.y);

            
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            float throwForce = Mathf.Sqrt(-2 * Physics2D.gravity.y * throwHeight * rb2d.mass);
            rb2d.AddForce(new Vector2(throwDistance * rotationFix, throwForce), ForceMode2D.Impulse);
            transform.parent = null;
            inMotion = true;
        }
    }
}