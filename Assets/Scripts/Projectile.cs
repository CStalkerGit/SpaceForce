using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    [Header("Projectile")]
    public int damage = 1;

    [Header("Sprite Animation")]
    public Sprite[] animationSprites;
    public float animationSpeed = 0.25f;

    // physics
    float speed;
    Vector3 velocity;

    // sprite animation
    float animationTime;
    int animationFrame;

    void FixedUpdate()
    {
        transform.position += velocity * Time.deltaTime;

        // покадровая анимация спрайта, если доступна
        if (animationSprites.Length > 0)
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
        
        if (isEnemy)
        {
            // проверка столкновения снарядов врагов с игроком
            if (Player.TestHit(this)) Kill();
        }
        else
        {
            // проверка столкновения снарядов игрока с тайловыми объектами
            var enemy = Engine.IsEnemyCollision(this);
            if (enemy)
            {
                // наносим урон врагу и уничтожаем снаряд
                enemy.Damage(damage);
                Kill();
            }
        }

        // проверка выхода за границу экрана
        if (Engine.OutOfBounds(transform.localPosition, 2.0f)) Kill();
    }

    /// <summary>
    /// Инициализация снаряда
    /// </summary>
    /// <param name="projSpeed">скорость снаряда тайлы/сек.</param>
    /// <param name="isEnemy">является ли снаряд вражеским или принадлежит игроку</param>
    public void Init(float projSpeed, bool isEnemy)
    {
        this.isEnemy = isEnemy;
        speed = projSpeed;
    }

    /// <summary>
    /// Направить направить снаряд в заданную точку
    /// </summary>
    /// <param name="direction">точка назначения</param>
    public void SetDestination(Vector3 destination)
    {
        velocity = destination - transform.position;
        velocity.Normalize();
        velocity *= speed;
    }

    /// <summary>
    /// Направить снаряд по заданному направлению
    /// </summary>
    /// <param name="direction">направление полета снаряда</param>
    public void SetDirection(Vector3 direction)
    {
        direction.Normalize();
        velocity = direction * speed;
    }
}
