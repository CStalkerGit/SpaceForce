using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Engine : MonoBehaviour
{
    public Tilemap tilemap;
    public Effect explosionPrefab;

    public const float scrollingSpeed = 2.5f;

    private Vector3 cameraPos;
    private static Engine instance;
    public static List<Entity> enemies = new List<Entity>();
    public static List<Entity> bonuses = new List<Entity>();

    // для смены сцены в случае победы или поражения
    private bool changeScene = false;
    private float changeSceneTime = 2.5f;

    private void Awake()
    {
        instance = this;
        cameraPos = Camera.main.transform.position;

        enemies.Clear();
        bonuses.Clear();
    }

    private void FixedUpdate()
    {
        // скроллинг камеры вверх
        cameraPos.y += scrollingSpeed * Time.deltaTime;
        Camera.main.transform.position = cameraPos;

        // проверка на необходимость загрузки следующей сцены (титульного экрана или следующего уровня)
        if (changeScene)
        {
            changeSceneTime -= Time.deltaTime;
            if (changeSceneTime < 0) SceneManager.LoadScene("TitleScreen");
        }
        else
        {
            // проверка на гибель игрока
            if (Player.IsPlayerDead()) changeScene = true;
        }

        // проверка на конец карты
        if (tilemap.transform.position.y < -tilemap.size.y - 8)
        {
            changeScene = true;
            Debug.Log("The end of the level");
        }
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
        position -= instance.cameraPos; // смещение относительно текущего положения камеры

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
        if (position.y < -Camera.main.orthographicSize - offset - instance.cameraPos.y) return true;
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

        TerrainTile tile = instance.tilemap.GetTile<TerrainTile>(entity.GetTilePosition());
        if (tile)
        {
            // получение объекта, находящегося на тайле
            Entity tileObject = GetInstantiatedObject(entity);
            if (tileObject)
                if (tileObject.IsCollission(entity)) return tileObject;
        }

        return null;
    }

    /// <summary>
    /// Проверка взаимодействия игрока с бонусом
    /// </summary>
    public static Bonus IsBonusCollision(Entity entity)
    {
        foreach (var bonus in bonuses)
        {
            if (bonus.IsCollission(entity)) return (Bonus)bonus;
        }

        return null;
    }

    /// <summary>
    /// Проверка взаимодействия игрока с бонусом
    /// </summary>
    public static Entity GetNearestEnemy(Entity entity)
    {
        foreach (var bonus in bonuses)
        {
            if (bonus.IsCollission(entity)) return (Bonus)bonus;
        }

        return null;
    }

    /// <summary>
    /// Получить находящийся на тайле объект по координатам entity
    /// </summary>
    public static Entity GetInstantiatedObject(Entity entity)
    {
        // получение GameObject, ассоциированного с тайлом
        GameObject obj = instance.tilemap.GetInstantiatedObject(entity.GetTilePosition());
        if (obj) return obj.GetComponent<Entity>(); else return null;
    }

    /// <summary>
    /// Создать эффект взрыва 
    /// </summary>
    public static void CreateExplosionEffect(Vector3 position)
    {
        Instantiate(instance.explosionPrefab, position, Quaternion.identity);
    }
}
