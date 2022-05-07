using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

[CreateAssetMenu]
public class StructureTile : TileBase
{
    public Sprite baseSprite;
    public TileBase crater;
    public GameObject structurePrefab;

    public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject go)
    {
        // выравнивание объекта на середину тайла
        if (go)
        {
            go.GetComponent<Structure>().Init(location);
            go.transform.position = go.transform.position + new Vector3(0.5f, 0.5f, 0);
        }
        return true;
    }

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = baseSprite;
        tileData.gameObject = structurePrefab;
        tileData.color = Color.white;
    }
}
