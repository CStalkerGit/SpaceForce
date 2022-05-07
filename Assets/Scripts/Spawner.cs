﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// класс для спавна врагов и бонусов
public class Spawner : MonoBehaviour
{
    [Tooltip("Массив префабов врагов, которых можно спавнить")]
    public Enemy[] enemyPrefabs;

    private int waveCount;
    private float spawnDelay = spawnDelayTime;

    // время в миллисекундах до спавна следующей волны
    const float spawnDelayTime = 0.0f;
    // граница спавна по горизонтали с небольшим отступом от краев экрана
    const float spawnBorderX = (Engine.widthInTiles - 1) / 2;
    // граница спавна по вертикали, за пределами экрана
    const float spawnBorderY = (Engine.heightInTiles + 1) / 2; 

    // Update is called once per frame
    void FixedUpdate()
    {
        // проверка на необходимость спавна следующей волны монстров
        if (Engine.enemies.Count == 0)
        {
            // отсрочка спавна
            spawnDelay -= Time.deltaTime;

            if (spawnDelay <= 0 && waveCount < 3) NextWave();
        }
    }

    void NextWave()
    {
        waveCount++;

        spawnDelay = spawnDelayTime;
        var enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        switch(enemy.wavePattern)
        {
            case WavePattern.Random:
                RandomPattern(enemy);
                break;
        }
    }

    void RandomPattern(Enemy enemy)
    {
        int count = Random.Range(10, 15);

        for (int i = 0; i < count; i++)
        {
            var position = new Vector3(Random.Range(-spawnBorderX, spawnBorderX), spawnBorderY + i / 2.0f, 0);
            Instantiate(enemy, position, Quaternion.identity);
        }
    }
}
