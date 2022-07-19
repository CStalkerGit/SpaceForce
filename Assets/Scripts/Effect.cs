using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO объединить с классом Entity
public class Effect : MonoBehaviour
{
    [Header("Sprite Animation")]
    [Tooltip("Массив спрайтов, если нужна покадровая анимация")]
    public Sprite[] animationSprites;
    [Tooltip("Скорость смены кадров анимации, в секундах")]
    public float animationSpeed = 0.25f;

    // components
    public SpriteRenderer spriteRenderer { get; private set; }

    // sprite animation
    private float animationTime;
    private int animationFrame; // текущий кадр анимации

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // анимация двигается вниз одновременно вместе с картой тайлов
        Vector3 pos = transform.position;
        pos.y -= Engine.scrollingSpeed * Time.deltaTime;
        transform.position = pos;

        // покадровая анимация спрайта, если доступна (есть 2 спрайта или больше)
        if (animationSprites.Length > 1)
        {
            animationTime += Time.deltaTime;

            // смена кадра анимации
            if (animationTime >= animationSpeed)
            {
                animationTime = 0;
                animationFrame++;
                if (animationFrame >= animationSprites.Length) Kill();
                else spriteRenderer.sprite = animationSprites[animationFrame];
            }
        }
    }

    /// <summary>
    /// Уничтожить анимацию (обычно происходит на последнем спрайте анимации)
    /// </summary>
    void Kill()
    {
        if (spriteRenderer.enabled)
        {
            spriteRenderer.enabled = false; // сразу скрыть объект с экрана до полного уничтожения
            Destroy(gameObject, 1.0f);
        }
    }
}
