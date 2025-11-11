using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement Parameter")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;
    private bool patrolInterupted = false;
    private bool isResetting = false;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void Update()
    {
        if (!patrolInterupted && !isResetting)
            PatrolMode();
    }

    private void DirectionChange()
    {
        anim.SetBool("moving", false);
        movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        anim.SetBool("moving", true);
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        enemy.position = new Vector3(enemy.position.x + _direction * Time.deltaTime * speed, enemy.position.y, enemy.position.z);
        //Debug.Log("Moving in direction: " + _direction);
    }

    private void PatrolMode()
    {
         if (movingLeft)
                {
                    if (enemy.position.x >= leftEdge.position.x)
                    {
                        MoveInDirection(-1);
                    }
                    else
                    {
                        DirectionChange();
                    }
                }
                else
                {
                    if (enemy.position.x <= rightEdge.position.x)
                    {
                        MoveInDirection(1);
                    }
                    else
                    {
                        DirectionChange();
                    }
                }
    }

    public void Interupted()
    {
        patrolInterupted = true;
        if (isResetting)
        {
            StopAllCoroutines(); // or keep a coroutine handle and StopCoroutine(handle)
            isResetting = false;
        }

    }

    public void ResetPatrol()
    {
        if(!isResetting)
            StartCoroutine(ReturnToClosetEdge());
    }


    private IEnumerator ReturnToClosetEdge()
    {
        isResetting = true;
        float distLeft = Vector3.Distance(enemy.position, leftEdge.position);
        float distRight = Vector3.Distance(enemy.position, rightEdge.position);

        Transform targetEdge = distLeft < distRight ? leftEdge : rightEdge;

        while (Vector3.Distance(enemy.position, targetEdge.position) > 0.099f)
        {
            int direction = targetEdge.position.x < enemy.position.x ? -1 : 1;
            MoveInDirection(direction);
            yield return null; // wait one frame
        }

        enemy.position = new Vector3(targetEdge.position.x, enemy.position.y, enemy.position.z);

        isResetting = false;
        patrolInterupted = false;
        movingLeft = (targetEdge == rightEdge);
    }
}
