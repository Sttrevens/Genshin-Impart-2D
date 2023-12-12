using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public delegate void OnHealthChanged(int currentHealth);
    public event OnHealthChanged onHealthChanged;

    public string mainGameScene;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Inform any subscribers that health has changed.
        onHealthChanged?.Invoke(currentHealth);

        Debug.Log("Your Health: " + currentHealth);

        // If the player's health drops to zero or below, the player dies.
        if (currentHealth <= 0)
        {
            ReloadScene();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Inform any subscribers that health has changed.
        onHealthChanged?.Invoke(currentHealth);
    }

    public void ReloadScene()
    {
        //SceneManager.LoadScene(mainGameScene);
    }
}