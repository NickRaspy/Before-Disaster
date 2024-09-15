using UnityEngine;

public class UIInteractable : Interactable
{
    [SerializeField] private GameObject UIElement;
    public override void Action()
    {
        UIElement.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        InteractingPlayer = null;
        DisableElement();
        InteractableSet(false, other);
    }
    public void DisableElement() => UIElement.SetActive(false);
}
