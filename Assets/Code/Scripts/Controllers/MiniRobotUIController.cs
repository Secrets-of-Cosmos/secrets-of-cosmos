using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniRobotUIController : MonoBehaviour
{
    [SerializeField] private HologramMenuController menu;
    [SerializeField] private InputActionReference openMenuAction;
    public bool IsMenuOpen { get; private set; }
    
    private void Start() {
        menu.gameObject.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            if (!IsMenuOpen) {
                menu.OpenMenu();
            }
            else {
                menu.CloseMenu();
            }
            IsMenuOpen = !IsMenuOpen;
        }
    }
}
