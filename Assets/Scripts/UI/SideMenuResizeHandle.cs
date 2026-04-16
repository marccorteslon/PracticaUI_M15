using UnityEngine;
using UnityEngine.EventSystems;

public class SideMenuResizeHandle : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private SideMenuResizable menu;
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private Canvas canvas;

    private Camera uiCamera;

    private void Awake()
    {
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            uiCamera = canvas.worldCamera;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (menu == null)
            return;

        if (!menu.EstaAbierto)
            menu.AbrirMenu();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (menu == null || panelRect == null)
            return;

        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                panelRect,
                eventData.position,
                uiCamera,
                out localPoint))
            return;

        float nuevoAncho = localPoint.x;
        menu.SetWidthFromDrag(nuevoAncho);
    }
}