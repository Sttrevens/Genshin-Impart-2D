using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomTilemapCollision : MonoBehaviour
{
    private Tilemap tilemap;
    public string grassTileName;
    public string lavaTileName;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        GenerateBoxColliders();
    }

    void GenerateBoxColliders()
    {
        Debug.Log("Start Generate!");
        // Loop through all tiles in the Tilemap
        for (int x = 0; x < tilemap.size.x; x++)
        {
            for (int y = 0; y < tilemap.size.y; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(cellPosition);

                // Check if the tile needs a BoxCollider
                if (tile != null && (tile.name == grassTileName || tile.name == lavaTileName))
                {
                    Debug.Log("generating...");
                    CreateBoxCollider(cellPosition, tile.name);
                }
            }
        }
    }

    void CreateBoxCollider(Vector3Int cellPosition, string tileName)
    {
        // Calculate the world position of the tile based on cell position
        Vector3 worldPosition = tilemap.CellToWorld(cellPosition);

        // Create a new GameObject for the BoxCollider
        GameObject colliderObject = new GameObject("Collider_" + tileName);

        // Add BoxCollider2D component and configure settings
        BoxCollider2D collider = colliderObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true; // Set as trigger collider
        collider.size = tilemap.cellSize; // Set size based on Tilemap cell size

        // Position the collider object to match the tile
        colliderObject.transform.position = worldPosition;

        // Set collider layer based on tile type (optional)
        if (tileName == grassTileName)
        {
            colliderObject.layer = LayerMask.NameToLayer("Grass");
        }
        else if (tileName == lavaTileName)
        {
            colliderObject.layer = LayerMask.NameToLayer("Lava");
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Fire")
        {
            Vector3Int cellPosition = tilemap.WorldToCell(other.transform.position);
            TileBase tile = tilemap.GetTile(cellPosition);

            // Check for specific tile type and display debug message
            if (tile != null)
            {
                if (tile.name == grassTileName)
                {
                    Debug.Log("Fire entered grass tile!");
                }
                else if (tile.name == lavaTileName)
                {
                    Debug.Log("Fire entered lava tile!");
                }
            }
        }
    }
}
