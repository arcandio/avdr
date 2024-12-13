using UnityEngine;

public class UiNavigation : MonoBehaviour
{
    public PageName targetPage = PageName.TrayPage;
    void Start(){
        UiPageManager.instance.CollectAllNavs();
    }

    /// <summary>
    /// Navigates to a given UI page. Useful for UI consequences to controller actions.
    /// </summary>
    /// <param name="pageName"></param>
    public void NavigateToPage(PageName pageName) {
        UiPageManager.instance.SetPage(pageName);
    }

    /// <summary>
    /// Navigates to a given UI page. Used on UnityEngine button events, which only accept
    /// bools, ints, and strings as input.
    /// </summary>
    public void NavigateToPage() {
        NavigateToPage(targetPage);
    }
}
