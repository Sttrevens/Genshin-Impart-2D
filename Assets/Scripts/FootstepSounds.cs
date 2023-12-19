using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    public AudioClip[] grassClips;
    public AudioClip[] stoneClips;
    public AudioClip[] waterClips;

    private AudioSource audioSource;

    private float lastPlayTime = 0f;
    public float minInterval = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time >= lastPlayTime + minInterval)
        {
            if (other.gameObject.GetComponent<GroundCollider>() is GroundCollider ground)
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
                    lastPlayTime = Time.time;
                }
            }
        }
    }
}