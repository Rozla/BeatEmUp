using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCam : MonoBehaviour
{
    [SerializeField] float additionalScrollSpeed;

    [SerializeField] GameObject[] backgrounds;

    [SerializeField] float[] scrollSpeed;


    float t;
    private void FixedUpdate()
    {
        t += Time.fixedDeltaTime;

        for (int background = 0; background < backgrounds.Length; background++)
        {

            Renderer rend = backgrounds[background].GetComponent<Renderer>();

            float offset = t * (scrollSpeed[background] + additionalScrollSpeed);

            rend.material.SetTextureOffset("_MainTex", new Vector2( 0, offset));

        }
    }
}
