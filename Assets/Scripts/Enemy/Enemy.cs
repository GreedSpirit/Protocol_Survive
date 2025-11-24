using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private RuntimeAnimatorController[] _enemyRunTimeAnimator;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private bool _isLive;
    
    private Rigidbody2D _enemyRigid;
    private SpriteRenderer _enemySpriteRenderer;
    private Animator _enemyAnimator;

    void Awake()
    {
        _enemyRigid = GetComponent<Rigidbody2D>();
        _enemySpriteRenderer = GetComponent<SpriteRenderer>();
        _enemyAnimator = GetComponent<Animator>();
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

    void OnEnable()
    {
        _targetTransform = GameManager.Instance.player.transform;
        _isLive = true;
        _health = _maxHealth;
    }

    public void Init(EnemyData data)
    {
        _enemyAnimator.runtimeAnimatorController = _enemyRunTimeAnimator[data.spriteIndex];        
        _moveSpeed = data.moveSpeed;
        _maxHealth = data.health;
        _health = _maxHealth;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Attack"))
        {
            _health -= collision.GetComponent<Attack>()._damage;

            if(_health > 0) // Live
            {
                
            }
            else // Die
            {
                gameObject.SetActive(false);
            }
        }
    }
}
