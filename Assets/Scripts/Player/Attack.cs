using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] public int _damage;
    [SerializeField] private int _penetration; //penetration = 관통

    public void Init(int damage, int penetration)
    {
        _damage = damage;
        _penetration = penetration;
    }
}
