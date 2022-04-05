using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public float speed;
    public const int maxHealth = 5;
    public const float invulnTime = 2.0f; // время неуязвимости после столкновения с врагом

    public static Vector3 position { get; private set; }

    private static Player instance;
    private static float invulnerabilityTime = 2.0f; // временная неуязвимость после столкновения с врагом
    private SpriteRenderer sprShield; // спрайт защитного поля для эффекта неуязвимости

    private void Start()
    {
        instance = this;
        sprShield = transform.Find("Shield").GetComponent<SpriteRenderer>();

        HUD.UpdateHealthBar(health);
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.localPosition; // направление движения объекта

        float distance = speed * Time.deltaTime;

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
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        float x = Mathf.Clamp(transform.localPosition.x, -width / 2, width / 2);
        float y = Mathf.Clamp(transform.localPosition.y, -height / 2, height / 2);
        transform.localPosition = new Vector3(x, y, 0);

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
}
