using UnityEngine;

public class DynamicSortingOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private ParticleSystemRenderer particleSystemRenderer;
    private SpriteRenderer[] m_SpriteGroup;
    private ParticleSystemRenderer[] m_ParticleGroup;

    void Start()
    {
        if (GetComponent<SpriteRenderer>() != null)
        // Try to get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (GetComponent<ParticleSystemRenderer>() != null)
        // Try to get the ParticleSystemRenderer component
        particleSystemRenderer = GetComponent<ParticleSystemRenderer>();
        if (this.transform.GetComponentsInChildren<SpriteRenderer>(true) != null)
        m_SpriteGroup = this.transform.GetComponentsInChildren<SpriteRenderer>(true);

        if (this.transform.GetComponentsInChildren<ParticleSystemRenderer>(true) != null)
            m_ParticleGroup = this.transform.GetComponentsInChildren<ParticleSystemRenderer>(true);
    }

    void Update()
    {
        int sortingOrder = Mathf.RoundToInt(transform.position.y * -100);

        // Update sorting order for SpriteRenderer
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = sortingOrder;
        }

        // Update sorting order for ParticleSystemRenderer
        if (particleSystemRenderer != null)
        {
            particleSystemRenderer.sortingOrder = sortingOrder;
        }

        if (m_SpriteGroup.Length > 0)
        {
            for (int i = 0; i < m_SpriteGroup.Length; i++)
            {

                m_SpriteGroup[i].sortingOrder = sortingOrder;
            }
        }

        if (m_ParticleGroup.Length > 0)
        {
            for (int i = 0; i < m_SpriteGroup.Length; i++)
            {
                m_ParticleGroup[i].sortingOrder = sortingOrder;
            }
        }
    }
}