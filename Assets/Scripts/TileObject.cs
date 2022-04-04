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
        // проверка выхода за границу экрана
        if (Engine.OutOfBoundsBottom(transform.localPosition, 1.0f)) entity.Kill();

        //Vector3 newDirection = Player.position - transform.position;
        //newDirection.z = 0;
        //newDirection.Normalize();

        //transform.rotation = Quaternion.LookRotation(Vector3.forward, newDirection);
    }
}
