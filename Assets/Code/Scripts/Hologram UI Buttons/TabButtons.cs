using UnityEngine;

public class Tabs : MonoBehaviour, IButton
{
    [SerializeField] private TabsType buttonType;
    [SerializeField] private float selectedColorIntensity;
    
    private Material _material;

    public void Start() {
        _material = GetComponent<MeshRenderer>().material;
    }

    public void OnMouseDown() {
        HologramMenuController.Instance.OnTabSelected(buttonType);
        LeanTween.scale(gameObject, new Vector3(0.6f, 0.6f, 0.6f), 0.5f).setEaseOutBack();
        _material.SetColor("_EmissionColor", _material.color * selectedColorIntensity);
    }

    public void OnMouseEnter() {
        Debug.Log(buttonType + " OnMouseEnter");
        LeanTween.rotateY(gameObject, 180f, 0.5f).setEaseOutBack();
    }
    
    public void OnMouseExit() {
        Debug.Log(buttonType + " OnMouseExit");
        LeanTween.rotateY(gameObject, 0f, 0.5f).setEaseOutBack();
    }
}

public enum TabsType {
    NOT_SELECTED,
    MISSIONS,
    PLANETS,
    SPACECRAFTS,
    INVENTORY,
    MAPS,
    DICTIONARY,
}