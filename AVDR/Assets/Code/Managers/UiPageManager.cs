using System.Collections.Generic;
using UnityEngine;

public class UiPageManager : MonoBehaviour
{
    public static UiPageManager instance;
    /// <summary>
    /// The existing pages in the scene.
    /// </summary>
    [SerializeField] private UiPage[] pages;

    /// <summary>
    /// The existing navigation elements in the scene
    /// </summary>
    [SerializeField] private UiNavigation[] navs;
    
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
        if(instance == null) {
            instance = this;
        }
        else {
            Debug.LogError("Destroying duplicate UiPageManager");
            Destroy(gameObject);
        }
    }
    
    void Start() {
        SetPage("tray");
    }

    public void CollectAllNavs() {
        pages = FindObjectsByType<UiPage>(FindObjectsSortMode.None);
        navs = FindObjectsByType<UiNavigation>(FindObjectsSortMode.None);
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
