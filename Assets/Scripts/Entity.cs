using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Entity : MonoBehaviour
{
    // inspector data
    [Tooltip("Размер хитбокса в тайлах")]
    public float hitboxRadius = 0.5f;
    [Tooltip("Объект является врагом или принадлежит игроку")]
    public bool isEnemy = true;

    public int health = 1;

    // components
    public SpriteRenderer spriteRenderer { get; private set; }

    public float PosX { get => transform.position.x; }
    public float PosY { get => transform.position.y; }

    public bool IsDormant { get; private set; } // объект бездействует, пока не появится на экране
    public bool IsDead { get; private set; } // объект был уничтожен

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        IsDormant = true;
    }

    protected void FixedUpdate()
    {
        if (IsDormant)
        {
            // бездействующий объект двигается вниз вместе с картой
            Vector3 pos = transform.position;
            pos.y -= Engine.scrollingSpeed * Time.deltaTime;
            transform.position = pos;

            // если объект показался на экране
            if (Engine.OutOfBounds(transform.position, 1.0f) == false)
            {
                // зарегистрировать, чтобы включить взаимодействие игрока с этим объектом
                RegEntity();
                IsDormant = false;
            }
        }
        else
        {
            // Если уже активированный объект вышел за границы экрана, то его надо уничтожить
            if (Engine.OutOfBounds(transform.position, 1.0f)) Kill();
        }
    }

    void OnDrawGizmos()
    {
        // отрисовка хитбокса объекта в режиме отладки
        Gizmos.DrawWireCube(transform.position, new Vector3(hitboxRadius * 2, hitboxRadius * 2, 0.01f));
    }

    public bool IsCollission(Entity entity)
    {
        // проверка, не находится ли объект вне экрана
        if (IsDormant) return false;
        // проверка, не был ли объект уже уничтожен
        if (IsDead) return false;

        // проверка на столкновение хитбоксов двух объектов
        if (Mathf.Abs(PosX - entity.PosX) > (hitboxRadius + entity.hitboxRadius)) return false;
        if (Mathf.Abs(PosY - entity.PosY) > (hitboxRadius + entity.hitboxRadius)) return false;

        // столкновение произошло
        return true;
    }

    /// <summary>
    /// Получить позицию объекта для TileMap
    /// </summary>
    public Vector3Int GetTilePosition(Vector3 tilemapPos)
    {
        Vector3 pos = transform.localPosition - tilemapPos;
        return new Vector3Int(Mathf.RoundToInt(pos.x - 0.5f), Mathf.RoundToInt(pos.y - 0.5f), 0);
    }

    /// <summary>
    /// Уничтожить объект
    /// </summary>
    public void Damage(int damageAmount)
    {
        if (IsDead == false)
        {
            health -= damageAmount;
            if (health <= 0) Kill();
        }
    }

    /// <summary>
    /// Уничтожить объект
    /// </summary>
    public void Kill()
    {
        if (IsDead) return;

        IsDead = true;
        Destroy(gameObject);
        UnregEntity();
    }

    // override

    protected virtual void RegEntity()
    {
        
    }

    protected virtual void UnregEntity()
    {

    }
}