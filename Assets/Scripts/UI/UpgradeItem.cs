using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    [SerializeField] private ItemData _data;
    [SerializeField] private int _itemLevel;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private Equipment _equipment;




    void Awake()
    {
        
    }

    public void OnClick()
    {
        switch (_data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if(_itemLevel == 0)
                {
                    GameObject weapon = new GameObject();
                    _weapon = weapon.AddComponent<Weapon>();
                    _weapon.Init(_data);
                }
                else
                {
                    _weapon.Upgrade(_data.baseDamage + (_data.baseDamage * _data.damages[_itemLevel]),
                                    _data.counts[_itemLevel]
                    );
                }
                break;
            case ItemData.ItemType.AS:
            case ItemData.ItemType.MS:
                if(_itemLevel == 0)
                {
                    GameObject equipment = new GameObject();
                    _equipment = equipment.AddComponent<Equipment>();
                    _equipment.Init(_data);
                }
                else
                {
                    float nextRate = _data.damages[_itemLevel];
                    _equipment.Upgrade(nextRate);
                }
                break;
            case ItemData.ItemType.Heal:
                GameManager.Instance.playerHealth += 10;
                if(GameManager.Instance.playerHealth >= GameManager.Instance.playerMaxHealth)
                {
                    GameManager.Instance.playerHealth = GameManager.Instance.playerMaxHealth;
                }
                break;
            default:
                break;
        }

        _itemLevel++;
        if(_itemLevel >= _data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
