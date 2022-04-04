using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : Entity
{
    protected override void RegEntity()
    {
        Engine.bonuses.Add(this);
    }

    protected override void UnregEntity()
    {
        Engine.bonuses.Remove(this);
    }
}
