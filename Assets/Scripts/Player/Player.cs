using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _baseMoveSpeed = 5f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _damageCooldown = 0.5f;
    [SerializeField] private RuntimeAnimatorController[] _playerRuntimeAnimControllers;
    private Vector2 _moveInput;
    private Rigidbody2D _playerRigid;
    private SpriteRenderer _playerSpriteRenderer;
    private Animator _playerAnimator;
    private UIController _uiController;
    private bool _isLive = true;
    private float _lastDamageTime = 0f;


    void Awake()
    {
        _playerRigid = GetComponent<Rigidbody2D>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _playerAnimator = GetComponent<Animator>();
        _moveSpeed = _baseMoveSpeed;
        _uiController = FindFirstObjectByType<UIController>();
    }

    void Start()
    {
        GameManager.Instance.player = this;
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
        {
            return;
        }
        Vector2 dirVec = Time.deltaTime * _moveSpeed * _moveInput;
        _playerRigid.MovePosition(new Vector2(transform.position.x, transform.position.y) + dirVec);

        if(dirVec.x != 0)
        {
            _playerSpriteRenderer.flipX = dirVec.x < 0 ? true : false; //플레이어 스프라이트 좌 우 변경
        }
        _playerAnimator.SetFloat("Speed", dirVec.magnitude);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }

    public void SetMoveSpeed(float rate)
    {
        _moveSpeed = _baseMoveSpeed + _baseMoveSpeed * rate;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.Instance.isLive)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(Time.time >= _lastDamageTime + _damageCooldown)
            {
                GameManager.Instance.playerHealth -= collision.gameObject.GetComponent<Enemy>().damage;
                _uiController.HealthChanged();
                StartCoroutine(DamageEffect());

                _lastDamageTime = Time.time;

                if(GameManager.Instance.playerHealth < 0 && _isLive)
                {
                    for(int idx = 2; idx < transform.childCount; idx++)
                    {
                        transform.GetChild(idx).gameObject.SetActive(false);
                    }

                    _playerAnimator.SetTrigger("Death");
                    GameManager.Instance.GameOver();
                }
            }
        }

    }

    void OnEnable()
    {
        _playerAnimator.runtimeAnimatorController = _playerRuntimeAnimControllers[GameManager.Instance.characterSelectIndex];
    }

    IEnumerator DamageEffect()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
