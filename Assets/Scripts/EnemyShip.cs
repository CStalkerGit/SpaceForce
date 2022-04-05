using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Entity
{
    new void FixedUpdate()
    {
        base.FixedUpdate();

        // если враг столкнулся с игроком, то врага нужно уничтожить.
        if (Player.TestHit(this)) Kill();
    }

    // override methods

    protected override void RegEntity()
    {
        Engine.enemies.Add(this);
    }

    protected override void UnregEntity()
    {
        Engine.enemies.Remove(this);
    }
}
