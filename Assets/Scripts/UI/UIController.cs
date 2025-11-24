using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] UIView uiView;
    void Start() //awake로 바꿔야하나?
    {
        GameManager.Instance.OnExpChanged += uiView.UpdateExpUI; //뷰와 모델 연결
        GameManager.Instance.OnKillCountChanged += uiView.UpdateKillUI;
        GameManager.Instance.OnTimerChanged += uiView.UpdateTimerUI;
        GameManager.Instance.OnHealthChanged += uiView.UpdateHealthUI;
    }

    public void ExpChanged()
    {
        GameManager.Instance.ExpChanged(); //컨트롤러;
    }

    public void KillCountChanged()
    {
        GameManager.Instance.KillChanged();
    }

    public void HealthChanged()
    {
        GameManager.Instance.HealthChanged();
    }

    
}
