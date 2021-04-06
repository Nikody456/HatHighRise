using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants 
{
    public static float PIXELS_PER_UNIT { get; private set; } = 32f;
    
    public static int PLAYER_LAYER { get; private set; } = 8;

    public static int HAT_LAYER { get; private set; } = 10;

    public static int IGNORE_LAYER { get; private set; } = 2;

    public static string HAT_SCORE_PATH { get; private set; } = "Hats/HatScore";
    public static string HAT_SCORE_KEY { get; private set; } = "SCORE";

    public static int HAT_SCORE { get; private set; } = 10;


}
