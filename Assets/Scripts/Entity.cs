using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // inspector data
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

    public float PosX { get => transform.position.x; }
    public float PosY { get => transform.position.y; }

    /// <summary>
    /// Объект бездействует, пока не появится на экране
    /// </summary>
    public bool IsDormant { get; private set; }
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
        IsDormant = true;

        // устанавливаем первый кадр анимации, если она доступна
        if (animationSprites.Length > 0) spriteRenderer.sprite = animationSprites[0];

        isTileObject = GetComponent<Structure>();
    }

    protected void FixedUpdate()
    {
        // проверка на бездействие объекта (такие находятся вне экрана игрока)
        if (IsDormant)
        {
            // бездействующий объект двигается вниз одновременно вместе с картой тайлов
            Vector3 pos = transform.position;
            pos.y -= Engine.scrollingSpeed * Time.deltaTime;
            if (!isTileObject) transform.position = pos; // FIXME тайловые объекты сдвигаются вместе с картой

            // если объект при этом показался на экране
            if (Engine.OutOfBounds(transform.position, 1.0f) == false)
            {
                RegEntity(); // зарегистрировать, чтобы включить взаимодействие игрока с этим объектом
                IsDormant = false; // объект больше не бездействует 
            }
        }
        else
        {
            if (Engine.OutOfBounds(transform.position, 1.0f)) OnOutOfBounds();
        }

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

    /// <summary>
    /// Функция вызывается при выходе объекта с экрана
    /// </summary>
    protected virtual void OnOutOfBounds()
    {
        Kill();
    }
}