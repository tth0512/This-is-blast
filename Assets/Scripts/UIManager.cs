using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GamePalette;

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

    [SerializeField]
    private GameObject Palette;

    private int LastLevel;
    public string GetCurrentLevel => dropdown.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;

    private void Start()
    {
        LastLevel = 2;
        ChangeColorByButton();
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

    public void ChangeColorByButton()
    {
        foreach (Transform button in Palette.transform)
        {
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                var buttonColor = BlockColor.Grey;

                if (Enum.TryParse<BlockColor>(button.gameObject.name, true, out var parsed)) {
                    buttonColor = parsed;
                }

                makeLevel.currentColor = (int)buttonColor;
            });
        }
    }

    
}
