using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestMenu : MonoBehaviour
{
    [Serializable]
    public class QuestRow
    {
        public string itemName;
        public int required = 1;
        public Slider bar;
        public TextMeshProUGUI label;
        public string[] aliases;
    }

    [Header("UI")]
    public GameObject questPanel;
    public TextMeshProUGUI headerText;
    public KeyCode toggleKey = KeyCode.Q;

    [Header("Objectives")]
    public QuestRow boat;
    public QuestRow paddles;

    [Header("Update")]
    public float refreshInterval = 0.25f;

    public bool AllObjectivesComplete { get; private set; }
    public event Action<bool> OnObjectivesCompleteChanged;

    float _timer;

    void Start()
    {
        if (questPanel) questPanel.SetActive(false);

        InitRow(boat);
        InitRow(paddles);

        UpdateAllRows();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            bool opening = questPanel && !questPanel.activeSelf;
            if (questPanel) questPanel.SetActive(!questPanel.activeSelf);

            if (opening)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                bool invOpen = InventorySystem.Instance != null && InventorySystem.Instance.isOpen;
                bool craftOpen = CraftingSystem.Instance != null && CraftingSystem.Instance.isOpen;
                if (!invOpen && !craftOpen)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }

        _timer += Time.deltaTime;
        if (_timer >= refreshInterval)
        {
            _timer = 0f;
            UpdateAllRows();
        }

        if (AllObjectivesComplete && Input.GetKeyDown(KeyCode.H))
        {
            BoatEndingController.PlayEnding();
        }
    }

    void InitRow(QuestRow row)
    {
        if (row == null) return;
        if (row.bar != null)
        {
            row.bar.minValue = 0f;
            row.bar.maxValue = Mathf.Max(1, row.required);
            row.bar.wholeNumbers = true;
        }
    }

    void UpdateAllRows()
    {
        int haveBoat    = CountItems(boat);
        int havePaddles = CountItems(paddles);

        SetRow(boat, haveBoat);
        SetRow(paddles, havePaddles);

        bool allDone = haveBoat >= (boat?.required ?? 1)
                    && havePaddles >= (paddles?.required ?? 2);

        if (headerText)
            headerText.text = allDone
                ? "Boat Repair — All objectives complete!"
                : "Boat Repair — Objectives";

        bool prev = AllObjectivesComplete;
        AllObjectivesComplete = allDone;
        if (AllObjectivesComplete != prev)
            OnObjectivesCompleteChanged?.Invoke(AllObjectivesComplete);
    }

    void SetRow(QuestRow row, int have)
    {
        if (row == null) return;
        have = Mathf.Clamp(have, 0, row.required);
        if (row.bar != null) row.bar.value = have;
        if (row.label != null)
            row.label.text = $"{row.itemName}: {have}/{row.required}";
    }

    int CountItems(QuestRow row)
    {
        if (row == null) return 0;
        var inv = InventorySystem.Instance;
        if (inv == null || inv.itemList == null) return 0;

        var names = new List<string> { row.itemName };
        if (row.aliases != null && row.aliases.Length > 0) names.AddRange(row.aliases);

        int count = 0;
        foreach (var n in inv.itemList)
            if (names.Contains(n)) count++;
        return count;
    }
}
