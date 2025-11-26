using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public AchiveData achiveData;
    private string _path;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);

        _path = Path.Combine(Application.persistentDataPath, "AchiveData.json");
        
        LoadGame();
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(achiveData, true);

        File.WriteAllText(_path, json);

        Debug.Log("save");

    }

    public void LoadGame()
    {
        if (!File.Exists(_path))
        {
            achiveData = new AchiveData();
            SaveGame();
            return;
        }

        string json = File.ReadAllText(_path);

        achiveData = JsonUtility.FromJson<AchiveData>(json);
    }

    public void ResetGame() //테스트용 초기화 함수
    {
        achiveData = new AchiveData();
        SaveGame();
    }
}
