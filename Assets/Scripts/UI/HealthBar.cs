using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class HealthBar : MonoBehaviour
{
    //[Header("Player")]
    public GameObject player; //the player object that holds the player script
    private PlayerController playerController; //the player object that holds the player script
    //[Header("Stats")]
    private int maxHealth; //total amount of hearts
    private int health; //current number of hearts

    //[Header("UI")]
    private Image healthUI; //use an image of four hearts next to eachother, spacing will help make it look better

    private void Start()
    {
        healthUI = gameObject.GetComponent<Image>();
        playerController = player.GetComponent<PlayerController>();
    }

    


    void Update()
    { //change the value of your sprite using a horizontal fill, space the hearts out so that 1 includes only 1 heart and so on
        
        maxHealth = playerController.MaxPossibleHealth;
        health = playerController.currentHealth;

        healthUI.fillAmount = health / (float)maxHealth; //will always return a decimal for percentage
    }
}