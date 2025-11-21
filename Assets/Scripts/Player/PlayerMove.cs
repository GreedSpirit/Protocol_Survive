using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    private Vector2 _moveInput;
    private Rigidbody2D _playerRigid;


    void Awake()
    {
        _playerRigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 dirVec = Time.deltaTime * _moveSpeed * _moveInput;
        _playerRigid.MovePosition(_playerRigid.position + dirVec);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }
}
