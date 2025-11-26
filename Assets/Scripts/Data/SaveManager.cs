using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public AchieveData achieveData;
    private string _path;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        _path = Path.Combine(Application.persistentDataPath, "AchieveData.json");
        
        LoadGame();
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(achieveData, true);
        File.WriteAllText(_path, json);
    }

    public void LoadGame()
    {
        if (!File.Exists(_path))
        {
            achieveData = new AchieveData();
            SaveGame();
            return;
        }

        string json = File.ReadAllText(_path);

        achieveData = JsonUtility.FromJson<AchieveData>(json);
    }

    public void ResetGame() //테스트용 초기화 함수
    {
        achieveData = new AchieveData();
        SaveGame();
        SceneManager.LoadScene(0);
    }
}
