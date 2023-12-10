using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewTileType", menuName = "MapGeneration/TileType")]
public class TileType : ScriptableObject
{
    public Tile[] tileAssets;
    public float noiseThreshold;
}
