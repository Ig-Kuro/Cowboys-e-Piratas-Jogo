using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ToolTip : MonoBehaviour
{
    public TextMeshProUGUI header,content;

    public LayoutElement layoutElement;
    public int characterWrapLimit;
    public RectTransform rectTransform;
    private void Awake()
    {
        rectTransform=GetComponent<RectTransform>();
    }

    public void SetText(string contentTXT,string headerTXT="")
    {
        if(string.IsNullOrEmpty(headerTXT))
        {
            header.gameObject.SetActive(false);
        }
        else
        {
            header.gameObject.SetActive(true);
            header.text=headerTXT;
        }
        content.text=contentTXT;
        
        int headerLength=header.text.Length;
        int contentLenght=content.text.Length;

        layoutElement.enabled= (headerLength>characterWrapLimit || contentLenght > characterWrapLimit) ? true : false;
    }
    public void Update()
    {
        Vector2 position= Input.mousePosition;

        float pivotX= position.x /Screen.width;
        float pivotY= position.y /Screen.height;

        rectTransform.pivot=new Vector2(pivotX,pivotY);

        transform.position=position;
    }

}
