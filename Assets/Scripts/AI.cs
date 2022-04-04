using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyShip))]
public class AI : MonoBehaviour
{
    // components
    public EnemyShip enemyShip { get; private set; }

    void Awake()
    {
        enemyShip = GetComponent<EnemyShip>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemyShip.IsDormant) return;

        Vector3 pos = transform.position;

        pos.y -= 3 * Time.deltaTime;

        transform.position = pos;
    }
}
