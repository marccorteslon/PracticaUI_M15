using System.Collections;
using UnityEngine;

public class SideMenuFillerItem : MonoBehaviour
{
    [SerializeField] private SideMenuController menuController;

    public void DeleteItem()
    {
        if (menuController == null)
        {
            Destroy(gameObject);
            return;
        }

        menuController.StartCoroutine(DeleteAndRefresh());
    }

    private IEnumerator DeleteAndRefresh()
    {
        Destroy(gameObject);
        yield return null;
        menuController.UpdateGrid();
    }
}