using UnityEngine;

public class CreatureBehaviour : MonoBehaviour
{
    public CreatureData PrevData;
    public CreatureData CurData;

    public void UpdateData(CreatureData newData)
    {
        PrevData = CurData;
        CurData = newData;

        transform.position = new Vector3(PrevData.PosX, PrevData.PosY, PrevData.PosZ);
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }

    public CreatureBehaviour(CreatureData data)
    {
        UpdateData(data);
    }

    private void Update()
    {
        if (PrevData != null)
        {
            Vector3 direction = new Vector3(CurData.PosX - PrevData.PosX, CurData.PosY - PrevData.PosY,
                CurData.PosZ - PrevData.PosZ).normalized;
            transform.TransformDirection(Time.deltaTime * PrevData.Speed * direction);
        }
    }
}