using System.Threading;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponIndex; // 0 = Melee 1 = Range
    public int prefabIndex; // Spawner의 인스펙터에 할당된 프리팹 인덱스
    public float speed; // 근접은 회전 속도, 원거리는 연사 속도
    public float damage;
    public int count; // 근접은 실제 구현 갯수, 원거리는 관통 수

    private float _curTimer;
    private Player _player;

    void Awake()
    {
        _player = GameManager.Instance.player;
    }

    void Start()
    {
        //Init();
    }

    void Update()
    {
        switch (weaponIndex)
        {
            case 0: //근접 무기 작동 장소
                transform.Rotate(speed * Time.deltaTime * Vector3.forward);
                break;
            case 1:
                _curTimer += Time.deltaTime;

                if(_curTimer > speed)
                {
                    _curTimer = 0;
                    Fire();
                }
                break;
            default:
                break;
        }

        if (Input.GetButtonDown("Jump"))
        {
            Upgrade(10, 3);
        }
    }

    public void Init(ItemData itemData)
    {
        transform.parent = _player.transform;
        transform.localPosition = Vector3.zero;
        name = itemData.itemName;

        weaponIndex = itemData.id;
        damage = itemData.baseDamage;
        count = itemData.baseCount;
        prefabIndex = weaponIndex + 1;

        


        switch (weaponIndex)
        {
            case 0: //근접 무기 초기화 장소
                PlaceMeleeAttack();
                break;
            case 1:
                speed = 0.3f;
                break;
            default:
                break;
        }

        _player.BroadcastMessage("ApplyUpgrade", SendMessageOptions.DontRequireReceiver);
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

            meleeAttack.GetComponent<Attack>().Init(damage, -1, Vector3.zero); // 무한 관통을 위해 -1로 설정
        }
    }

    public void Upgrade(float damage, int count)
    {
        this.damage = damage;
        this.count += count;
        switch (weaponIndex)
        {
            case 0: //근접 무기 재배치
                PlaceMeleeAttack();
                break;
            default:
                break;
        }

        _player.BroadcastMessage("ApplyUpgrade", SendMessageOptions.DontRequireReceiver);
    }

    void Fire()
    {
        if(_player.GetComponent<RayCastScan>().target == null) 
            return;
        Vector3 targetPostion = _player.GetComponent<RayCastScan>().target.position;
        Vector3 dir = targetPostion - _player.transform.position;
        dir = dir.normalized;

        Transform rangeAttack = GameManager.Instance.spawner.Spawn(prefabIndex).transform;
        rangeAttack.position = transform.position;
        rangeAttack.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        rangeAttack.GetComponent<Attack>().Init(damage, count, dir);
    }
}
