using UnityEngine;

public class UiNavigation : MonoBehaviour
{
    void Start(){
        UiPageManager.instance.CollectAllNavs();
    }

    public void NavigateToPage(string pageName) {
        UiPageManager.instance.SetPage(pageName);
    }
}
