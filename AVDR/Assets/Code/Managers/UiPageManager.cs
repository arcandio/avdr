using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Manages what page the app is on and navigation
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class UiPageManager : MonoBehaviour
{
    public static UiPageManager instance;
    public CharacterManager characterManager;

    public AudioSource audioSource;

    public UiPage trayPage;
    public UiPage menuPage;
    public UiPage characterListPage;
    public UiPage characterEditPage;
    public UiPage presetListPage;
    public UiPage presetEditPage;
    public UiPage historyPage;
    public UiPage settingsPage;



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

    public void Setup() {
        if(instance == null) {
            instance = this;
        }
        else {
            Debug.LogError("Destroying duplicate UiPageManager");
            Destroy(gameObject);
        }
        SetPage(PageName.TrayPage, false);
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
    UiPage FindPage(PageName pageName) {
        UiPage foundPage = null;
        switch(pageName) {
            case PageName.TrayPage:
                foundPage = trayPage;
                break;
            case PageName.MenuPage:
                foundPage = menuPage;
                break;
            case PageName.CharacterListPage:
                foundPage = characterListPage;
                break;
            case PageName.CharacterEditPage:
                foundPage = characterEditPage;
                break;
            case PageName.PresetListPage:
                foundPage = presetListPage;
                break;
            case PageName.PresetEditPage:
                foundPage = presetEditPage;
                break;
            case PageName.HistoryPage:
                foundPage = historyPage;
                break;
            case PageName.SettingsPage:
                foundPage = settingsPage;
                break;
            default:
                Debug.LogError("Did not find page " + pageName);
                break;
        }
        return foundPage;
    }

    /// <summary>
    /// Sets the current page by moving the others out of the way
    /// and moving the correct one into the view.
    /// </summary>
    /// <param name="pageName">string name of the target page.</param>
    public void SetPage(PageName pageName, bool playSound = true) {
        if(pageName != PageName.TrayPage) {
            thrower.enabled = false;
        }
        else {
            thrower.enabled = true;
        }
        if(pageName == PageName.CharacterEditPage) {
            characterManager.PopulateCharacterInputs(true);
        }
        UiPage target = FindPage(pageName);
        foreach(UiPage page in pages) {
            page.transform.position = holdingCellTransform.position;
        }
        target.transform.position = screenArea.position;
        if(playSound) audioSource.Play();
    }
}