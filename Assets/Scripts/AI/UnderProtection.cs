using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnderProtection : MonoBehaviour
{

    private List<Protect> protectors = new List<Protect>();        
    private float protectorOffset = 24f;


    private void Start()
    {
        GameEvents.current.onProtectorsDeath += RemoveProtector;
    }

    public void AddProtector(Protect protector)
    {
        protectors.Add(protector);
        protectors = protectors.OrderBy(p => Vector2.Distance(p.transform.position, transform.position)).ToList();
        RecalculateOffsets();
    }

    public void RemoveProtector(Protect protector)
    {
        protectors.Remove(protector);
        protectors = protectors.OrderBy(p => Vector2.Distance(p.transform.position, transform.position)).ToList();
        if (protectors.Count == 0)
            Destroy(this);

        RecalculateOffsets();
    }

    private void RecalculateOffsets()
    {
        for (int i = 0; i < protectors.Count; i++)
        {
            protectors[i].offset = CalculateOffset(i);
        }
    }

    private float CalculateOffset(int protectorIndex)
    {
        int offsetSign = (protectorIndex - 1) % 2 == 1 ? 1 : -1;
        return (protectorIndex /2 ) * protectorOffset * offsetSign;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < protectors.Count; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, protectors[i].transform.position);
        }
    }
}
