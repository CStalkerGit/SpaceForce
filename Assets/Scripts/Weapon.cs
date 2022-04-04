using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireDirection
{
    Random,
    NearestEnemy,
    Up,
    Down
}

public class Weapon : MonoBehaviour
{
    [Header("Weapon settings")]
    [Tooltip("Скорострельность в секундах")]
    public float firerate = 3.0f;
    [Tooltip("Оружие стреляет автоматически")]
    public bool automaticFire;

    [Header("Fire settings")]
    [Tooltip("Направление вылета снарядов")]
    public FireDirection fireDirection;
    [Tooltip("Смещение точки выстрела от центра стреляющего")]
    public Vector2 projStartPoint;

    [Header("Projectile")]
    public Projectile projPrefab;
    [Tooltip("Скорость полета снаряда, тайлы/секунда")]
    public float projSpeed = 4.0f; 

    float shootDelay;
    bool isEnemy; // принадлежит ли оружие (и выпускаемые им снаряды) врагу

    void Start()
    {
        Entity entity = GetComponent<Entity>();
        if (entity) isEnemy = entity.isEnemy; else isEnemy = false;
    }

    void FixedUpdate()
    {
        if (shootDelay > 0) shootDelay -= Time.deltaTime;

        if (automaticFire) Shoot();
    }

    public void Shoot()
    {
        if (shootDelay > 0 || !projPrefab) return;

        shootDelay = firerate;

        Projectile projectile = Instantiate(projPrefab);
        projectile.transform.position = transform.position + (Vector3)projStartPoint;
        projectile.Init(projSpeed, isEnemy);

        switch (fireDirection)
        {
            case FireDirection.Random:
                {
                    float angle = Random.Range(-Mathf.PI, Mathf.PI);
                    Vector3 randomDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
                    projectile.SetDirection(randomDirection);
                    break;
                }

            case FireDirection.NearestEnemy:
                {
                    if (isEnemy) projectile.SetDestination(Player.position);
                    break;
                }

            case FireDirection.Up:
                {
                    projectile.SetDirection(new Vector3(0, 1, 0));
                    break;
                }

            case FireDirection.Down:
                {
                    projectile.SetDirection(new Vector3(0, -1, 0));
                    break;
                }

            default: throw new System.Exception("fireDirection is unknown");
        }
    }

}
