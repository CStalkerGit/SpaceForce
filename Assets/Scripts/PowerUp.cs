using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Entity
{
    [Header("PowerUp Effects")]
    [Tooltip("Восстанавливает утраченное здоровье, не превышая максимум.")]
    public int restoreHealth;
    [Tooltip("Улучшает вооружение игрока. Null, если это не нужно.")]
    public Upgrade upgradePrefab;

    const float speed = 1.5f;

    new void FixedUpdate()
    {
        base.FixedUpdate();

        // поверапы медленно опускаются вниз по карте 
        Vector3 pos = transform.position;
        pos.y -= speed * Time.deltaTime * speed;
        transform.position = pos;
    }

    public void Apply()
    {
        if (IsDead) return;

        if (restoreHealth > 0) Player.RestoreHealth(restoreHealth);
        if (upgradePrefab) Player.AddWeapon(upgradePrefab);
    }

    // override methods

    // регистрация бонуса в списке активных бонусов на экране
    protected override void RegEntity()
    {
        Engine.bonuses.Add(this);
    }

    protected override void UnregEntity()
    {
        Engine.bonuses.Remove(this);
    }
}
