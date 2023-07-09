using UnityEngine;

public class ControlButtons : MonoBehaviour, IButton
{
    [SerializeField] private ControlButtonsType buttonType;
    public void OnMouseDown() {
        HologramMenuController.Instance.OnControlButtonsPressed(buttonType);
    }
}

public enum ControlButtonsType {
    LEFT,
    RIGHT,
}
