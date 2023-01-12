using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    float scoreValue;

    [HideInInspector]
    public bool addScore;

    // Start is called before the first frame update
    void Start()
    {
        scoreValue = 0f;   
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = scoreValue.ToString("000000");

        if (addScore)
        {
            addScore = false;
            StartCoroutine(AddScore());
        }

    }


    IEnumerator AddScore()
    {
        for (int loop = 0; loop < 50; loop++)
        {
            scoreValue += 1;
            yield return new WaitForSeconds(.01f);
        }   
        
    }
}
