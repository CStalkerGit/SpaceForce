using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Engine : MonoBehaviour
{
    public GameMap map;
    public Effect explosionPrefab;
    public TileBase crater;

    // interpolation
    public static float alpha = 0;

    public const float GameAspect = 0.6f;
    public const float scrollingSpeed = 2.5f;
    public const float PPU = 16; // pixels per unit

    private static Engine instance;
    public static List<Entity> enemies = new List<Entity>();
    public static List<Entity> bonuses = new List<Entity>();

    // для смены сцены в случае победы или поражения
    private bool changeScene = false;
    private float changeSceneTime = 2.5f;


    private void Awake()
    {
        instance = this;

        enemies.Clear();
        bonuses.Clear();
    }

    void Update()
    {
        alpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        // проверка на необходимость загрузки следующей сцены (титульного экрана или следующего уровня)
        if (changeScene)
        {
            changeSceneTime -= Time.deltaTime;
            if (changeSceneTime < 0) SceneManager.LoadScene("TitleScreen");
            return;
        }

        // проверка на гибель игрока
        if (Player.IsPlayerDead()) changeScene = true;

        // проверка на конец карты
        if (map.IsMapEnd()) changeScene = true;
    }

    /// <summary>
    /// Проверка, не вышел ли объект за границы экрана
    /// </summary>
    /// <param name="position">текущая позиция объекта</param>
    /// <param name="offset">дополнительное смещение от краев экрана</param>
    public static bool OutOfBounds(Vector3 position, float offset)
    {
        return instance.map.OutOfBounds(position, offset);
    }

    /// <summary>
    /// Проверка, не вышел ли объект за нижнюю границу экрана
    /// </summary>
    /// <param name="position">текущая позиция объекта</param>
    /// <param name="offset">смещение от нижнего края экрана</param>
    public static bool OutOfBoundsBottom(Vector3 position, float offset)
    {
        if (position.y < 0 - offset) return true;
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

        // получение объекта, находящегося на тайле
        Entity tileObject = instance.map.GetTileObject(entity.transform.localPosition);
        if (tileObject)
            if (tileObject.IsCollission(entity)) return tileObject;


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
    /// Создает эффект взрыва 
    /// </summary>
    public static void CreateExplosionEffect(Vector3 position)
    {
        Instantiate(instance.explosionPrefab, position, Quaternion.identity);
    }

    /// <summary>
    /// Создает эффект воронки при взрыве здания на карте. При этом уничтожается gameObject, связанный с тайлом!
    /// </summary>
    public static void CreateСrater(Vector3 position)
    {
        instance.map.SetTile(instance.crater, position);
    }
}
