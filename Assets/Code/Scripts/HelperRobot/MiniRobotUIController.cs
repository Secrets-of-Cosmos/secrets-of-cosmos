using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniRobotUIController : MonoBehaviour
{
    [SerializeField] private HologramMenuController menu;
    [SerializeField] private InputActionReference openMenuAction;
    private bool isMenuOpen;
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            
            if (!isMenuOpen) menu.OpenMenu();
            else menu.CloseMenu();
            
            isMenuOpen = !isMenuOpen;
        }
        
    }
}
