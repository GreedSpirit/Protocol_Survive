using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance //싱글톤 구조
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameManager>();
            }
            return _instance;
        }
    }
    [Header("About Player")]
    public Player player;
    public float playerHealth;
    public float playerMaxHealth = 100f;
    public int curLevel;
    public int curExp;
    public int killCount;
    public int[] nextExp;
    public int characterSelectIndex;

    [Header("About Object Pool")]
    public ObjectPool spawner;
    public GameObject clear;
    
    [Header("About Game State")]
    public float maxGameTimer = 60f;
    public float curGameTimer = 0;
    public bool isLive;
    [Header("UI")]
    [SerializeField] private RectTransform _levelUpRectTransform;
    [SerializeField] private GameObject _inGameUI;

    public event Action<int, int> OnExpChanged;
    public event Action<float> OnTimerChanged;
    public event Action<int> OnKillCountChanged;
    public event Action<float, float> OnHealthChanged;
    public event Action OnLevelChanged;
    public event Action OnGameOverChanged;
    private UpgradeItem[] _items;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {            
            _instance = this;            
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        _items = _levelUpRectTransform.GetComponentsInChildren<UpgradeItem>(true); //true를 함으로써 비활성화상태인 자식도 갖고옴
    }

    void BaseWeapon(int idx)
    {
        _items[idx].OnClick();
    }

    public void GameStart(int idx)
    {
        _inGameUI.SetActive(true);
        characterSelectIndex = idx;
        playerHealth = playerMaxHealth;
        player.gameObject.SetActive(true);
        BaseWeapon(idx % 2); // 현재 무기가 2종류 이기 때문에 2

        OnExpChanged?.Invoke(curExp, nextExp[curLevel]);
        OnKillCountChanged?.Invoke(killCount);
        OnTimerChanged?.Invoke(maxGameTimer - curGameTimer);
        OnHealthChanged?.Invoke(playerHealth, playerMaxHealth);

        TimeResume();
    }

    void Init()
    {
        playerHealth = playerMaxHealth;
        curExp = 0;
        killCount = 0;
        curGameTimer = 0;
        curLevel = 0;

        OnExpChanged?.Invoke(curExp, nextExp[curLevel]);
        OnKillCountChanged?.Invoke(killCount);
        OnTimerChanged?.Invoke(maxGameTimer - curGameTimer);
        playerHealth = playerMaxHealth;
        OnHealthChanged?.Invoke(playerHealth, playerMaxHealth);
    }

    public void GameReStart()
    {
        Init();
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        isLive = false;
        yield return new WaitForSeconds(1.2f);
        OnGameOverChanged?.Invoke();
        TimeStop();
    }

    public void GameVictory()
    {
        SaveManager.Instance.achiveData.isGameCleared = true;

        if (SaveManager.Instance.achiveData.isCharacterUnlocked[3] == false)
        {
            SaveManager.Instance.achiveData.isCharacterUnlocked[3] = true;
            Debug.Log("4char achive");

            SaveManager.Instance.SaveGame();
        }
        
        StartCoroutine(GameVictoryCoroutine());
    }

    IEnumerator GameVictoryCoroutine()
    {
        isLive = false;
        clear.SetActive(true);

        yield return new WaitForSeconds(0.7f);

        OnGameOverChanged?.Invoke();
        TimeStop();
    }

    void Update()
    {
        if(!isLive)
            return;

        curGameTimer += Time.deltaTime;
        if(curGameTimer >= maxGameTimer)
        {
            curGameTimer = maxGameTimer;
            GameVictory();
        }

        OnTimerChanged?.Invoke(maxGameTimer - curGameTimer);
    }

    public void ExpChanged() // 모델
    {
        if(!isLive)
            return;

        curExp++;

        if(curExp >= nextExp[curLevel])
        {
            curExp = 0;
            curLevel++;
            OnLevelChanged?.Invoke();
            TimeStop();
            if(curLevel >= nextExp.Length) 
                curLevel = nextExp.Length - 1;
        }

        OnExpChanged?.Invoke(curExp, nextExp[curLevel]);
    }

    public void KillChanged()
    {
        killCount++;
        SaveManager.Instance.achiveData.totalKillCount++;

        if(SaveManager.Instance.achiveData.totalKillCount >= 10)
        {
            if(SaveManager.Instance.achiveData.isCharacterUnlocked[2] == false)
            {
                SaveManager.Instance.achiveData.isCharacterUnlocked[2] = true;
                Debug.Log("3char achive");

                SaveManager.Instance.SaveGame();
            }
        }

        OnKillCountChanged?.Invoke(killCount);
    }

    public void HealthChanged()
    {
        OnHealthChanged?.Invoke(playerHealth, playerMaxHealth);
    }

    public void LevelChanged()
    {
        OnLevelChanged?.Invoke();
    }

    public void TimeStop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void TimeResume()
    {
        isLive = true;
        Time.timeScale = 1;
    }

    public void OnClickExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
