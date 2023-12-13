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

    public ParticleSystem[] bloodEffects;
    public GameObject drunkEffect;

    public CharacterController_2D characterController;

     private void Start()
    {
        currentHealth = maxHealth;
        UpdateBloodEffectColor();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHealthChanged?.Invoke(currentHealth);
        UpdateBloodEffectColor();

        characterController.m_Animator.Play("Hit");

        if (currentHealth <= 0)
        {
            ReloadScene();
        }
    }

    public void TakeFireDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHealthChanged?.Invoke(currentHealth);
        UpdateBloodEffectColor();

        if (currentHealth <= 0)
        {
            ReloadScene();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHealthChanged?.Invoke(currentHealth);
        UpdateBloodEffectColor();
    }

    void UpdateBloodEffectColor()
    {
        float healthRatio = (float)currentHealth / maxHealth;
        if (bloodEffects.Length > 0)
        {
            foreach (ParticleSystem bloodEffect in bloodEffects)
            {
            var mainModule = bloodEffect.main;

            Color startColor1 = new Color(1, 1, 1, 1.0f - healthRatio * 2);
        Color startColor2 = new Color(1, 1, 1, 1.0f - healthRatio);

        mainModule.startColor = new ParticleSystem.MinMaxGradient(startColor1, startColor2);
            }
        }
        if (drunkEffect != null)
        {
            SpriteRenderer spriteRenderer = drunkEffect.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(1, 1, 1, 1.0f - healthRatio);
        }
    }

    public void ReloadScene()
    {
        characterController.m_Animator.Play("Die");
        //SceneManager.LoadScene(mainGameScene);
    }
}