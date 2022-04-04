using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    Background,
    FullBlock,
    Platform,
    Spikes
}

[CreateAssetMenu]
public class TerrainTile : TileBase
{
    public TileType type;

    public Sprite sprite;
    public GameObject gameObject;

    public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
    {
        // выравнивание объекта на середину тайла
        if (go) go.transform.position = go.transform.position + new Vector3(0.5f, 0.5f, 0);
        return true;
    }

    //public override void RefreshTile(Vector3Int location, ITilemap tilemap)

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = sprite;
        tileData.gameObject = gameObject;
    }
}
