using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyShip))]
public class AI : MonoBehaviour
{
    public bool patternSin;
    public float speed = 3;

    private float sinusoid;

    // components
    // test comment
    // test comment 2
    public EnemyShip enemyShip { get; private set; }

    void Awake()
    {
        enemyShip = GetComponent<EnemyShip>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyShip.IsDormant) return;

        // для паттерна синусоиды
        sinusoid += Time.deltaTime;
        if (sinusoid > Mathf.PI) sinusoid -= Mathf.PI * 2;

        Vector3 pos = transform.position;

        pos.y -= speed * Time.deltaTime;
        if (patternSin) pos.x = Mathf.Sin(sinusoid) * 5;

        transform.position = pos;
    }
}
