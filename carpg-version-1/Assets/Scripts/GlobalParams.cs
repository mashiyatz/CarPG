using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalParams
{
    public static float boundaryPosX = 25;
    public static float boundaryPosZ = 8;
    public static Direction attackDirection = Direction.None;
    public static float speedScale = 1f;

    public enum Direction
    {
        None,
        FORWARD,
        BACK,
        RIGHT,
        LEFT
    }

    public enum Archetype
    {
        SNIPER,
        BOMBER,
        DRIVER,
        MECH
    }

    public static Dictionary<Archetype, string> typeToAttack = new()
    {
            { Archetype.SNIPER, "Bullet" },
            { Archetype.BOMBER, "Jump" },
            { Archetype.DRIVER, "Bash" },
            { Archetype.MECH, "Bullet" }
    };

    public static Dictionary<Archetype, Color> typeToColor = new()
    {
            { Archetype.SNIPER, Color.red },
            { Archetype.BOMBER, Color.blue },
            { Archetype.DRIVER, Color.green },
            { Archetype.MECH, Color.yellow }
    };
}
