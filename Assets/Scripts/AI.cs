using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class AI : MonoBehaviour
{
    // AI data
    [Tooltip("Набор паттернов")]
    public Pattern pattern;
    [Tooltip("Таймер, после которого врагу надо будет улететь с экрана")]
    public float timer = 5.0f;
    public float vertSpeed = 3;
    public float horSpeed = 3;

    // components
    public Enemy enemyShip { get; private set; }

    // static
    private static readonly float SinWidth = 5;

    private float currentTimer;
    private float sinusoid;
    private Vector3 rndPoint;

    void Awake()
    {
        // components
        enemyShip = GetComponent<Enemy>();

        // extra data
        sinusoid = Mathf.Sin(transform.position.x / SinWidth);
        rndPoint = RandomCoord();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        const float moveEpsilon = 0.1f;

        Vector3 pos = transform.position;

        // таймер, после которого враг покидает экран
        if (pattern != Pattern.StraightDown)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > timer) pattern = Pattern.StraightDown;
        }

        bool movingDown = true;

        switch (pattern)
        {
            case Pattern.StraightDown:
                break;

            case Pattern.Sinusoid:
                // для паттерна синусоиды
                sinusoid += Time.deltaTime * 2;
                if (sinusoid > Mathf.PI) sinusoid -= Mathf.PI * 2;
                pos.x = Mathf.Sin(sinusoid) * SinWidth;   
                break;

            case Pattern.TowardPlayer:
                if (Player.position.x > transform.position.x - moveEpsilon) pos.x += horSpeed * Time.deltaTime;
                if (Player.position.x < transform.position.x + moveEpsilon) pos.x -= horSpeed * Time.deltaTime;
                break;

            case Pattern.Random:
                movingDown = false;
                Vector3 dir = rndPoint - transform.localPosition;
                if (Mathf.Abs(dir.x * dir.y) < 1) rndPoint = RandomCoord();
                dir.Normalize();
                pos += dir * horSpeed * Time.deltaTime;
                break;
        }  

        if (movingDown) pos.y -= vertSpeed * Time.deltaTime;

        transform.position = pos;
    }

    Vector3 RandomCoord()
    {
        float range = Engine.widthInTiles / 2;
        float x = Random.Range(-range, range);
        range = (Engine.heightInTiles - 1) / 2;
        float y = Random.Range(-range, range);
        return new Vector3(x, y, 0);
    }
}
