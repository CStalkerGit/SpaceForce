using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

[CreateAssetMenu]
public class TerrainTile : TileBase
{
    public Sprite baseSprite;
    public Sprite[] sprites;

    static readonly int[] indices16 = new int[16] { 15, 11, 12, 8, 3, 7, 0, 4, 14, 10, 13, 9, 2, 6, 1, 5 };

    public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
    {
        return true;
    }

    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        // при обновлении соседних тайлов пересоздаются gameObject на них
        if (sprites.Length == 16)
        {
            for (int yd = -1; yd <= 1; yd++)
                for (int xd = -1; xd <= 1; xd++)
                {
                    Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                    if (IsNeighbor(tilemap, position)) tilemap.RefreshTile(position);
                }
        }

        tilemap.RefreshTile(location);
    }

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        switch (sprites.Length)
        {
            case 16:
                tileData.sprite = Tiles16(location, tilemap);
                break;
            default:
                tileData.sprite = baseSprite;
                break;
        }

        tileData.color = Color.white;
    }

    private Sprite Tiles16(Vector3Int location, ITilemap tilemap)
    {
        int mask = IsNeighbor(tilemap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
        mask += IsNeighbor(tilemap, location + new Vector3Int(1, 0, 0)) ? 2 : 0;
        mask += IsNeighbor(tilemap, location + new Vector3Int(0, -1, 0)) ? 4 : 0;
        mask += IsNeighbor(tilemap, location + new Vector3Int(-1, 0, 0)) ? 8 : 0;

        if (mask >= 0 && mask < sprites.Length) return sprites[indices16[mask]];
        else
        {
            Debug.LogWarning("Not enough sprites in instance");
            return baseSprite;
        }
    }

    // проверка, является ли соседний тайл совместимым с текущим
    private bool IsNeighbor(ITilemap tilemap, Vector3Int position)
    {
        TerrainTile tile = tilemap.GetTile(position) as TerrainTile;
        if (tile == null) return false;
        //if (tile == this) return true;
        return true;
    }
}
