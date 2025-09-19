using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField]
    private TMP_Dropdown dropdown;

    [SerializeField]
    private MakeLevel makeLevel;

    private int LastLevel;
    public string GetCurrentLevel => dropdown.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;

    private void Start()
    {
        LastLevel = 2;
    }
    public void ChangeLevel()
    {
        if (GetCurrentLevel == "Add Level")
        {
            dropdown.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"Level {++LastLevel}";
            dropdown.options[dropdown.options.Count - 1].text = $"Level {LastLevel}";
            dropdown.options.Add(new TMP_Dropdown.OptionData($"Add Level"));
        }
        makeLevel.currentLevel = GetCurrentLevel;
        makeLevel.ImportCurrentLevel();
    }


}
