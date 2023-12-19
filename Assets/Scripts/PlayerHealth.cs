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

    private Rigidbody2D playerRigidbody;

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
        if (damage <= 10)
        {
            characterController.PlaySoundEffect(characterController.hitClips[Random.Range(0, 1)], 1.5f);
        }
        else if(damage > 10 && damage < 30)
        {
            characterController.PlaySoundEffect(characterController.hitClips[Random.Range(1, 3)], 1.5f);
        }
        else if (damage >= 30)
        {
            characterController.PlaySoundEffect(characterController.hitClips[Random.Range(3, 6)], 1.5f);
        }

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
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.angularVelocity = 0f;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll; // Freeze position and rotation
        }

        characterController.attackDamage = 0;
        //SceneManager.LoadScene(mainGameScene);
    }
}