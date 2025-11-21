using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private bool _isLive = true;

    private Rigidbody2D _enemyRigid;
    private SpriteRenderer _enemySpriteRenderer;

    void Start()
    {
        _enemyRigid = GetComponent<Rigidbody2D>();
        _enemySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if(!_isLive) return;
        
        Vector2 dirVec = _targetTransform.position - transform.position;
        Vector2 moveVec = Time.fixedDeltaTime * _moveSpeed * dirVec.normalized;
        _enemyRigid.MovePosition(new Vector2(transform.position.x, transform.position.y) + moveVec);

        if(dirVec.x != 0)
        {
            _enemySpriteRenderer.flipX = dirVec.x < 0 ? true : false; //스프라이트 좌 우 변경
        }
    }
}
