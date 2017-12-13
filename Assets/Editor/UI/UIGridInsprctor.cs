using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(UIGrid))]
public class UIGridInsprctor : Editor
{
    UIGrid _grid = null;
    UIGrid grid
    {
        get
        {
            if (_grid == null)
                _grid = target as UIGrid;
            return _grid;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        grid.SetElementCount(EditorGUILayout.IntField("元素数量", grid.GetElementCount()));
        float w = EditorGUILayout.FloatField("单个元素宽度", grid.GetElementWidth());
        float h = EditorGUILayout.FloatField("单个元素高度", grid.GetElementHeight());
        grid.SetElementSize(w, h);
        if (grid.IsVertical())
        {
            if (GUILayout.Button("Vertical"))
            {
                grid.SetIsVertical(false);
            }
        }
        else
        {
            if (GUILayout.Button("Horizontal"))
            {
                grid.SetIsVertical(true);
            }
        }
        int _c = EditorGUILayout.IntField("限制数量", grid.GetConstraintCount());
        grid.SetConstraintCount(_c);

        EditorGUILayout.LabelField("当前列数", grid.GetColumnCount().ToString());
        EditorGUILayout.LabelField("当前行数", grid.GetRowCount().ToString());
        EditorGUILayout.LabelField("起始Index", grid.GetIndexStart().ToString());
        EditorGUILayout.LabelField("结束Index", grid.GetIndexEnd().ToString());

        EditorGUILayout.LabelField("显示数量", grid.GetShowCount().ToString());

        if (GUILayout.Button("apply"))
        {
            grid.ApplySetting();
        }
        if (GUILayout.Button("clear"))
        {
            grid.ClearSetting();
        }
    }
}