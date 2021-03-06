using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Engine : MonoBehaviour
{
    public GameMap map;
    public Effect explosionPrefab;
    public AudioClip sndExplosion;

    // interpolation
    public static float alpha = 0;

    public const float GameAspect = 0.6f;
    public const float scrollingSpeed = 3.0f;
    public const float PPU = 16; // pixels per unit
    public const int widthInTiles = 12;
    public const int heightInTiles = 20;

    public static Engine Instance { get; private set; }
    public static List<Entity> enemies = new List<Entity>();
    public static List<Entity> bonuses = new List<Entity>();

    public static bool IsPaused {get; set;}

    // для смены сцены в случае победы или поражения
    private bool changeScene;
    private bool victory;
    private float changeSceneTime = 2.5f;

    Engine()
    {
        IsPaused = false;
        Instance = this;
    }

    private void Awake()
    {
        enemies.Clear();
        bonuses.Clear();
    }

    void Update()
    {
        alpha = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPaused) Options.Show(); else Options.Hide();
        }
    }

    private void FixedUpdate()
    {
        // проверка на необходимость загрузки следующей сцены (титульного экрана или следующего уровня)
        if (changeScene)
        {
            changeSceneTime -= Time.deltaTime;
            if (changeSceneTime < 0) Exit();
            return;
        }

        // проверка на гибель игрока
        if (Player.IsPlayerDead()) EndStage(false);

        // проверка на конец карты
        //if (map.IsMapEnd()) changeScene = true;
    }

    public static void EndStage(bool victory)
    {
        if (Instance.changeScene) return;

        Instance.victory = victory;
        Instance.changeScene = true;
    }

    public void Exit()
    {
        if (victory) SceneManager.LoadScene("EndStage");
        else SceneManager.LoadScene("TitleScreen");
    }

    /// <summary>
    /// Проверка, не вышел ли объект за границы экрана
    /// </summary>
    /// <param name="position">текущая позиция объекта</param>
    /// <param name="upperBorder">нужно ли учитывать верхнюю границу экрана</param>
    public static bool OutOfBounds(Vector3 position, bool upperBorder)
    {
        return Instance.map.OutOfBounds(position, upperBorder);
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
        Structure structure = Instance.map.GetTileObject(entity.transform.localPosition);
        if (structure)
            if (structure.IsCollission(entity)) return structure;


        return null;
    }

    /// <summary>
    /// Проверка взаимодействия игрока с бонусом
    /// </summary>
    public static PowerUp IsBonusCollision(Entity entity)
    {
        foreach (var bonus in bonuses)
        {
            if (bonus.IsCollission(entity)) return (PowerUp)bonus;
        }

        return null;
    }

    /// <summary>
    /// Проверка взаимодействия игрока с бонусом
    /// </summary>
    public static Entity GetNearestEnemy(Entity entity)
    {
        return null;
    }

    /// <summary>
    /// Создает эффект взрыва 
    /// </summary>
    public static void CreateExplosionEffect(Vector3 position)
    {
        Instantiate(Instance.explosionPrefab, position, Quaternion.identity);
    }

    /// <summary>
    /// Создает эффект воронки при взрыве здания на карте. При этом уничтожается gameObject, связанный с тайлом!
    /// </summary>
    public static void CreateСrater(Vector3Int coord)
    {
        Instance.map.CreateCrater(coord);
    }

    /// <summary>
    /// Включить таймер для самоуничтожения соседних тайлов
    /// </summary>
    public static void DestroyNeighbors(Vector3Int position)
    {
        for (int x = position.x - 1; x <= position.x + 1; x += 2)
        {
            Structure structure = Instance.map.GetTileObject(x, position.y);
            if (structure) structure.SelfDestruct();
        }

        for (int y = position.y - 1; y <= position.y + 1; y += 2)
        {
            Structure structure = Instance.map.GetTileObject(position.x, y);
            if (structure) structure.SelfDestruct();
        }
    }
}
