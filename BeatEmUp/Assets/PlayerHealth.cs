using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] public float currentHealth;
    [SerializeField] public int lifeCount;

    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI lifeTMP;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        lifeCount = 3;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTMP.text = lifeCount.ToString();
        healthSlider.value = currentHealth;
        healthSlider.maxValue = maxHealth;

        if (Input.GetKeyDown("b"))
        {
            TakeDamage();
        }

        if (Input.GetKeyDown("n"))
        {
            TakeHealth();
        }

    }

    public void TakeDamage()
    {
        currentHealth -= 10f;

        if (currentHealth <= 0f)
        {
            GetComponent<PlayerMovement>().isDead = true;
            lifeCount -= 1;
            currentHealth = maxHealth;
        }
        else
        {
            GetComponent<PlayerMovement>().isHurt = true;
        }
    }

    public void TakeHealth()
    {
        if(currentHealth >= maxHealth)
        {
            return;
        }

        currentHealth += 10f;
    }
}
