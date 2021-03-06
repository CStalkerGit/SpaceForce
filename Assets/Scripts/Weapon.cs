using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode
{
    Random,
    ToDirection,
    NearestEnemy,
    Spread,
    Double
}

public class Weapon : MonoBehaviour
{
    [Header("Weapon settings")]
    [Tooltip("Скорострельность в секундах")]
    public float firerate = 3.0f;
    [Tooltip("Оружие стреляет автоматически")]
    public bool automaticFire;

    [Header("Fire settings")]
    [Tooltip("Режим стрельбы")]
    public FireMode fireMode;
    [Tooltip("Направление выстрела для режима стрельбы ToDirection")]
    public Vector3 toDirection;
    [Tooltip("Смещение точки выстрела от центра стреляющего")]
    public Vector2 projStartPoint;

    [Header("Projectile")]
    public Projectile projPrefab;

    float shootDelay;
    bool isEnemy; // принадлежит ли оружие (и выпускаемые им снаряды) врагу

    void Start()
    {
        Entity entity = GetComponent<Entity>();
        if (GetComponent<Enemy>()) isEnemy = true; else isEnemy = false;
        if (automaticFire) shootDelay = Random.Range(0, firerate); // для случайного времени начала стрельбы
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

        Projectile projectile = NewProjectile();

        switch (fireMode)
        {
            case FireMode.Random:
                {
                    float angle = Random.Range(-Mathf.PI, Mathf.PI);
                    Vector3 randomDirection = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
                    projectile.SetDirection(randomDirection);
                    break;
                }

            case FireMode.ToDirection:
                {
                    projectile.SetDirection(toDirection);
                    break;
                }

            case FireMode.NearestEnemy:
                {
                    if (isEnemy) projectile.SetDestination(Player.position);
                    break;
                }

            case FireMode.Spread:
                {
                    projectile.SetDirection(new Vector3(0, 1, 0));

                    for (float f = -Mathf.PI; f < Mathf.PI; f += Mathf.PI / 4)
                    {
                        Projectile projectile2 = NewProjectile();
                        projectile2.Init(isEnemy);
                        projectile2.SetDirection(new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0));
                    }

                    break;
                }

            case FireMode.Double:
                {
                    projectile.SetDirection(toDirection);

                    projectile = NewProjectile();
                    projectile.transform.position = transform.position 
                        + new Vector3(-projStartPoint.x, projStartPoint.y, transform.position.z);
                    projectile.SetDirection(toDirection);
                    break;
                }

            default: throw new System.ArgumentException("fireDirection is unknown");
        }
    }

    public Projectile NewProjectile()
    {
        Projectile projectile = Instantiate(projPrefab);
        projectile.transform.position = transform.position + (Vector3)projStartPoint;
        projectile.Init(isEnemy);
        return projectile;
    }

}
