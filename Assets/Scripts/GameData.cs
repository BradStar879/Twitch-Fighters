using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    private static int players;
    private static MainMenuScript.characters fighterOneCharacter;
    private static MainMenuScript.characters fighterTwoCharacter;

    public static int GetPlayers()
    {
        return players;
    }

    public static void SetPlayers(int newPlayers)
    {
        players = newPlayers;
    }

    public static MainMenuScript.characters GetFighterOneCharacter()
    {
        return fighterOneCharacter;
    }

    public static void SetFighterOneCharacter(MainMenuScript.characters character)
    {
        fighterOneCharacter = character;
    }

    public static MainMenuScript.characters GetFighterTwoCharacter()
    {
        return fighterTwoCharacter;
    }

    public static void SetFighterTwoCharacter(MainMenuScript.characters character)
    {
        fighterTwoCharacter = character;
    }
}