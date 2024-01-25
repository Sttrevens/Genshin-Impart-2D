using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class TileBurner : MonoBehaviour
{
    public Tile dirtTile; // The tile to replace the burned tile with
    public Tile grassTile;
    public Tilemap tilemap; // Reference to the tilemap
    public float burnDuration = 10f;
    public ParticleSystem burnEffectPrefab; // Prefab for visual burning effect

    private Vector3Int tilePosition;

    private void Start()
    {
        // Calculate and store the tile position based on the collider's position
        tilePosition = tilemap.WorldToCell(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fire"))
        {
            TileBase tile = tilemap.GetTile(tilePosition);
            if (tile != null && tile == grassTile) // Change this condition based on your requirement
            {
                StartCoroutine(BurnTile());
            }
        }
    }

    private IEnumerator BurnTile()
    {
        Debug.Log("Buring!");
        // Trigger burn visual effect
        if (burnEffectPrefab != null)
        {
            ParticleSystem burnEffect = Instantiate(burnEffectPrefab, transform.position, Quaternion.identity);
            Destroy(burnEffect.gameObject, burnDuration); // Destroy effect after duration
        }

        yield return new WaitForSeconds(burnDuration);

        // Change to dirt tile after burning
        tilemap.SetTile(tilePosition, dirtTile); 
    }
}