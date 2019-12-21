using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    private ServerGameData cur;

    public GameObject ProjectilePrefab;
    public GameObject CreaturePrefab;

    public bool IsPlayerDead = false;

    public List<ProjectileBehaviour> ProjectileBehaviours = new List<ProjectileBehaviour>();
    public List<CreatureBehaviour> CreatureBehaviours = new List<CreatureBehaviour>();

    public void UpdateData(ServerGameData serverGameData, string guid)
    {
        cur = serverGameData;

        foreach (var prjData in serverGameData.ProjectileDatas)
        {
            var prj = ProjectileBehaviours.Find(x => x.Data.Guid == prjData.Guid);
            if (prj == null && !prjData.IsDead)
            {
                GameObject obj = Instantiate(ProjectilePrefab);
                ProjectileBehaviours.Add(obj.GetComponent<ProjectileBehaviour>());
            }
            else
            {
                if (prjData.IsDead)
                {
                    prj.Kill();
                }
                else
                {
                    prj.UpdateData(prjData);
                }
            }
        }

        foreach (var crtData in serverGameData.CreatureDatas)
        {
            if (crtData.Guid == guid)
            {
                continue;
            }

            var crt = CreatureBehaviours.Find(x => x.CurData.Guid == crtData.Guid);
            if (crt == null)
            {
                GameObject obj = Instantiate(CreaturePrefab);
                CreatureBehaviours.Add(obj.GetComponent<CreatureBehaviour>());
            }
            else
            {
                if (crtData.IsDead)
                {
                    crt.Kill();
                }
                else
                {
                    crt.UpdateData(crtData);
                }
            }
        }

        if (serverGameData.CreatureDatas.Any(x => x.Guid == guid && x.IsDead))
        {
            IsPlayerDead = true;
        }
    }
}