using UnityEngine;
using System.Collections;

public class FlammableObject : MonoBehaviour
{
    private bool isBurning = false;
    public float burnDuration = 20f;
    

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collide with " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Fire" && !isBurning)
        {
            StartCoroutine(Burn());
        }
    }

    IEnumerator Burn()
    {
        Debug.Log("Fire!");
        isBurning = true;
        gameObject.tag = "Fire"; // Change tag to "Fire"
        // Add visual effects to show the object is burning
        yield return new WaitForSeconds(burnDuration); // Wait for 10 seconds
        Destroy(gameObject); // Destroy the object after burning
    }
}