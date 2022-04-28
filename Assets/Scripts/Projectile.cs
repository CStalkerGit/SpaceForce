using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Projectile : Entity
{
    [Header("Projectile")]
    public int damage = 1;

    // physics
    float speed;
    Vector3 velocity;

    // data
    private bool isEnemy;

    new void FixedUpdate()
    {
        base.FixedUpdate();

        transform.position += velocity * Time.deltaTime;
        
        if (isEnemy)
        {
            // проверка столкновения снарядов врагов с игроком
            if (Player.TestHit(this)) Kill();
        }
        else
        {
            // проверка столкновения снарядов игрока с врагами
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

    // override methods

    // регистрация пока не требуется, посколько снаряды сами взаимодействуют с другими объектами
    protected override void RegEntity()
    {

    }

    protected override void UnregEntity()
    {

    }
}
