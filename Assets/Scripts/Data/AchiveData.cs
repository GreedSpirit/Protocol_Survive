using UnityEngine;

[System.Serializable]
public class AchieveData
{
    public int totalKillCount = 0;
    public bool isGameCleared = false;
    public bool[] isCharacterUnlocked = new bool[4];

    public AchieveData()
    {
        isCharacterUnlocked[0] = true;
        isCharacterUnlocked[1] = true;
    }
}
