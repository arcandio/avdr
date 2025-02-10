using UnityEngine;

public class OpenLink : MonoBehaviour
{
    public void ClickedLink(string url) {
        Application.OpenURL(url);
    }
}
