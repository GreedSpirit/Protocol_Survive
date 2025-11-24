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

    public Player player;
    public ObjectPool spawner;
    public float maxGameTimer = 60f;
    public float curGameTimer;
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

    void Update()
    {
        curGameTimer += Time.deltaTime;
        if(curGameTimer >= maxGameTimer)
        {
            curGameTimer = maxGameTimer;
        }
    }

}
