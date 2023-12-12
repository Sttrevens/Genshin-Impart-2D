using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTilemapManager : MonoBehaviour
{
    public TileType[] tileTypes; // Array of different tile types
    public int width, height; // Dimensions of the tilemap

    private GameObject[,] tiles; // 2D array to keep track of placed tiles

    void Start()
    {
        tiles = new GameObject[width, height];
        // Initialize your tilemap here if needed
    }

    public void PlaceTile(int x, int y, int tileIndex)
    {
        Debug.Log("Start placing!");
        if (x < 0 || x >= width || y < 0 || y >= height || tileIndex >= tileTypes.Length)
            return;

        GameObject tilePrefab = tileTypes[tileIndex].tilePrefab;
        Vector3 position = new Vector3(x, y, 0); // Modify as needed for your coordinate system

        GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity, transform);
        tiles[x, y] = newTile;
    }

    public void RemoveTile(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return;

        if (tiles[x, y] != null)
        {
            DestroyImmediate(tiles[x, y]);
            tiles[x, y] = null;
        }
    }
}