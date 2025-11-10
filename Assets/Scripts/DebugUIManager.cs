using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

//COPIED FROM CHATGPT
public class DebugCanvas : MonoBehaviour
{
    [System.Serializable]
    public class DebugVariable
    {
        public string label;             // What name to display
        public MonoBehaviour target;     // Which script the variable belongs to
        public string variableName;      // Exact variable name (case-sensitive)
    }

    public List<DebugVariable> debugVariables = new List<DebugVariable>();
    public TextMeshProUGUI debugTextTemplate;
    private List<TextMeshProUGUI> activeTexts = new List<TextMeshProUGUI>();

    private void Start()
    {
        // Create a text line for each debug variable
        foreach (var variable in debugVariables)
        {
            var newText = Instantiate(debugTextTemplate, debugTextTemplate.transform.parent);
            newText.gameObject.SetActive(true);
            activeTexts.Add(newText);
        }

        debugTextTemplate.gameObject.SetActive(false); // Hide template
    }

    private void Update()
    {
        for (int i = 0; i < debugVariables.Count; i++)
        {
            var dv = debugVariables[i];
            if (dv.target == null) continue;

            FieldInfo field = dv.target.GetType().GetField(dv.variableName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            PropertyInfo prop = dv.target.GetType().GetProperty(dv.variableName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            object value = null;
            if (field != null) value = field.GetValue(dv.target);
            else if (prop != null && prop.CanRead) value = prop.GetValue(dv.target);

            activeTexts[i].text = $"{dv.label}: {(value != null ? value.ToString() : "N/A")}";
        }
    }
}
