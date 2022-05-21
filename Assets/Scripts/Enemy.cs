using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    new void FixedUpdate()
    {
        base.FixedUpdate();

        // Delete the enemy on player collision
        if (Player.TestHit(this)) Kill();
    }

    // override methods

    // Adding the enemy ship to the list of foes currently visible on the screen
    protected override void RegEntity()
    {
        Engine.enemies.Add(this);
    }

    protected override void UnregEntity()
    {
        Engine.enemies.Remove(this);
    }

    protected override void OnKillByEntity()
    {
        Engine.CreateExplosionEffect(transform.position);
    }
}
