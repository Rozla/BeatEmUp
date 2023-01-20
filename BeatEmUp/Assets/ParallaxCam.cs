using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCam : MonoBehaviour
{
    [SerializeField] float additionalScrollSpeed;

    [SerializeField] GameObject[] backgrounds;

    [SerializeField] float[] scrollSpeed;

    private void FixedUpdate()
    {
        for (int background = 0; background < backgrounds.Length; background++)
        {

            Renderer rend = backgrounds[background].GetComponent<Renderer>();

            float offset = Time.time * (scrollSpeed[background] + additionalScrollSpeed);

            rend.material.SetTextureOffset("_MainTex", new Vector2( 0, offset));

        }
    }
}
