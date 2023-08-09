using UnityEngine;

public class Tabs : MonoBehaviour, IButton
{
    [SerializeField] private TabType buttonType;
    [SerializeField] private float selectedColorIntensity;
    
    private Material material;

    public void Start() {
        material = GetComponent<MeshRenderer>().material;
    }

    public void OnMouseDown() {
        HologramMenuController.Instance.OnTabSelected(buttonType);
        LeanTween.scale(gameObject, new Vector3(0.6f, 0.6f, 0.6f), 0.5f).setEaseOutBack();
        material.SetColor("_EmissionColor", material.color * selectedColorIntensity);
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

public enum TabType {
    NOT_SELECTED,
    MISSIONS,
    PLANETS,
    SPACECRAFTS,
    MAPS
}