using System.Collections.Generic;

public class PlayerSettings
{
    public int gold;
    public int completedDays;

    public int unlockedShapes;
    public int unlockedMouth;
    public int unlockedEyes;

    public PlayerSettings()
    {
        gold = 0;
        completedDays = 0;
        unlockedEyes = 0;
        unlockedMouth = 0;
        unlockedShapes = 0;
    }
}
