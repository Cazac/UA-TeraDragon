using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int MaxLives;
    public int CurrentLives;

    public void RemoveLife(int i)
    {
        CurrentLives -= i;
        Debug.Log("Lose " + i + " lives, Current Lives:" + CurrentLives);
        if (CurrentLives < 0)
        {
            CurrentLives = 0;
        }

        if(CurrentLives <= 0)
        {
            GameOver();
        }

    }

    public int CheckLife()
    {
        return CurrentLives;
    }

    public void GameOver()
    {
        GameOverScript gos = (GameOverScript) FindObjectOfType(typeof(GameOverScript));
        gos.TurnOnGameOver();
        Debug.Log("Game over man, Game over");
    }


}
