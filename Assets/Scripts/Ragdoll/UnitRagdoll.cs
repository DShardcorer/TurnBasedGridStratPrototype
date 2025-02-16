using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;
    private Transform originalRootBone;

    [SerializeField] private Vector3 explosionOffset;


    public void SetUp(Transform originalRootBone)
    {
        explosionOffset = new Vector3(0, 5, 0);
        this.originalRootBone = originalRootBone;
        CopyOriginalBoneTransforms(originalRootBone, ragdollRootBone);
        ApplyRagdollExplosionForce(ragdollRootBone, 3000f, transform.position + explosionOffset, 10f);

    }

    private void CopyOriginalBoneTransforms(Transform originalRootBone, Transform ragdollRootBone)
    {
        foreach(Transform ragdollChildBone in ragdollRootBone)
        {
            Transform originalChildBone = originalRootBone.Find(ragdollChildBone.name);
            if(originalChildBone != null)
            {
                ragdollChildBone.position = originalChildBone.position;
                ragdollChildBone.rotation = originalChildBone.rotation;
            }
            CopyOriginalBoneTransforms(originalChildBone, ragdollChildBone);
        }
    }

    private void ApplyRagdollExplosionForce(Transform ragdollRootBone, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        foreach(Transform ragdollChildBone in ragdollRootBone)
        {
            if(ragdollChildBone.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
                ApplyRagdollExplosionForce(ragdollChildBone, explosionForce, explosionPosition, explosionRadius);
            }
        }
    }
}
