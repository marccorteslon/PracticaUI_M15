using System;
using UnityEngine;

public class SideMenuResizable : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private RectTransform resizeHandleRect;

    [Header("Tamaños")]
    [SerializeField] private float anchoCerrado = 0f;
    [SerializeField] private float anchoAbiertoPorDefecto = 320f;
    [SerializeField] private float anchoMinimo = 260f;
    [SerializeField] private float anchoMaximo = 600f;

    [Header("Animación")]
    [SerializeField] private float velocidadAnimacion = 12f;
    [SerializeField] private bool empezarAbierto = false;

    private float anchoActual;
    private float anchoObjetivo;
    private float ultimoAnchoAbierto;
    private bool abierto;

    public bool EstaAbierto => abierto;
    public float AnchoActual => anchoActual;
    public float AnchoMinimo => anchoMinimo;
    public float AnchoMaximo => anchoMaximo;

    public Action OnWidthChanged;
    public Action<bool> OnMenuStateChanged;

    private void Awake()
    {
        if (panelRect == null)
            panelRect = GetComponent<RectTransform>();

        ultimoAnchoAbierto = Mathf.Clamp(anchoAbiertoPorDefecto, anchoMinimo, anchoMaximo);

        abierto = empezarAbierto;
        anchoActual = abierto ? ultimoAnchoAbierto : anchoCerrado;
        anchoObjetivo = anchoActual;

        AplicarEstado(true);
        OnMenuStateChanged?.Invoke(abierto);
    }

    private void Update()
    {
        float anchoAnterior = anchoActual;

        anchoActual = Mathf.Lerp(anchoActual, anchoObjetivo, velocidadAnimacion * Time.unscaledDeltaTime);

        if (Mathf.Abs(anchoActual - anchoObjetivo) < 0.1f)
            anchoActual = anchoObjetivo;

        bool changed = Mathf.Abs(anchoActual - anchoAnterior) > 0.01f;
        AplicarEstado(changed);
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
        OnMenuStateChanged?.Invoke(true);
    }

    public void CerrarMenu()
    {
        abierto = false;
        anchoObjetivo = anchoCerrado;
        OnMenuStateChanged?.Invoke(false);
    }

    public void SetWidthFromDrag(float nuevoAncho)
    {
        nuevoAncho = Mathf.Clamp(nuevoAncho, anchoMinimo, anchoMaximo);

        ultimoAnchoAbierto = nuevoAncho;
        anchoObjetivo = nuevoAncho;

        if (!abierto)
        {
            abierto = true;
            OnMenuStateChanged?.Invoke(true);
        }
    }

    public void SetWidthInstant(float nuevoAncho)
    {
        nuevoAncho = Mathf.Clamp(nuevoAncho, anchoMinimo, anchoMaximo);

        ultimoAnchoAbierto = nuevoAncho;
        anchoActual = nuevoAncho;
        anchoObjetivo = nuevoAncho;
        abierto = true;

        AplicarEstado(true);
        OnMenuStateChanged?.Invoke(true);
    }

    private void AplicarEstado(bool notify)
    {
        if (panelRect != null)
            panelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, anchoActual);

        if (resizeHandleRect != null)
        {
            resizeHandleRect.anchorMin = new Vector2(1f, 0.5f);
            resizeHandleRect.anchorMax = new Vector2(1f, 0.5f);
            resizeHandleRect.pivot = new Vector2(0.5f, 0.5f);
            resizeHandleRect.anchoredPosition = new Vector2(0f, 0f);
        }

        if (notify)
            OnWidthChanged?.Invoke();
    }
}