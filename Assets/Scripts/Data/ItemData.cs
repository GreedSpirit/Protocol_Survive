using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData", order = 0)]
public class ItemData : ScriptableObject 
{
    public enum ItemType {Melee, Range, AS, MS, Heal}

    [Header("Base Infomation")]
    public ItemType itemType;
    public int id;
    public string itemName;
    public string desc;

    [Header("Upgrade Data")]
    public float baseDamage;
    public float[] damages;
    public int baseCount;
    public int[] counts;

    [Header("Attack")]
    public GameObject attackPrefab;
}
