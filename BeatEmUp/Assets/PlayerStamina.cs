using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{

    [SerializeField] float maxStamina = 100f;
    [SerializeField] float currentStamina;
    [SerializeField] Slider staminaSlider;

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
    }

    public void TakeStamina()
    {
        if(currentStamina <= maxStamina)
        {
            currentStamina += 10f;
        }
    }

    public void ResetStamina()
    {
        currentStamina = 0f;
    }


}
