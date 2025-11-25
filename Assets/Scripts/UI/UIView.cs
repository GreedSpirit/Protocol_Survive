using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Slider _expSlider;
    [SerializeField] private TextMeshProUGUI _killText;
    [SerializeField] private Slider _healthSlider;

    [SerializeField] private RectTransform _healthRectTransform;
    [SerializeField] private RectTransform _levelUpRectTransform;

    private UpgradeItem[] _items;

    void Start()
    {
        _items = _levelUpRectTransform.GetComponentsInChildren<UpgradeItem>(true); //true를 함으로써 비활성화상태인 자식도 갖고옴
    }

    void LateUpdate()
    {
        _healthRectTransform.position = Camera.main.WorldToScreenPoint
        (
            GameManager.Instance.player.transform.position
        );
    }

    public void UpdateExpUI(int curExp, int maxExp) // 뷰
    {
        _expSlider.value = (float)curExp / (float)maxExp;
    }

    public void UpdateTimerUI(float curtime)
    {
        int min = Mathf.FloorToInt(curtime / 60);
        int sec = Mathf.FloorToInt(curtime % 60);
        _timeText.text = string.Format("{0:D2} : {1:D2}", min, sec);
    }

    public void UpdateKillUI(int killCount)
    {
        _killText.text = "Kill : " + killCount;
    }

    public void UpdateHealthUI(int curHealth, int maxHealth)
    {
        _healthSlider.value = (float) curHealth / (float)maxHealth;
    }
    public void UpdateLevelUpUI()
    {
        if(_levelUpRectTransform.localScale == Vector3.one)
        {
            _levelUpRectTransform.localScale = Vector3.zero;
        }
        else
        {
            foreach(var item in _items)
            {
                item.gameObject.SetActive(false);
            }

            int[] ranIdx = GetRandomIndices(_items.Length, 3);

            for(int i = 0; i < ranIdx.Length; i++)
            {
                var item = _items[ranIdx[i]];

                if (item.IsMaxLevel())
                {
                    _items[4].gameObject.SetActive(true); //이미 만렙인 장비라면 힐팩을 대신 활성화
                }
                else
                {
                    item.gameObject.SetActive(true);
                }
            }
            _levelUpRectTransform.localScale = Vector3.one;
        }
    }

    public int[] GetRandomIndices(int maxCount, int pickCount)
    {
        if(maxCount < pickCount)
        {
            Debug.LogError("Error pickCount over maxCount");
            return null;
        }
        // 중복을 허용하지 않는 HashSet을 사용
        HashSet<int> uniqueNumbers = new HashSet<int>();

        while (uniqueNumbers.Count < pickCount)
        {
            int rand = Random.Range(0, maxCount);
            uniqueNumbers.Add(rand);
        }

        return uniqueNumbers.ToArray();
    }
}
