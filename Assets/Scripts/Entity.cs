using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // entity data
    [Header("Data")]
    [Tooltip("Размер хитбокса в тайлах")]
    public float hitboxRadius = 0.5f;
    public int health = 1;

    [Header("Sprite Animation")]
    [Tooltip("Массив спрайтов, если нужна покадровая анимация")]
    public Sprite[] animationSprites;
    [Tooltip("Скорость смены кадров анимации, в секундах")]
    public float animationSpeed = 0.25f;

    // components
    public SpriteRenderer spriteRenderer { get; private set; }

    // properties 
    public float PosX { get => transform.position.x; }
    public float PosY { get => transform.position.y; }
    public bool IsDead { get; private set; } // объект был уничтожен

    // sprite animation
    // TODO перенести анимацию в отдельный класс
    private float animationTime;
    private int animationFrame; // текущий кадр анимации

    private bool isTileObject; // FIXME

    // smooth movement
    Vector3 smoothBegin;
    Vector3 smoothEnd;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // устанавливаем первый кадр анимации, если она доступна
        if (animationSprites.Length > 0) spriteRenderer.sprite = animationSprites[0];

        isTileObject = GetComponent<Structure>();
    }

    private void Start()
    {
        RegEntity();
    }

    protected void FixedUpdate()
    {
        if (Engine.OutOfBounds(transform.position, false)) Kill();

        // покадровая анимация спрайта, если доступна (есть 2 спрайта или больше)
        if (animationSprites.Length > 1)
        {
            animationTime += Time.deltaTime;

            // смена кадра анимации
            if (animationTime >= animationSpeed)
            {
                animationTime = 0;
                animationFrame++;
                if (animationFrame >= animationSprites.Length) animationFrame = 0;
                spriteRenderer.sprite = animationSprites[animationFrame];
            }
        }

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