using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    // data
    [Header("Sprite Animation")]
    [Tooltip("Массив спрайтов, если нужна покадровая анимация")]
    public Sprite[] sprites;
    [Tooltip("Скорость смены кадров анимации, в секундах")]
    public float speed = 0.25f;
    public bool loop = true;
    public bool autoPlay = true;

    // properties 
    public bool IsPlaying { get; private set; }

    // components
    public SpriteRenderer spriteRenderer { get; private set; }

    // animation
    private float currentTime;
    private int frame; // текущий кадр анимации

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // устанавливаем первый кадр анимации, если она доступна
        if (sprites.Length > 0) spriteRenderer.sprite = sprites[0];
        IsPlaying = sprites.Length > 1  ? autoPlay : false;
    }

    protected void FixedUpdate()
    {
        // покадровая анимация спрайта
        if (IsPlaying)
        {
            currentTime += Time.deltaTime;

            // смена кадра анимации
            if (currentTime >= speed)
            {
                currentTime = 0;
                frame++;
                if (frame >= sprites.Length)
                {
                    frame = 0;
                    if (loop == false)
                    {
                        frame = sprites.Length - 1;
                        IsPlaying = false;
                    }
                }

                spriteRenderer.sprite = sprites[frame];
            }
        }
    }
}
