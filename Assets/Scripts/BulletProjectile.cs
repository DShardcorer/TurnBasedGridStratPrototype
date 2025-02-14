using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private GameObject hitEffectPrefab;
    private Coroutine moveCoroutine;
    private float speed = 200f;
    private Vector3 targetShotPosition;
    public void SetUp(Vector3 targetShotPosition)
    {
        SetTargetShotPosition(targetShotPosition);
        moveCoroutine = StartCoroutine(MoveCoroutine());
    }
    private void SetTargetShotPosition(Vector3 targetShotPosition)
    {
        this.targetShotPosition = targetShotPosition;
    }

    private IEnumerator MoveCoroutine()
    {
        while (true)
        {
            Vector3 moveDirection = (targetShotPosition - transform.position).normalized;
            float distanceBeforeMoving = Vector3.Distance(transform.position, targetShotPosition);

            transform.position += moveDirection * speed * Time.deltaTime;

            float distanceAfterMoving = Vector3.Distance(transform.position, targetShotPosition);
            if (distanceAfterMoving >= distanceBeforeMoving)
            {
                transform.position = targetShotPosition;
                trailRenderer.transform.parent = null;
                Destroy(gameObject);
                Instantiate(hitEffectPrefab, targetShotPosition, Quaternion.identity);

            }
            yield return null;
        }

    }
}
