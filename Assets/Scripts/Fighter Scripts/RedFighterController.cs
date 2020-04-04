using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RedFighterController : FighterController
{
    public override void Init(bool isPlayerOne, GameObject otherFighter)
    {
        base.Init(isPlayerOne, otherFighter);
        introQuotes = new string[]{
            "Call me Freddie because I'm you're worst nightmare.",
            "Bring it on dingus. I could eat your ass all day.",
            "You're going down. I mean all the way down. Like downtown down."};
        victoryQuotes = new string[]{
            "Knew you couldn't handle this D.",
            "What an amateur.",
            "You're not even worth a rematch."};
    }
}
