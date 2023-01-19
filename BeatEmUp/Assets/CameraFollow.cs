using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] Vector3 offset = Vector3.zero;
    [SerializeField] BoxCollider2D cameraBounds;
    [SerializeField] public bool canFollow;

    Camera mainCamera;
    Vector2 cameraDimension;


    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        cameraDimension.y = mainCamera.orthographicSize;
        cameraDimension.x = mainCamera.orthographicSize * mainCamera.aspect;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!canFollow)
        {
            return;
        }

        Vector3 followingPosition = playerTransform.position + offset;

        float minX = cameraBounds.transform.position.x - cameraBounds.size.x / 2 + cameraDimension.x;
        float maxX = cameraBounds.transform.position.x + cameraBounds.size.x / 2 - cameraDimension.x;
        followingPosition.x = Mathf.Clamp(followingPosition.x, minX, maxX);

        float minY = cameraBounds.transform.position.y - cameraBounds.size.y / 2 + cameraDimension.y;
        float maxY = cameraBounds.transform.position.y + cameraBounds.size.y / 2 - cameraDimension.y;
        followingPosition.y = Mathf.Clamp(followingPosition.y, minY, maxY);

        Vector3 currentVelocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, followingPosition, ref currentVelocity, Time.deltaTime * moveSpeed);


    }

}
