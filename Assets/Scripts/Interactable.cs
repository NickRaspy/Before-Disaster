using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public Player InteractingPlayer { get; set; }

    public void Interact() => Action();
    public abstract void Action();
}
public interface IInteractable
{
    public void Action();
}