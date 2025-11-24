using UnityEngine;

public class RayCastScan : MonoBehaviour
{
    private RaycastHit2D[] _enemys;
    [SerializeField] private float _circleRadius;
    [SerializeField] private LayerMask _enemyLayer;
    public Transform target;
    void Update()
    {
        _enemys = Physics2D.CircleCastAll(transform.position, _circleRadius, Vector2.zero, 0, _enemyLayer);
        target = SelectTarget();
    }

    Transform SelectTarget()
    {
        Transform target = null;
        float distance = 50;

        foreach(var enemy in _enemys)
        {
            Vector2 _playerPos = transform.position;
            Vector2 enemyPos = enemy.transform.position;
            float curDis = Vector2.Distance(_playerPos, enemyPos);

            if(curDis < distance)
            {
                distance = curDis;
                target = enemy.transform;
            }
        }

        return target;
    }
}
