using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs;
    private List<GameObject>[] _pools;

    void Awake()
    {
        _pools = new List<GameObject>[_prefabs.Length];
        for(int i = 0; i < _prefabs.Length; i++)
        {
            _pools[i] = new List<GameObject>();
        }
    }

    public GameObject Spawn(int index)
    {
        GameObject result = null;

        foreach(GameObject p in _pools[index])
        {
            if (!p.activeSelf)
            {
                result = p;
                p.SetActive(true);
                break;
            }
        }
        if(result == null)
        {
            result = Instantiate(_prefabs[index], transform);
            _pools[index].Add(result);
        }

        return result;
    }
}
