using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FighterControllerState
{
    protected FighterController fighterController;

    public void Init(FighterController fighterController)
    {
        this.fighterController = fighterController;
    }

    public static FighterControllerState DetermineFighterControllerState(FighterController fighterController)
    {
        FighterControllerState fighterControllerState = null;
        if (fighterController.IsPlayerOne() || GameData.GetPlayers() == 2)
        {
            fighterControllerState = new PlayerControllerState();
        }
        else
        {
            switch (GameData.GetComputerDifficulty())
            {
                case Difficulty.Easy:
                    fighterControllerState = new EasyAiControllerState();
                    break;
                case Difficulty.Medium:
                    fighterControllerState = new MediumAiControllerState();
                    break;
                case Difficulty.Hard:
                    fighterControllerState = new HardAiControllerState();
                    break;
            }
        }

        fighterControllerState.Init(fighterController);
        return fighterControllerState;
    }

    public abstract void FighterUpdate();
}

public class PlayerControllerState : FighterControllerState
{
    public override void FighterUpdate()
    {
        fighterController.PlayerUpdate();
    }
}

public class EasyAiControllerState : FighterControllerState
{
    public override void FighterUpdate()
    {
        fighterController.EasyAiUpdate();
    }
}

public class MediumAiControllerState : FighterControllerState
{
    public override void FighterUpdate()
    {
        fighterController.MediumAiUpdate();
    }
}

public class HardAiControllerState : FighterControllerState
{
    public override void FighterUpdate()
    {
        fighterController.HardAiUpdate();
    }
}