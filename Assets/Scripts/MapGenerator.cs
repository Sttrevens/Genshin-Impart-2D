using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public TileType[] tileTypes;
    public ObjectType[] objectTypes;

    public int width = 10;
    public int height = 10;

    public float noiseScale = 0.1f;

    public Tilemap tilemap;

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noiseValue = Mathf.PerlinNoise(x * noiseScale, y * noiseScale);
                TileType tileType = ChooseTileType(noiseValue);
                CreateTile(tileType, x, y);

                // Optionally generate objects
                /*if (ShouldPlaceObject())
                {
                    ObjectType objectType = ChooseObjectType();
                    PlaceObject(objectType, x, y);
                }*/
            }
        }
    }

    private bool ShouldPlaceObject()
    {
        // Example: 20% chance to place an object
        return Random.value < 0.2f;
    }



    private TileType ChooseTileType(float noiseValue)
    {
        // Sort the tile types by noise threshold
        var sortedTileTypes = tileTypes.OrderBy(t => t.noiseThreshold).ToList();

        foreach (var tileType in sortedTileTypes)
        {
            if (noiseValue <= tileType.noiseThreshold)
            {
                Debug.Log("Chosen Tile Type: " + tileType.name);
                return tileType;
            }
        }

        // Random fallback if no threshold is met
        return sortedTileTypes[Random.Range(0, sortedTileTypes.Count)];
    }

    private ObjectType ChooseObjectType()
    {
        float totalProbability = objectTypes.Sum(o => o.spawnProbability);
        float randomPoint = Random.value * totalProbability;
        float currentSum = 0;

        foreach (var objectType in objectTypes)
        {
            currentSum += objectType.spawnProbability;
            if (randomPoint <= currentSum)
                return objectType;
        }

        return objectTypes.Last(); // Fallback
    }

    private void CreateTile(TileType tileType, int x, int y)
    {
        Tile chosenTile = tileType.tileAssets[Random.Range(0, tileType.tileAssets.Length)];
        Vector3Int tilePosition = new Vector3Int(x - width / 2, y - height / 2, 0);
        tilemap.SetTile(tilePosition, chosenTile);
    }

    private void PlaceObject(ObjectType objectType, int x, int y)
    {
        // Instantiate a prefab from the objectType's prefab array
        GameObject chosenPrefab = objectType.objectPrefabs[Random.Range(0, objectType.objectPrefabs.Length)];

        // Place the object at position x, y
        // Implementation depends on your game setup
    }
}