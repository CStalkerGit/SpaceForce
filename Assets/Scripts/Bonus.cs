using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : Entity
{
    [Header("Bonus Effects")]
    [Tooltip("Восстанавливает утраченное здоровье, не превышая максимум")]
    public int restoreHealth;
    [Tooltip("Улучшает вооружение игрока")]
    public bool upgradeWeapon;

    const float speed = 1.5f;

    new void FixedUpdate()
    {
        base.FixedUpdate();

        // бонусы медленно опускаются вниз по карте 
        if (!IsDormant)
        {
            Vector3 pos = transform.position;
            pos.y -= speed * Time.deltaTime;
            transform.position = pos;
        }
    }

    public void Apply()
    {
        if (IsDead) return;

        if (restoreHealth > 0) Player.RestoreHealth(restoreHealth);
        if (upgradeWeapon) Player.UpgradeWeapon();
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
