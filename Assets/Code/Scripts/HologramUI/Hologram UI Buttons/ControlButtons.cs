using UnityEngine;

public class ControlButtons : MonoBehaviour, IButton
{
    [SerializeField] private ControlButtonsType buttonType;
    [SerializeField] private float selectedColorIntensity;
    
    private Material material;
    private Color originalColor;
    
    private void Start() {
        material = GetComponent<Renderer>().material;
        originalColor = material.GetColor("_EmissionColor");
    }
    
    
    public void OnMouseDown() {
        HologramMenuController.Instance.OnControlButtonsPressed(buttonType);
    }
    
    public void OnMouseEnter() {
        material.SetColor("_EmissionColor", originalColor * selectedColorIntensity);
    }
    
    public void OnMouseExit() {
        material.SetColor("_EmissionColor", originalColor);
    }
}

public enum ControlButtonsType {
    LEFT,
    RIGHT,
}
