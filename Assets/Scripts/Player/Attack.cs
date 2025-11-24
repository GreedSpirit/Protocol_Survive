using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] public float _damage;
    [SerializeField] private int _penetration; //penetration = 관통

    private Rigidbody2D _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>(); // 근접공격은 갖고 있지 않음
    }
    public void Init(float damage, int penetration, Vector3 dir)
    {
        _damage = damage;
        _penetration = penetration;

        if(penetration > -1) // 원거리 공격이면
        {
            _rigidbody.linearVelocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if(_penetration > -1)
            {
                _penetration--;

                if(_penetration <= -1)
                {
                    _rigidbody.linearVelocity = Vector2.zero;
                    gameObject.SetActive(false);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DetectGround"))
        {
            _rigidbody.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
