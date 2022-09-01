using UnityEngine;
using UnityEngine.UI;

public class VisualDebugger : Framework.MonoBehaviorSingleton<VisualDebugger>
{
    public Text text;
    public void SetText(string s) {
        this.text.text = s;
    }
}
