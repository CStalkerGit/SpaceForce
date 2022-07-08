using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class GameMap : MonoBehaviour
{
    private Tilemap tilemap;

    private Vector3 tilemapPos;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // зацикливание карты
        if (transform.position.y < -tilemap.size.y) tilemapPos.y = Engine.heightInTiles;

        // вертикальный скроллинг карты вниз
        tilemapPos.y -= Engine.scrollingSpeed * Time.deltaTime;
        transform.position = tilemapPos;
    }

    /// <summary>
    /// Проверка, не вышел ли объект за границы экрана
    /// </summary>
    /// <param name="position">текущая позиция объекта</param>
    /// <param name="offset">дополнительное смещение от краев экрана</param>
    public bool OutOfBounds(Vector3 position, bool upperBorder)
    {
        const float offset = 1.5f;
        float height = 2f * Camera.main.orthographicSize;
        float width = Engine.widthInTiles; //height * Camera.main.aspect;

        // выход за левую или правую границу экрана
        if (position.x < (-width / 2 - offset) || position.x > (width / 2 + offset)) return true;
        // выход за нижнюю границу экрана
        if (position.y < (-height / 2 - offset)) return true;
        // выход за верхнюю границу экрана, если нужно проверить
        if (upperBorder && (position.y > (height / 2 + offset))) return true;

        return false;
    }

    public void CreateCrater(Vector3Int coord)
    {
        StructureTile tile = tilemap.GetTile<StructureTile>(coord);
        if (tile) tilemap.SetTile(coord, tile.crater);
    }

    /// <summary>
    /// Получить координаты тайла в указанной позиции
    /// </summary>
    public Vector3Int GetPosition(Vector3 coord)
    {
        coord -= tilemapPos; // сдвигаем позицию тестируемого объекта относительно положения карты
        return new Vector3Int(Mathf.RoundToInt(coord.x - 0.5f), Mathf.RoundToInt(coord.y - 0.5f), 0);
    }

    /// <summary>
    /// Возвращает Entity, связанный cо зданием на карте
    /// </summary>
    /// <param name="coord">Координаты, где должно находится здание</param>
    /// <returns>Entity здания либо null если его нет в указанной точке</returns>
    public Structure GetTileObject(Vector3 coord)
    {
        GameObject obj = tilemap.GetInstantiatedObject(GetPosition(coord));
        if (obj) return obj.GetComponent<Structure>();
        return null;
    }

    public Structure GetTileObject(int x, int y)
    {
        GameObject obj = tilemap.GetInstantiatedObject(new Vector3Int(x, y, 0));
        if (obj) return obj.GetComponent<Structure>();
        return null;
    }
}
