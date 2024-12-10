using UnityEngine;

public class UiPageManager : MonoBehaviour
{
    /// <summary>
    /// The existing pages in the scene.
    /// </summary>
    public UiPage[] pages;

    /// <summary>
    /// The existing navigation elements in the scene
    /// </summary>
    public UiNavigation[] navs;
    
    public Thrower thrower;

    /// <summary>
    /// A position to store the other pages at.
    /// </summary>
    public RectTransform holdingCellTransform;

    /// <summary>
    /// A position with which to show the selected page.
    /// </summary>
    public RectTransform screenArea;

    void Awake() {
        /* gather the pages when the app starts. */
        pages = FindObjectsByType<UiPage>(FindObjectsSortMode.None);
        navs = FindObjectsByType<UiNavigation>(FindObjectsSortMode.None);
        foreach(UiNavigation nav in navs) {
            nav.SetupPage(this);
        }
    }
    
    void Start() {
        SetPage("tray");
    }
    
    /// <summary>
    /// Finds a page in the list of pages by name.
    /// </summary>
    /// <param name="pageName">Page name as a string</param>
    /// <returns>string name of the target page.</returns>
    UiPage FindPage(string pageName) {
        UiPage foundPage = null;
        foreach(UiPage page in pages) {
            if(page.gameObject.name == pageName) {
                foundPage = page;
            }
        }
        if(foundPage == null) {
            Debug.LogError("Did not find page: " + pageName);
            return null;
        }
        return foundPage;
    }

    /// <summary>
    /// Sets the current page by moving the others out of the way
    /// and moving the correct one into the view.
    /// </summary>
    /// <param name="pageName">string name of the target page.</param>
    public void SetPage(string pageName) {
        if(pageName != "tray") {
            thrower.enabled = false;
        }
        else {
            thrower.enabled = true;
        }
        UiPage target = FindPage(pageName);
        foreach(UiPage page in pages) {
            page.transform.position = holdingCellTransform.position;
        }
        target.transform.position = screenArea.position;
    }
}
