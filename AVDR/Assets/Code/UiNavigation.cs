using UnityEngine;

public class UiNavigation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private UiPageManager manager;

    public void SetupPage(UiPageManager uipm) {
        manager = uipm;
    }

    public void NavigateToPage(string pageName) {
        manager.SetPage(pageName);
    }
}
