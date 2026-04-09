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
        if (menu != null && !menu.EstaAbierto)
            menu.AbrirMenu();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (menu == null || panelRect == null)
            return;

        Vector2 localPoint;

        // Convertimos la posición del ratón/puntero al espacio local del panel
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            panelRect.parent as RectTransform,
            eventData.position,
            uiCamera,
            out localPoint))
        {
            // Como el panel está anclado a la izquierda, la X local sirve como ancho deseado
            float nuevoAncho = Mathf.Max(0f, localPoint.x);
            menu.SetWidthFromDrag(nuevoAncho);
        }
    }
}