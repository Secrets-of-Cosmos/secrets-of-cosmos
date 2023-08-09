using UnityEngine;
using UnityEngine.Events;

public class CardViewButtons : MonoBehaviour, IButton {
    
    [SerializeField] private string normalState = "Normal";
    [SerializeField] private string highlightedState = "Highlighted";
    [SerializeField] private string pressedState = "Pressed";
    
    [SerializeField] private UnityEvent onClick = new();
    
    private Animator animator;
    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void OnMouseEnter() {
        animator.Play(highlightedState);
    }
    
    public void OnMouseExit() {
        animator.Play(normalState);
    }
    
    public void OnMouseDown() {
        animator.Play(pressedState);
        onClick.Invoke();
    }
    
    public void OnMouseUp() {
        animator.Play(normalState);
    }
}
