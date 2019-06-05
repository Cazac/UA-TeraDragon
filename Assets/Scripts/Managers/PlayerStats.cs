using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    static int MaxLives;
    static int CurrentLives;

    public static void RemoveLife(int i)
    {
        CurrentLives -= i;
        Debug.Log("Lose " + i + " lives, Current Lives:" + CurrentLives);
        if (CurrentLives < 0)
            CurrentLives = 0;

    }

    public static int CheckLife()
    {
        return CurrentLives;
    }

    public static void GameOver()
    {
        Debug.Log("Game over man, Game over");
    }


}
