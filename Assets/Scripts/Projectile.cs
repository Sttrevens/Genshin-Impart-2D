using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 targetPosition;
    public float speed = 5f;
    public GameObject explosionEffectPrefab;

    private CharacterController_2D characterController;

    public bool isIceBall = false;

    void Start()
    {
        Invoke("Explode", 2f);

        GameObject mainCharacter = GameObject.Find("MainCharacter");
        if (mainCharacter != null)
        {
            characterController = mainCharacter.GetComponent<CharacterController_2D>();
            if (characterController == null)
            {
                Debug.LogError("CharacterController_2D component not found on MainCharacter.");
            }
        }
        else
        {
            Debug.LogError("MainCharacter GameObject not found in the scene.");
        }
    }

    void Update()
    {
        // Move towards targetPosition
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void IdentifyPlayer(Vector3 playerPosition)
    {
        targetPosition = playerPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth targetHealth = other.GetComponent<PlayerHealth>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(10);
            Explode();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
        {
            Explode();
        }

        void OnDestroy()
        {
            Explode();
        }

        private void Explode()
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        if (!isIceBall)
        {
            characterController.PlaySoundEffect(characterController.powerClips[0]);
        }
        else
        {
            characterController.PlaySoundEffect(characterController.powerClips[3]);
        }
            Destroy(gameObject); // Destroy the projectile
        }
}
