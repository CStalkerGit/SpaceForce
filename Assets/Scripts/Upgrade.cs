using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Orientation
{
    Top,
    Side,
    Bottom,
    Option
}

public class Upgrade : MonoBehaviour
{
    public Orientation orientation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool TestCompatibibly()
    {
        return false;
    }
}
