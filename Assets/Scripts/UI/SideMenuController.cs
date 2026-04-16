using UnityEngine;
using UnityEngine.UI;

public class SideMenuController : MonoBehaviour
{
    [Header("Referencias principales")]
    [SerializeField] private SideMenuResizable sideMenu;
    [SerializeField] private Image darkOverlay;

    [Header("Pestañas")]
    [SerializeField] private GameObject optionsTab;
    [SerializeField] private GameObject fillerTab;

    [Header("Botones pestañas")]
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button fillerButton;

    [Header("Grid relleno")]
    [SerializeField] private RectTransform fillerContentRect;
    [SerializeField] private GridLayoutGroup fillerGrid;
    [SerializeField] private float minCellWidth = 90f;
    [SerializeField] private float cellHeight = 90f;
    [SerializeField] private float spacing = 10f;
    [SerializeField] private int horizontalPadding = 20;

    private void Awake()
    {
        if (sideMenu != null)
        {
            sideMenu.OnWidthChanged += UpdateGrid;
            sideMenu.OnMenuStateChanged += OnMenuStateChanged;
        }

        if (optionsButton != null)
            optionsButton.onClick.AddListener(ShowOptionsTab);

        if (fillerButton != null)
            fillerButton.onClick.AddListener(ShowFillerTab);
    }

    private void Start()
    {
        ShowOptionsTab();
        UpdateGrid();
        OnMenuStateChanged(sideMenu != null && sideMenu.EstaAbierto);
    }

    private void OnDestroy()
    {
        if (sideMenu != null)
        {
            sideMenu.OnWidthChanged -= UpdateGrid;
            sideMenu.OnMenuStateChanged -= OnMenuStateChanged;
        }

        if (optionsButton != null)
            optionsButton.onClick.RemoveListener(ShowOptionsTab);

        if (fillerButton != null)
            fillerButton.onClick.RemoveListener(ShowFillerTab);
    }

    public void ToggleMenu()
    {
        if (sideMenu == null) return;
        sideMenu.ToggleMenu();
    }

    public void OpenMenu()
    {
        if (sideMenu == null) return;
        sideMenu.AbrirMenu();
    }

    public void CloseMenu()
    {
        if (sideMenu == null) return;
        sideMenu.CerrarMenu();
    }

    public void ShowOptionsTab()
    {
        if (optionsTab != null) optionsTab.SetActive(true);
        if (fillerTab != null) fillerTab.SetActive(false);
    }

    public void ShowFillerTab()
    {
        if (optionsTab != null) optionsTab.SetActive(false);
        if (fillerTab != null) fillerTab.SetActive(true);

        UpdateGrid();
    }

    private void OnMenuStateChanged(bool abierto)
    {
        if (darkOverlay != null)
        {
            darkOverlay.raycastTarget = abierto;
            Color c = darkOverlay.color;
            c.a = abierto ? 0.35f : 0f;
            darkOverlay.color = c;
        }
    }

    public void UpdateGrid()
    {
        if (fillerGrid == null || fillerContentRect == null)
            return;

        float availableWidth = fillerContentRect.rect.width - horizontalPadding;
        if (availableWidth <= 0f)
            return;

        int columns = Mathf.Max(1, Mathf.FloorToInt((availableWidth + spacing) / (minCellWidth + spacing)));

        float totalSpacing = spacing * (columns - 1);
        float cellWidth = (availableWidth - totalSpacing) / columns;
        cellWidth = Mathf.Max(10f, cellWidth);

        fillerGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        fillerGrid.constraintCount = columns;
        fillerGrid.cellSize = new Vector2(cellWidth, cellHeight);
        fillerGrid.spacing = new Vector2(spacing, spacing);

        LayoutRebuilder.ForceRebuildLayoutImmediate(fillerContentRect);
    }
}