using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Disruptor : MonoBehaviour
{
    [SerializeField] private float timeInSeconds;
    [SerializeField] private string requiredAnimationBoolName;
    [SerializeField] private string conditionAnimationBoolName = string.Empty;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
            if(CheckCondition(other.GetComponent<Player>()))
                other.GetComponent<Player>().StartCoroutine(Disrupt(other.GetComponent<Player>()));
    }
    bool CheckCondition(Player player)
    {
        return conditionAnimationBoolName != string.Empty && player.Animator.GetBool(conditionAnimationBoolName);
    }
    public IEnumerator Disrupt(Player player)
    {
        player.Animator.SetBool(requiredAnimationBoolName, true);
        player.CanMove = false;

        yield return new WaitForSeconds(timeInSeconds);

        player.Animator.SetBool(requiredAnimationBoolName, false);
        player.CanMove = true;
    }
    
}
