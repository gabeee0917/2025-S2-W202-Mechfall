using UnityEngine;
using UnityEngine.UI;  // Import UI elements

// Health bar for multiplayer PVP
public class HealthBarMulti : MonoBehaviour
{
   
    public Slider healthSlider;
    
    public Image fillImage;

 
    public float currentHealth;

    public float maxHealth = 100f;
    public UIManagerMulti uIManager;
    void Start()
    {
 
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

 
        UpdateHealthBarColor();
    }

    void Update()
    {
        if (uIManager.playerStatus != null)
        {
            currentHealth = uIManager.playerStatus.health;
            healthSlider.value = uIManager.playerStatus.health;

            UpdateHealthBarColor();
        }

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
