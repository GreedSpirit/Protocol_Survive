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
}
