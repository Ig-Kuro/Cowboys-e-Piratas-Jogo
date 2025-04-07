using UnityEditor;
using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    private static ToolTipSystem current;

    public ToolTip tt;
    void Awake()
    {
        current=this;
    }

    public static void Show(string contentTXT,string headerTXT)
    {
        current.tt.SetText(contentTXT,headerTXT);
        current.tt.gameObject.SetActive(true);
    }
    public static void Hide()
    {
        current.tt.gameObject.SetActive(false);
    }

}
