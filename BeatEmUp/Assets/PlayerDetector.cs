using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    EnemyBehavior enemyScriptRb;
    [SerializeField] Transform player;
    [SerializeField] float walkEnemySpeed = 5f;

    bool isTracking = false;

    [SerializeField] CircleCollider2D detectCollider;


    // Start is called before the first frame update
    private void Start()
    {
        detectCollider = GetComponent<CircleCollider2D>();
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTracking = true;
            Vector2 direction = new Vector2(player.position.x - player.position.y, transform.position.x - transform.position.y).normalized;

         enemyScriptRb.rb2d.MovePosition(enemyScriptRb.rb2d.position + direction * Time.fixedDeltaTime * walkEnemySpeed);
            // enemyDet = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y) * walkEnemySpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTracking = false;
            // enemyDet = Vector2.zero;
        }
    }
}













