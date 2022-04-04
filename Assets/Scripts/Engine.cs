using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Engine : MonoBehaviour
{
    public Tilemap tilemap;

    public const float scrollingSpeed = 2.0f;

    private Vector3 tilemapPos;
    private static Engine instance;
    public static List<Entity> enemies = new List<Entity>();
    public static List<Entity> bonuses = new List<Entity>();

    private void Awake()
    {
        instance = this;
        tilemapPos = tilemap.transform.position;
    }

    private void FixedUpdate()
    {
        // скроллинг карты вниз
        tilemapPos.y -= scrollingSpeed * Time.deltaTime;
        tilemap.transform.position = tilemapPos;
    }

    /// <summary>
    /// Проверка, не вышел ли объект за границы экрана
    /// </summary>
    /// <param name="position">текущая позиция объекта</param>
    /// <param name="offset">дополнительное смещение от краев экрана</param>
    public static bool OutOfBounds(Vector3 position, float offset)
    {
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        if (position.x < -width / 2 - offset || position.x > width / 2 + offset) return true;
        if (position.y < -height / 2 - offset || position.y > height / 2 + offset) return true;
        return false;
    }

    /// <summary>
    /// Проверка, не вышел ли объект за нижнюю границу экрана
    /// </summary>
    /// <param name="position">текущая позиция объекта</param>
    /// <param name="offset">смещение от нижнего края экрана</param>
    public static bool OutOfBoundsBottom(Vector3 position, float offset)
    {
        if (position.y < -Camera.main.orthographicSize - offset) return true;
        return false;
    }

    /// <summary>
    /// Проверка столкновения объекта с врагом или тайлом
    /// </summary>
    public static Entity IsEnemyCollision(Entity entity)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.IsCollission(entity)) return enemy;
        }

        TerrainTile tile = instance.tilemap.GetTile<TerrainTile>(entity.GetTilePosition(instance.tilemap.transform.position));
        if (tile)
        {
            // получение объекта, находящегося на тайле
            Entity tileObject = GetInstantiatedObject(entity);
            if (tileObject) if (tileObject.IsCollission(entity)) return tileObject;
        }

        return null;
    }

    /// <summary>
    /// Проверка взаимодействия игрока с бонусом
    /// </summary>
    public static Entity IsBonusCollision(Entity entity)
    {
        foreach (var bonus in bonuses)
        {
            if (bonus.IsCollission(entity)) return bonus;
        }

        return null;
    }

    /// <summary>
    /// Получить находящийся на тайле объект по координатам entity
    /// </summary>
    public static Entity GetInstantiatedObject(Entity entity)
    {
        // получение GameObject, ассоциированного с тайлом
        GameObject obj = instance.tilemap.GetInstantiatedObject(entity.GetTilePosition(instance.tilemap.transform.position));
        if (obj) return obj.GetComponent<Entity>(); else return null;
    }
}
