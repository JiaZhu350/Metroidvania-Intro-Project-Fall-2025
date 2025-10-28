using UnityEngine;

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

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void Update()
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
            MoveInDirection(-1);
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

    private void DirectionChange()
    {
        movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        // Movement logic here
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        enemy.position = new Vector3(enemy.position.x + _direction * Time.deltaTime * speed, enemy.position.y, enemy.position.z);
        Debug.Log("Moving in direction: " + _direction);
    }
}
