using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Projectile : Entity
{
    [Header("Projectile")]
    public int damage = 1;
    public bool continuousDamage;
    [Tooltip("Скорость полета снаряда, тайлы/секунда")]
    public float projSpeed;

    // physics
    private Vector3 velocity;
    private bool isEnemy;

    new protected void Awake()
    {
        base.Awake();
        var snd = GetComponent<AudioSource>();
        if (snd) snd.pitch = Random.Range(0.9f, 1.0f);
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        if (IsDead) return;

        transform.position += velocity * Time.deltaTime;
        
        if (isEnemy)
        {
            // проверка столкновения снарядов врагов с игроком
            if (Player.TestHit(this, damage)) Kill();
        }
        else
        {
            // проверка столкновения снарядов игрока с врагами
            var enemy = Engine.IsEnemyCollision(this);
            if (enemy)
            {
                // наносим урон врагу и уничтожаем снаряд
                enemy.Damage(damage);
                if (!continuousDamage) Kill(true);
            }
        }

        // проверка выхода за границу экрана
        if (Engine.OutOfBounds(transform.localPosition, true)) Kill();
    }

    /// <summary>
    /// Инициализация снаряда
    /// </summary>
    /// <param name="projSpeed">скорость снаряда тайлы/сек.</param>
    /// <param name="isEnemy">является ли снаряд вражеским или принадлежит игроку</param>
    public void Init(bool isEnemy)
    {
        this.isEnemy = isEnemy;
    }

    /// <summary>
    /// Направить направить снаряд в заданную точку
    /// </summary>
    /// <param name="direction">точка назначения</param>
    public void SetDestination(Vector3 destination)
    {
        SetDirection(destination - transform.position);
    }

    /// <summary>
    /// Направить снаряд по заданному направлению
    /// </summary>
    /// <param name="direction">направление полета снаряда</param>
    public void SetDirection(Vector3 direction)
    {
        direction.Normalize();
        velocity = direction * projSpeed;
    }

    // override methods

    // регистрация пока не требуется, посколько снаряды сами взаимодействуют с другими объектами
    protected override void RegEntity()
    {

    }

    protected override void UnregEntity()
    {

    }

    protected override void OnKillByEntity()
    {
        Engine.CreateExplosionEffect(transform.position);
    }
}
