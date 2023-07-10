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
        var currentRot = transform.localRotation.eulerAngles;
        currentRot.y = 180;
        LeanTween.rotateLocal(gameObject, currentRot, 0.5f).setEaseOutBack();
    }
    
    public void OnMouseExit() {
        var currentRot = transform.localRotation.eulerAngles;
        currentRot.y = 0;
        LeanTween.rotateLocal(gameObject, currentRot, 0.5f).setEaseOutBack();
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