using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class AI : MonoBehaviour
{
    public AIPattern pattern;
    public float speed = 3;

    private static readonly float SinWidth = 5;

    private float sinusoid;

    public Enemy enemyShip { get; private set; }

    void Awake()
    {
        enemyShip = GetComponent<Enemy>();
        if (pattern == AIPattern.Sinusoid) sinusoid = Mathf.Sin(transform.position.x / SinWidth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // для паттерна синусоиды
        sinusoid += Time.deltaTime * 2;
        if (sinusoid > Mathf.PI) sinusoid -= Mathf.PI * 2;

        Vector3 pos = transform.position;

        pos.y -= speed * Time.deltaTime;
        if (pattern == AIPattern.Sinusoid) pos.x = Mathf.Sin(sinusoid) * SinWidth;

        transform.position = pos;
    }
}
