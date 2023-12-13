using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public TileType[] tileTypes;
    public ObjectType[] objectTypes;

    public int width = 10;
    public int height = 10;

    public float environmentItemPossibility = 0.01f;
    public float noiseScale = 0.1f;

    public List<GameObject> tiles; // Replace Tilemap with List of GameObjects

    void Start()
    {
        tiles = new List<GameObject>();
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
                if (ShouldPlaceObject())
                {
                  ObjectType objectType = ChooseObjectType();
                  PlaceObject(objectType, x, y);
                }
            }
        }
    }

    private bool ShouldPlaceObject()
    {
        // Example: 20% chance to place an object
        return Random.value < environmentItemPossibility;
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
        GameObject chosenPrefab = tileType.tilePrefab;
        if (chosenPrefab != null)
        {
            SpriteRenderer spriteRenderer = chosenPrefab.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = tileType.tileAssets[Random.Range(0, tileType.tileAssets.Length)];
            }
            else
            {
                Debug.LogError("Prefab does not have a SpriteRenderer component.");
            }
        }
        else
        {
            Debug.LogError("chosenPrefab is null.");
        }
        Vector3Int tilePosition = new Vector3Int(x - width / 2 + Mathf.FloorToInt(transform.position.x), y - height / 2 + Mathf.FloorToInt(transform.position.y), 0);

        GameObject tileObject = Instantiate(chosenPrefab, tilePosition, Quaternion.identity);
        tileObject.transform.parent = transform;
        tiles.Add(tileObject);
    }

    private void PlaceObject(ObjectType objectType, int x, int y)
    {
        // Instantiate a prefab from the objectType's prefab array
        GameObject chosenPrefab = objectType.objectPrefabs[Random.Range(0, objectType.objectPrefabs.Length)];

        // Retrieve the corresponding tile object from the list
        GameObject tileObject = tiles[x * height + y];

        // Calculate world position for the new object
        Vector3 worldPosition = tileObject.transform.position;

        // Place the object as a child of the tile object
        GameObject objectInstance = Instantiate(chosenPrefab, worldPosition, Quaternion.identity);
        objectInstance.transform.parent = tileObject.transform;

        // Optionally, adjust local position if needed
        // objectInstance.transform.localPosition = Vector3.zero; or any other offset
    }
}