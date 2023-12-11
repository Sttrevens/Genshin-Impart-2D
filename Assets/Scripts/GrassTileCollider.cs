using UnityEngine;
using UnityEngine.Tilemaps;

public class GrassTileColliders : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase grassTile;
    public GameObject grassColliderPrefab; // A prefab with a collider and a script for interaction

    private void Start()
    {
        CreateCollidersForGrassTiles();
    }

    private void CreateCollidersForGrassTiles()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile == grassTile)
                {
                    Vector3 worldPos = tilemap.CellToWorld(new Vector3Int(x + bounds.x, y + bounds.y, 0));
                    Instantiate(grassColliderPrefab, worldPos, Quaternion.identity);
                }
            }
        }
    }
}