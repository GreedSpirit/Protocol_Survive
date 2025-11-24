using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance //기본적인 싱글톤 구조 (해당 프로젝트에서는 단일 장면으로 이루어지기에 필수적이지 않지만 연습을 위해 구현)
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
    public int playerHealth;
    public int playerMaxHealth = 100;
    public int curLevel;
    public int curExp;
    public int killCount;
    public int[] nextExp;

    [Header("About Object Pool")]
    public ObjectPool spawner;
    
    [Header("About Game State")]
    public float maxGameTimer = 60f;
    public float curGameTimer;

    public event Action<int, int> OnExpChanged;
    public event Action<float> OnTimerChanged;
    public event Action<int> OnKillCountChanged;
    public event Action<int, int> OnHealthChanged;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {            
            _instance = this;            
            DontDestroyOnLoad(this.gameObject); 
        }
    }

    void Start()
    {
        OnExpChanged?.Invoke(curExp, nextExp[curLevel]);
        OnKillCountChanged?.Invoke(killCount);
        OnTimerChanged?.Invoke(maxGameTimer - curGameTimer);
        playerHealth = playerMaxHealth;
        OnHealthChanged?.Invoke(playerHealth, playerMaxHealth);
    }

    void Update()
    {
        curGameTimer += Time.deltaTime;
        if(curGameTimer >= maxGameTimer)
        {
            curGameTimer = maxGameTimer;
        }

        OnTimerChanged?.Invoke(maxGameTimer - curGameTimer);
    }

    public void ExpChanged() // 모델
    {
        curExp++;

        if(curExp >= nextExp[curLevel])
        {
            curExp = 0;
            curLevel++;
        }

        OnExpChanged?.Invoke(curExp, nextExp[curLevel]);
    }

    public void KillChanged()
    {
        killCount++;

        OnKillCountChanged?.Invoke(killCount);
    }

    public void HealthChanged()
    {
        OnHealthChanged?.Invoke(playerHealth, playerMaxHealth);
    }

}
