using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponIndex; // 0 = Melee 1 = Range
    public int prefabIndex;
    public int speed;
    public int damage;
    public int count;

    void Start()
    {
        Init();
    }

    void Update()
    {
        switch (weaponIndex)
        {
            case 0: //근접 무기 작동 장소
                transform.Rotate(speed * Time.deltaTime * Vector3.forward);
                break;
            default:
                break;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Upgrade(10, 3);
        }
    }

    void Init()
    {
        switch (weaponIndex)
        {
            case 0: //근접 무기 초기화 장소
                PlaceMeleeAttack();
                break;
            default:
                break;
        }
    }

    void PlaceMeleeAttack()
    {
        for(int i = 0; i < count; i++)
        {
            speed = 100;

            Transform meleeAttack;
            if(i < transform.childCount) // pool에 이미 생성된 무기 재사용
            {
                meleeAttack = transform.GetChild(i);
            }
            else
            {
                meleeAttack = GameManager.Instance.spawner.Spawn(prefabIndex).transform;
            }
            meleeAttack.transform.parent = transform;

            meleeAttack.localPosition = Vector3.zero;
            meleeAttack.localRotation = Quaternion.identity;

            Vector3 rotateVec = ((360 * i / count) + 90) * Vector3.forward;
            meleeAttack.Rotate(rotateVec);
            meleeAttack.Translate(Vector2.right * 1, Space.Self);

            meleeAttack.GetComponent<Attack>().Init(damage, -1); // 무한 관통을 위해 -1로 설정
        }
    }

    void Upgrade(int damage, int count)
    {
        switch (weaponIndex)
        {
            case 0: //근접 무기 업그레이드 장소
                this.damage = damage;
                this.count += count;
                PlaceMeleeAttack();
                break;
            default:
                break;
        }
    }
}
