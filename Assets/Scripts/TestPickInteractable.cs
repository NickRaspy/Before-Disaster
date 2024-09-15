using UnityEngine;

public class TestPickInteractable : Interactable
{
    public override void Action()
    {
        if (InteractingPlayer.HoldableObject != requiredObject) Debug.Log("test wrong");
        else Debug.Log("test correct");
    }
}
