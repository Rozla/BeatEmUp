using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;


    Vector3 dirToFollow;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        dirToFollow = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
        transform.position = dirToFollow;
    }
}
