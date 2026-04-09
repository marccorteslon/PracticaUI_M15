using UnityEngine;

public class SideMenuResizable : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private RectTransform toggleButtonRect;

    [Header("Tamaños")]
    [SerializeField] private float anchoCerrado = 0f;
    [SerializeField] private float anchoAbiertoPorDefecto = 300f;
    [SerializeField] private float anchoMinimo = 180f;
    [SerializeField] private float anchoMaximo = 600f;

    [Header("Animación")]
    [SerializeField] private float velocidad = 10f;
    [SerializeField] private bool empezarAbierto = false;

    private float anchoObjetivo;
    private float anchoActual;
    private float ultimoAnchoAbierto;
    private bool abierto;

    public float AnchoMinimo => anchoMinimo;
    public float AnchoMaximo => anchoMaximo;
    public bool EstaAbierto => abierto;

    private void Awake()
    {
        if (panelRect == null)
            panelRect = GetComponent<RectTransform>();

        ultimoAnchoAbierto = Mathf.Clamp(anchoAbiertoPorDefecto, anchoMinimo, anchoMaximo);

        abierto = empezarAbierto;
        anchoActual = abierto ? ultimoAnchoAbierto : anchoCerrado;
        anchoObjetivo = anchoActual;

        AplicarAncho(anchoActual);
    }

    private void Update()
    {
        anchoActual = Mathf.Lerp(anchoActual, anchoObjetivo, velocidad * Time.deltaTime);

        // Evita vibración al llegar al final
        if (Mathf.Abs(anchoActual - anchoObjetivo) < 0.1f)
            anchoActual = anchoObjetivo;

        AplicarAncho(anchoActual);
    }

    public void ToggleMenu()
    {
        if (abierto)
            CerrarMenu();
        else
            AbrirMenu();
    }

    public void AbrirMenu()
    {
        abierto = true;
        anchoObjetivo = ultimoAnchoAbierto;
    }

    public void CerrarMenu()
    {
        abierto = false;
        anchoObjetivo = anchoCerrado;
    }

    public void SetWidthInstant(float nuevoAncho)
    {
        nuevoAncho = Mathf.Clamp(nuevoAncho, anchoMinimo, anchoMaximo);

        ultimoAnchoAbierto = nuevoAncho;
        abierto = nuevoAncho > anchoCerrado + 0.01f;

        anchoActual = nuevoAncho;
        anchoObjetivo = nuevoAncho;

        AplicarAncho(anchoActual);
    }

    public void SetWidthFromDrag(float nuevoAncho)
    {
        nuevoAncho = Mathf.Clamp(nuevoAncho, anchoMinimo, anchoMaximo);

        ultimoAnchoAbierto = nuevoAncho;
        abierto = true;
        anchoObjetivo = nuevoAncho;
    }

    private void AplicarAncho(float ancho)
    {
        panelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ancho);

        // Coloca el botón justo fuera del borde derecho del panel
        if (toggleButtonRect != null)
        {
            float mitadBoton = toggleButtonRect.rect.width * 0.5f;
            toggleButtonRect.anchoredPosition = new Vector2(mitadBoton, 0f);
        }
    }
}