using System.Collections;
using UnityEngine;

public class LongActionInteractable : Interactable
{
    [Space]
    [SerializeField] private float timeInSeconds;
    [SerializeField] private string requiredAnimationBoolName;
    public override void Action()
    {
        InteractingPlayer.StartCoroutine(LongAction());
    }
    public IEnumerator LongAction()
    {
        InteractingPlayer.Animator.SetBool(requiredAnimationBoolName, true);

        yield return new WaitForSeconds(timeInSeconds);

        InteractingPlayer.Animator.SetBool(requiredAnimationBoolName, false);
        onInteractionEnd.Invoke();
    }
}
