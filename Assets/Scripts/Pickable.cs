using UnityEngine;

public class Pickable : Interactable
{
    [SerializeField] private GameObject givableObject;

    public override void Action()
    {
        if(InteractingPlayer.HoldableObject != null) return;

        InteractingPlayer.HoldableObject = Instantiate(givableObject, InteractingPlayer.objectHolder);
    }
}
