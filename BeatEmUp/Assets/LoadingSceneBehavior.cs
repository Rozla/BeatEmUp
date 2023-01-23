using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingSceneBehavior : MonoBehaviour
{

    [SerializeField] float maxValue = 100f;
    [SerializeField] float currentValue;
    [SerializeField] Slider loadingSlider;
    [SerializeField] TextMeshProUGUI percentText;
    [SerializeField] GameObject loaderScene;

    float t = 0f;


    // Start is called before the first frame update
    void Start()
    {
        currentValue = 0f;
        loadingSlider.maxValue = maxValue;
        loadingSlider.value = currentValue;

    }

    // Update is called once per frame
    void Update()
    {

        loadingSlider.value = currentValue;
        percentText.text = currentValue.ToString() + "%";

        t += Time.deltaTime;

        if(t > .2f && currentValue < maxValue)
        {
            currentValue += 1f;
            t = 0f;
        }

        if(currentValue >= maxValue)
        {
            percentText.text = "CLICK THE SCREEN TO BEGIN";
            percentText.fontSize = 20f;

            if (Input.GetMouseButtonDown(0))
            {
                loaderScene.GetComponent<SceneLoader>().LoadScene1();
            }
        }
    }
}
