using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    public AudioClip[] grassClips;
    public AudioClip[] stoneClips;
    public AudioClip[] waterClips;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<GroundCollider>() is GroundCollider ground && !audioSource.isPlaying)
        {
            AudioClip clip = null;

            if (ground.isWet)
            {
                clip = waterClips[Random.Range(0, waterClips.Length)];
            }
            else if (ground.isFrozen)
            {
                clip = stoneClips[Random.Range(0, stoneClips.Length)];
            }
            else
            {
                clip = grassClips[Random.Range(0, grassClips.Length)];
            }

            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}