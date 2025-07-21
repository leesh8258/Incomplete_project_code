using UnityEngine;

public class DeathStateBehaviour : StateMachineBehaviour
{
    // Death 상태가 종료될 때 호출됨
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // animator는 자식 오브젝트에 있으므로, 부모를 가져옴
        Transform parent = animator.transform.parent;
        if (parent != null)
        {
            Enemy deathHandler = parent.GetComponent<Enemy>();
            if (deathHandler != null)
            {
                deathHandler.StartCoroutine(deathHandler.StartDissolve());
            }
        }
    }
}
