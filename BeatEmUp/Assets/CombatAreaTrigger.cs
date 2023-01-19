using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAreaTrigger : MonoBehaviour
{

    [SerializeField] GameObject cameraPlayer;

    [SerializeField] bool enemyNotDetected;
    bool canOverlap;

    Vector2 boxSize;

    [SerializeField] float sizeX = 5f;
    [SerializeField] float sizeY = 10f;
    [SerializeField] LayerMask enemyLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        boxSize.x = sizeX;
        boxSize.y = sizeY;
    }

    // Update is called once per frame
    void Update()
    {
        cameraPlayer.GetComponent<CameraFollow>().canFollow = !enemyNotDetected;

        if (canOverlap)
        {
            enemyNotDetected = Physics2D.OverlapBox(transform.position, boxSize, 0f, enemyLayerMask);

        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            canOverlap = true;
        }
    }
}
