using System.Collections;
using UnityEngine;

public class CollectiblesBehavior : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Transform destination;

    Vector2 destinationPoint;

    Coroutine corAandD;

    bool isSpawned = true;

    bool isCollected;

    // Start is called before the first frame update
    void Start()
    {
        
        corAandD = StartCoroutine(AnimAndDestroy());
    }

    // Update is called once per frame
    void Update()
    {


        destinationPoint = destination.position;

        if(isSpawned)
        {
            StartCoroutine(AnimAndDestroy());
        }

        if(isCollected)
        {
            MoveToScore();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopCoroutine(corAandD);
            isCollected = true;
            collision.gameObject.GetComponent<PlayerScore>().addScore = true;
        }
    }

    IEnumerator AnimAndDestroy()
    {
        isSpawned = false;
        yield return new WaitForSeconds(5f);
        sr.enabled = false;
        yield return new WaitForSeconds(.2f);
        sr.enabled = true;
        yield return new WaitForSeconds(.3f);
        sr.enabled = false;
        yield return new WaitForSeconds(.2f);
        sr.enabled = true;
        yield return new WaitForSeconds(.3f);
        sr.enabled = false;
        yield return new WaitForSeconds(.2f);
        sr.enabled = true;
        yield return new WaitForSeconds(.3f);
        sr.enabled = false;
        yield return new WaitForSeconds(.2f);
        sr.enabled = true;
        yield return new WaitForSeconds(.3f);
        sr.enabled = false;
        yield return null;
        Destroy(gameObject);
    }

    void MoveToScore()
    {
        transform.position = Vector2.Lerp(transform.position, destinationPoint, 10f * Time.deltaTime);


        sr.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 80);

        StartCoroutine(DelayDestroy());
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
}
