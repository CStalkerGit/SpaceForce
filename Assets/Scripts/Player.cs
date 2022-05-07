﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public float speed;
    public const int maxHealth = 5;
    public const float invulnTime = 2.0f; // время неуязвимости после столкновения с врагом

    // TODO заменить на специальный класс
    public Weapon[] weapons2;
    public Weapon[] weapons3;

    public static Vector3 position { get; private set; }

    private static Player instance;
    private static float invulnerabilityTime = -1; // временная неуязвимость после столкновения с врагом
    private SpriteRenderer sprShield; // спрайт защитного поля для эффекта неуязвимости
    private static int weaponLevel; // уровень прокачки оружия

    private void Start()
    {
        instance = this;
        sprShield = transform.Find("Shield").GetComponent<SpriteRenderer>();
        weaponLevel = 1;

        HUD.UpdateHealthBar(health);
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.localPosition; // направление движения объекта

        float distance = speed * Time.deltaTime;

        Vector3 pos = transform.localPosition;
        pos.y += Engine.scrollingSpeed * Time.deltaTime;
        transform.localPosition = pos;

        // во избежание дрожания объекта
        if (direction.x * direction.x + direction.y * direction.y <= distance * distance)
        {
            mousePosition.z = 0;
            transform.localPosition = mousePosition;
        }
        else
        {
            direction.z = 0;
            direction.Normalize();
            direction *= distance;
            transform.localPosition += direction;
        }

        // ограничение движения игрока за пределы экрана
        float offsetY = Camera.main.transform.position.y;
        float boundY = Camera.main.orthographicSize;
        float boundX = Camera.main.orthographicSize * Engine.GameAspect;
        float x = Mathf.Clamp(transform.localPosition.x, -boundX, boundX);
        float y = Mathf.Clamp(transform.localPosition.y, -boundY + offsetY, boundY + offsetY);
        transform.localPosition = new Vector3(x, y, 0);

        // горизонтальный скроллинг камеры, если она не полностью помещается на экране игрока
        Vector3 cameraPos = Camera.main.transform.position;
        if (Camera.main.aspect < Engine.GameAspect)
        {
            float aspect = 1 - (Camera.main.pixelWidth * Camera.main.aspect) / (Camera.main.pixelWidth * Engine.GameAspect);
            cameraPos.x = transform.localPosition.x * aspect;
        }
        else cameraPos.x = 0;
        Camera.main.transform.position = cameraPos;

        // позиция игрока для стрельбы врагами по нему 
        position = transform.position;

        // обновить время временной неуязвимости
        if (invulnerabilityTime > 0)
        {
            invulnerabilityTime -= Time.deltaTime;
            if (invulnerabilityTime < 0)
            {
                invulnerabilityTime = -1;
                instance.sprShield.gameObject.SetActive(false); // убрать защитное поле вокруг игрока
            }
        }

        // проверка на находящиеся поблизости бонусы, которые может подобрать игрок
        Bonus bonus = Engine.IsBonusCollision(this);
        if (bonus)
        {
            bonus.Apply(); // применить бонус на игроке
            bonus.Kill(); // уничтожить бонус
        }
    }

    public static bool TestHit(Entity entity)
    {
        if (!instance) return false;

        if (instance.IsCollission(entity))
        {
            if (invulnerabilityTime < 0)
            {
                SetInvulnerability(invulnTime); // временная неуязвимость
                instance.Damage(1); // отнимаем одно сердечко
                HUD.UpdateHealthBar(instance.health);
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Включить временную неуязвимость
    /// </summary>
    public static void SetInvulnerability(float timeInSeconds)
    {
        if (!instance) return;

        invulnerabilityTime = timeInSeconds;
        instance.sprShield.gameObject.SetActive(true); // показать защитное поле вокруг игрока
    }

    public static void RestoreHealth(int amountHealth)
    {
        if (!instance) return;

        instance.health += amountHealth;
        instance.health = Mathf.Clamp(instance.health, 1, maxHealth);
        HUD.UpdateHealthBar(instance.health);
    }

    // TODO переписать на нормлаьный код
    public static void UpgradeWeapon()
    {
        if (!instance) return;

        weaponLevel++;

        switch (weaponLevel)
        {
            case 2:
                EnableWeapon(instance.weapons2);
                break;
            case 3:
                EnableWeapon(instance.weapons3);
                break;
        }
    }

    public static void EnableWeapon(Weapon[] weapons)
    {
        foreach (var weapon in weapons)
            weapon.gameObject.SetActive(true);
    }

    /// <summary>
    /// Проверка на гибель игрока
    /// </summary>
    public static bool IsPlayerDead()
    {
        if (!instance) return true;
        return instance.IsDead;
    }
}
