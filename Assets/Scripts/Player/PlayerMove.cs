using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    private Vector2 _moveInput;
    private Rigidbody2D _playerRigid;
    private SpriteRenderer _playerSpriteRenderer;
    private Animator _playerAnimator;


    void Awake()
    {
        _playerRigid = GetComponent<Rigidbody2D>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
        _playerAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
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
}
