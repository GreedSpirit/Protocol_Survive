using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public float moveSpeed;
    public int health;
    public float spawnTime;
    public int spriteIndex;
    public int damage;
}

public class SpawnPoints : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private EnemyData[] _enemyDatas;

    private float _curTimer;
    private int _spawnLevel;
    private float _divideLevel;


    void Start()
    {
        _spawnPoints = GetComponentsInChildren<Transform>();
        _divideLevel = GameManager.Instance.maxGameTimer / _enemyDatas.Length;
    }

    void Update()
    {
        if (!GameManager.Instance.isLive)
        {
            return;
        }
        _curTimer += Time.deltaTime;
        _spawnLevel = Mathf.FloorToInt(GameManager.Instance.curGameTimer / _divideLevel);
        if(_spawnLevel >= _enemyDatas.Length - 1) _spawnLevel = _enemyDatas.Length - 1;
        if(_curTimer >= _enemyDatas[_spawnLevel].spawnTime)
        {
            Spawn();
            _curTimer = 0;
        }
    }

    private void Spawn()
    {
        GameObject enemy = GameManager.Instance.spawner.Spawn(0);
        enemy.GetComponent<Enemy>().Init(_enemyDatas[_spawnLevel]);
        
        // Range를 0부터 잡을 경우 부모인 SpawnPoints도 포함되므로 1로 적어야한다.
        enemy.transform.position = _spawnPoints[Random.Range(1, _spawnPoints.Length)].transform.position;
    }
}
