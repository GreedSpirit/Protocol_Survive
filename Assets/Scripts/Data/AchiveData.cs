using UnityEngine;

[System.Serializable]
public class AchiveData
{
    public int totalKillCount = 0;
    public bool isGameCleared = false;

    public bool[] isCharacterUnlocked = new bool[4];

    public AchiveData()
    {
        isCharacterUnlocked[0] = true;
        isCharacterUnlocked[1] = true;
    }
}
