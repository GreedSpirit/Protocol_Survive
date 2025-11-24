using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Status")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private RuntimeAnimatorController[] _enemyRunTimeAnimator;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private bool _isLive;

    [Header("Controller")]
    [SerializeField] private UIController _uiController;
    
    private Rigidbody2D _enemyRigid;
    private SpriteRenderer _enemySpriteRenderer;
    private Animator _enemyAnimator;
    private Collider2D _enemyCollider;
    private WaitForSeconds _waitTime;

    void Awake()
    {
        _enemyRigid = GetComponent<Rigidbody2D>();
        _enemySpriteRenderer = GetComponent<SpriteRenderer>();
        _enemyAnimator = GetComponent<Animator>();
        _enemyCollider = GetComponent<Collider2D>();
        _waitTime = new WaitForSeconds(1.2f);
        _uiController = FindFirstObjectByType<UIController>();
    }

    void FixedUpdate()
    {
        if(!_isLive || _enemyAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Hit")) 
            return;

        Vector2 dirVec = _targetTransform.position - transform.position;
        Vector2 moveVec = Time.fixedDeltaTime * _moveSpeed * dirVec.normalized;
        _enemyRigid.MovePosition(new Vector2(transform.position.x, transform.position.y) + moveVec);
        _enemyRigid.linearVelocity = Vector2.zero;

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

        _enemyRigid.simulated = true;
        _enemyCollider.enabled = true;
        _enemyAnimator.SetBool("Death", !_isLive);
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
        if (!_isLive)
        {
            return;
        }
        if (collision.CompareTag("Attack"))
        {
            _health -= collision.GetComponent<Attack>()._damage;

            if(_health > 0) // Live
            {
                StartCoroutine(KnockBack());
                _enemyAnimator.SetTrigger("Hit");
                
            }
            else // Die
            {
                _isLive = false;
                _enemyRigid.simulated = false;
                _enemyCollider.enabled = false;

                _enemyAnimator.SetBool("Death", !_isLive);
                _uiController.ExpChanged();
                _uiController.KillCountChanged();
                Invoke("Death", 2f);
            }
        }
    }
    
    IEnumerator KnockBack()
    {
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        dirVec = dirVec.normalized;
        _enemyRigid.AddForce(dirVec * 2, ForceMode2D.Impulse);
        yield return _waitTime;
    }

    void Death()
    {
        gameObject.SetActive(false);
    }
}
