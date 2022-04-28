using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : Entity
{
    protected override void OnKillByEntity()
    {
        Engine.CreateExplosionEffect(transform.position);

        // метод меняет тайл и немедленно уничтожает связанный с ним gameObject,
        // поэтому должнен быть вызван в последнюю очередь
        Engine.CreateСrater(transform.position); 
    }
}
