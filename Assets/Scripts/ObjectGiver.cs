using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(UIInteractable))]
public class ObjectGiver : MonoBehaviour
{
    [Inject] GameManager gameManager;
    [Inject] List<PickableObject> pickableObjects;

    public void GiveObject(GameObject go)
    {
        Debug.Log(pickableObjects.Any(x => x.gameObject == go));
        if (GetComponent<UIInteractable>() == null || !pickableObjects.Any(x => x.gameObject == go)) return;

        UIInteractable inter = GetComponent<UIInteractable>();

        if (inter.InteractingPlayer.HoldableObject != null) inter.InteractingPlayer.RemoveObject();

        PickableObject pickableObject = pickableObjects.Find(x => x.gameObject == go);
        switch (pickableObject.type)
        {
            case PickableObject.Type.Standard:
                inter.InteractingPlayer.SetBigObjectMode(false);
                inter.InteractingPlayer.GiveObject(go, false);
                break;
            case PickableObject.Type.Big:
                inter.InteractingPlayer.SetBigObjectMode(true);
                inter.InteractingPlayer.GiveObject(go, true);
                break;
        }

    }
}
