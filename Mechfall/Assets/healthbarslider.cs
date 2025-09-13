using UnityEngine;
using UnityEngine.UI;  

public class HealthBar : MonoBehaviour
{
   
    public Slider healthSlider;
    
    public Image fillImage;

 
    public float currentHealth;

    public float maxHealth = 100f;
    
    void Start()
    {
 
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

 
        UpdateHealthBarColor();
    }

    void Update()
    {
        

            UpdateHealthBarColor();

    }

   
    private void UpdateHealthBarColor()
    {
        if (currentHealth <= 20)
        {
            
            fillImage.color = Color.red;
        }
        else if (currentHealth <= 90)
        {
            
            fillImage.color = Color.orange; 
        }
        else
        {
           
            fillImage.color = Color.green;
         
        }
    }
}
