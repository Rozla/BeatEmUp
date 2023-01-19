using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAreaTrigger : MonoBehaviour
{

    [SerializeField] GameObject cameraPlayer;

    [SerializeField] bool enemyDetected;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraPlayer.GetComponent<CameraFollow>().canFollow = !enemyDetected;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            enemyDetected = true;
        }
        else
        {
            enemyDetected = false;
        }
    }
}
