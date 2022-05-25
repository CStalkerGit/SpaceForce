using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // data
    [Header("Data")]
    [Tooltip("Размер хитбокса в тайлах")]
    public float hitboxRadius = 0.5f;
    public int health = 1;

    // properties 
    public float PosX { get => transform.position.x; }
    public float PosY { get => transform.position.y; }
    public bool IsDead { get; private set; } // объект был уничтожен

    private bool isTileObject; // FIXME

    // smooth movement
    Vector3 smoothBegin;
    Vector3 smoothEnd;

    protected void Awake()
    {
        isTileObject = GetComponent<Structure>();
    }

    private void Start()
    {
        RegEntity();
    }

    protected void FixedUpdate()
    {
        if (Engine.OutOfBounds(transform.position, false)) Kill();

        // interpolation
        //smoothBegin = smoothEnd;
        //smoothEnd = transform.position;
    }

    void OnDrawGizmos()
    {
        // отрисовка хитбокса объекта в режиме отладки
        Gizmos.DrawWireCube(transform.position, new Vector3(hitboxRadius * 2, hitboxRadius * 2, 0.01f));
    }

    public bool IsCollission(Entity entity)
    {
        // проверка, не находится ли объект вне экрана
        if (Engine.OutOfBounds(transform.position, true)) return false;

        // проверка, не был ли объект уже уничтожен
        if (IsDead) return false;

        // проверка на столкновение хитбоксов двух объектов
        if (Mathf.Abs(PosX - entity.PosX) > (hitboxRadius + entity.hitboxRadius)) return false;
        if (Mathf.Abs(PosY - entity.PosY) > (hitboxRadius + entity.hitboxRadius)) return false;

        // столкновение произошло
        return true;
    }

    /// <summary>
    /// Уничтожить объект
    /// </summary>
    public void Damage(int amountDamage)
    {
        if (IsDead == false)
        {
            health -= amountDamage;
            if (health <= 0) Kill(true);
        }
    }

    /// <summary>
    /// Уничтожить объект
    /// </summary>
    /// <param name="byEntity"/>Объект был уничтожен другим объектом, для вызова соответствующих обработчиков</param>
    public void Kill(bool byEntity = false)
    {
        if (IsDead) return; // объект был уже уничтожен ранее

        IsDead = true;
        UnregEntity();

        Destroy(gameObject);
        if (byEntity) OnKillByEntity(); // нужно вызывать в самом конце
    }

    // virtual methods

    // регистрация объекта в нужном списке, для последующей проверки на столкновения
    // переопределяется в производных классах при необходимости
    protected virtual void RegEntity()
    {
        //Debug.LogWarning("No virtual method override");
    }

    protected virtual void UnregEntity()
    {
        //Debug.LogWarning("No virtual method override");
    }

    /// <summary>
    /// Функция вызывается при уничтожении объекта с помощью другого entity (снаряда или кораблем)
    /// Функция НЕ вызывается, если объект исчезает за пределами экрана или при закрытии сцены
    /// </summary>
    protected virtual void OnKillByEntity()
    { 

    }
}