using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public static Vector3 position { get; private set; }

    private static Entity playerEntity;
    private static float invulnerabilityTime = 2.0f; // временная неуязвимость после столкновения с врагом

    void Awake()
    {
        playerEntity = GetComponent<Entity>();
    }

    private void Start()
    {
        HUD.UpdateHealthBar(playerEntity.health);
    }

    void FixedUpdate()
    {
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

        // временная неуязвимость после столкновения с врагом
        invulnerabilityTime -= Time.deltaTime;

        // проверка на находящиеся поблизости бонусы, которые может подобрать игрок
        Entity bonus = Engine.IsBonusCollision(playerEntity);
        if (bonus) bonus.Kill();
    }

    public static bool TestHit(Entity entity)
    {
        if (playerEntity.IsCollission(entity))
        {
            if (invulnerabilityTime < 0)
            {
                invulnerabilityTime = 1.0f; // временная неуязвимость
                playerEntity.Damage(1);
                HUD.UpdateHealthBar(playerEntity.health);
            }
            return true;
        }
        return false;
    }
}
