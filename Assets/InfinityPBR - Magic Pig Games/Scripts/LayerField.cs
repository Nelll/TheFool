using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class LayerField : PropertyAttribute
{
    private int defaultValue;
    private string label;

    public LayerField(string label)
    {
        this.label = label;
    }

    public LayerField(string label, int defaultValue) : this(label)
    {
        this.defaultValue = defaultValue;
    }

    public string tooltip { get; internal set; }
    public string name { get; internal set; }


}