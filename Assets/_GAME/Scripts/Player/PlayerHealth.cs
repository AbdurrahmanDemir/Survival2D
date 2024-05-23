using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private int maxHealth;
    int health;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;


    void Start()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        UpdateUI();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        int realDamage= Mathf.Min(damage,health);
        health-=realDamage;
        UpdateUI();
        if (health < 0)
        {
            PassAway();
        }
    }
    private void UpdateUI()
    {
        healthSlider.value = health;
        healthText.text=health + " / " + maxHealth;
    }
    private void PassAway()
    {
        Debug.Log("Ded");
    }
}
