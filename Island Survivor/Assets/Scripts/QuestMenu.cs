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
        [Tooltip("Optional alternate names that should count as this item (exact matches).")]
        public string[] aliases;
    }

    [Header("UI")]
    public GameObject questPanel;
    public TextMeshProUGUI headerText;
    public KeyCode toggleKey = KeyCode.Q;

    [Header("Objectives")]
    public QuestRow rocks;    // 20 Rocks
    public QuestRow bones;    // 5 Bones
    public QuestRow secret;   // 1 Secret Item
    public QuestRow wood;     // 40 Wood

    [Header("Update")]
    public float refreshInterval = 0.25f;

    float _timer;

    void Start()
    {
        if (questPanel) questPanel.SetActive(false);

        InitRow(rocks);
        InitRow(bones);
        InitRow(secret);
        InitRow(wood);

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
                // Only relock if other menus aren’t open
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
        int r = CountItems(rocks);
        int b = CountItems(bones);
        int s = CountItems(secret);
        int w = CountItems(wood);

        SetRow(rocks, r);
        SetRow(bones, b);
        SetRow(secret, s);
        SetRow(wood,  w);

        bool allDone =
            r >= rocks.required &&
            b >= bones.required &&
            s >= secret.required &&
            w >= wood.required;

        if (headerText)
            headerText.text = allDone ? "Boat Repair — All objectives complete!" : "Boat Repair — Objectives";
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

        // Exact-match names used in your project:
        // Rock => "Rock (Psst Right Click!)"
        // Wood => "Wood (Psst Right Click!)"
        // Bone => "Bone"
        // Secret => e.g., "Secret Item"
        var names = new List<string> { row.itemName };
        if (row.aliases != null && row.aliases.Length > 0) names.AddRange(row.aliases);

        int count = 0;
        foreach (var n in inv.itemList)
        {
            if (names.Contains(n)) count++;
        }
        return count;
    }
}
