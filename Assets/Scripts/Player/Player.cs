using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _baseMoveSpeed = 5f;
    [SerializeField] private float _moveSpeed = 5f;
    private Vector2 _moveInput;
    private Rigidbody2D _playerRigid;
    private SpriteRenderer _playerSpriteRenderer;
    private Animator _playerAnimator;
    private UIController _uiController;
    private bool _isLive = true;


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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.Instance.isLive)
        {
            return;
        }

        GameManager.Instance.playerHealth -= 10;
        _uiController.HealthChanged();


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
