using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var v = transform.localPosition;
        v.y -= Time.deltaTime * 2;
        if (v.y < -4) v.y += 8;
        transform.localPosition = v;
    }
}
