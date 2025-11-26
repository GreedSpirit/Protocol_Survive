using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    public GameObject[] charButtons;
    public GameObject[] lockCharButtons;

    void Start()
    {
        for(int i = 0; i < charButtons.Length; i++)
        {
            if (SaveManager.Instance.achieveData.isCharacterUnlocked[i])
            {
                charButtons[i].SetActive(true);
            }
            else
            {
                charButtons[i].GetComponent<Button>().interactable = false;
            }
        }
    }
}
