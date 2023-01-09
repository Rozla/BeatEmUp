using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_MOVEMENT : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    Rigidbody2D bossrb;
    public bool _moveRight = true;
    float bossSpeed = 500;
    private float _startPos;
    private float _endPos;
    public bool _isFacingRight;
    public int UnitsToMove = 5;
  
    


    // Start is called before the first frame update
    public void Awake()
    {
        bossrb = GetComponent<Rigidbody2D>();
        _startPos = transform.position.x;
        _endPos = _startPos + UnitsToMove;
        _isFacingRight = transform.localScale.x > 0;
    }

   

    // Update is called once per frame
    void Update()
    {
        if (_moveRight)
        {
            bossrb.AddForce(Vector2.right * bossSpeed * Time.deltaTime);
            if (!_isFacingRight)
                Flip();
        }

        if (bossrb.position.x >= _endPos)
            _moveRight = false;

       


    }
    public void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        _isFacingRight = transform.localScale.x > 0;
    }
}
