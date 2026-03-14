using UnityEngine;

public class PrewiewScript : MonoBehaviour
{

    [SerializeField] Collider[] ownColliders;
    private void Awake()
    {
        ownColliders = GetComponentsInChildren<Collider>();
    }

    public bool IsValid()
    {
        return !IsPreviewColliding();
    }

    // ChatGPT's Method (probably needs fixing/optimizing)
    bool IsPreviewColliding()
    {
        foreach (var own in ownColliders)
        {
            Collider[] hits = Physics.OverlapBox(
                own.bounds.center,
                own.bounds.extents,
                own.transform.rotation,
                ~BuildingSystem.Instance.buildOnLayer
            );

            foreach (var hit in hits)
            {
                if (!hit.GetComponent<StructureScript>())
                    continue;
                if (Physics.ComputePenetration(
                    own, own.transform.position, own.transform.rotation,
                    hit, hit.transform.position, hit.transform.rotation,
                    out Vector3 direction, out float distance))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
