using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;

    [SerializeField] private float _spawnTimer = 1f;
    private float _curTimer;

    void Start()
    {
        _spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        _curTimer += Time.deltaTime;

        if(_curTimer >= _spawnTimer)
        {
            Spawn();
            _curTimer = 0;
        }
    }

    private void Spawn()
    {
        GameObject enemy = GameManager.Instance.spawner.Spawn(Random.Range(0, 4));
        // Range를 0부터 잡을 경우 부모인 SpawnPoints도 포함되므로 1로 적어야한다.
        enemy.transform.position = _spawnPoints[Random.Range(1, _spawnPoints.Length)].transform.position;
    }
}
