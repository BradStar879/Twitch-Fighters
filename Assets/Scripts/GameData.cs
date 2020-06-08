using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    private static int players = 1;
    private static Difficulty computerDifficulty;
    private static bool pressedButtonPlayerOne; //True if player one pressed the last button
    private static characters fighterOneCharacter;
    private static characters fighterTwoCharacter;
    private static bool sameSettings = false;   //True if starting fight with same settings but new fighters

    public static int GetPlayers()
    {
        return players;
    }

    public static void SetPlayers(int newPlayers)
    {
        players = newPlayers;
    }

    public static Difficulty GetComputerDifficulty()
    {
        return computerDifficulty;
    }

    public static void SetComputerDifficulty(Difficulty difficulty)
    {
        computerDifficulty = difficulty;
    }

    public static bool GetPressedButtonPlayerOne()
    {
        return pressedButtonPlayerOne;
    }

    public static void SetPressedButtonPlayerOne(bool isPlayerOne)
    {
        pressedButtonPlayerOne = isPlayerOne;
    }

    public static characters GetFighterOneCharacter()
    {
        return fighterOneCharacter;
    }

    public static void SetFighterOneCharacter(characters character)
    {
        fighterOneCharacter = character;
    }

    public static characters GetFighterTwoCharacter()
    {
        return fighterTwoCharacter;
    }

    public static void SetFighterTwoCharacter(characters character)
    {
        fighterTwoCharacter = character;
    }

    public static bool GetSameSettings()
    {
        return sameSettings;
    }

    public static void SetSameSettings(bool isSameSettings)
    {
        sameSettings = isSameSettings;
    }
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}