using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Entity))]
public class TileObject : MonoBehaviour
{
    // components
    public SpriteRenderer spriteRenderer { get; private set; }
    public Entity entity { get; private set; }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        entity = GetComponent<Entity>();
    }

    void FixedUpdate()
    {

    }

    protected virtual void OnKillByEntity()
    {
        //Engine.CreateExplosionEffect(transform.position);
    }
}
