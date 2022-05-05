using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : Entity
{
    Vector3Int location;
    float timer = 9999;
    bool isSelfDestruct;

    public void Init(Vector3Int location)
    {
        this.location = location;
    }

    public void SelfDestruct()
    {
        if (!isSelfDestruct)
        {
            isSelfDestruct = true;
            timer = Random.Range(0.3f, 0.6f);
        }
    }

    void Update()
    {
        if (isSelfDestruct && (health > 0))
        {
            timer -= Time.deltaTime;
            if (timer < 0) Kill(true);
        }
    }

    protected override void OnKillByEntity()
    {
        Engine.CreateExplosionEffect(transform.position);
        Engine.DestroyNeighbors(location);

        // метод меняет тайл и немедленно уничтожает связанный с ним gameObject,
        // поэтому должнен быть вызван в последнюю очередь
        Engine.CreateСrater(location); 
    }
}
