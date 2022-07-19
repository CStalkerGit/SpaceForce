using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float scrollSpeed;

    // Update is called once per frame
    void Update()
    {
        var v = transform.localPosition;
        v.y -= Time.deltaTime * scrollSpeed;
        if (v.y < -4) v.y += 8;
        transform.localPosition = v;
    }
}
