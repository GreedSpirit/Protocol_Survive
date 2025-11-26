using UnityEngine;

public class Equipment : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;
    public void Init(ItemData data)
    {
        name = "Equipment for" + data.itemName;
        transform.parent = GameManager.Instance.player.transform;
        

        type = data.itemType;
        rate = data.damages[0];
        ApplyUpgrade();
    }

    public void Upgrade(float rate)
    {
        this.rate = rate;
        ApplyUpgrade();
    }

    void AttackSpeedIncrease()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach(var weapon in weapons)
        {
            weapon.SpeedIncrease(rate);
        }
    }

    void MoveSpeedIncrease()
    {
        GameManager.Instance.player.SetMoveSpeed(rate);
    }

    void ApplyUpgrade()
    {
        switch (type)
        {
            case ItemData.ItemType.AS:
                AttackSpeedIncrease();
                break;
            case ItemData.ItemType.MS:
                MoveSpeedIncrease();
                break;
        }
    }


}
