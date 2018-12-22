using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    List<Arm> Arms;
    public int State = 0;
    public List<float> Genetic1;
    public List<float> Genetic2;

    public float Power;
    // Start is called before the first frame update
    void Start()
    {
        Arms = new List<Arm>();
        for (int i = 0; i < 4; i++)
        {
            Arm arm = new Arm
            {
                Leg = transform.GetChild((i * 2) + 1).gameObject,
                LegHinge = transform.GetChild((i * 2) + 2).gameObject
            };
            Arms.Add(arm);
        }
        
        CreateGenetic();
        StartCoroutine(Runnig());
    }
    

    void CreateGenetic()
    {
        Genetic1.Clear();
        Genetic2.Clear();

        for (int i = 0; i < 4; i++)
        {
            Genetic1.Add(Random.Range(-30.0f, 30.0f));
            Genetic1.Add(Random.Range(-60.0f, 60.0f));
        }
        for (int i = 0; i < 4; i++)
        {
            Genetic2.Add(Random.Range(-30.0f, 30.0f));
            Genetic2.Add(Random.Range(-60.0f, 60.0f));
        }
    }
    IEnumerator Runnig()
    {
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                Arms[i].Leg.transform.Rotate(new Vector3(Genetic1[i * 2], 0,0));
                Arms[i].LegHinge.transform.Rotate(new Vector3(Genetic1[i * 2 + 1], 0, 0));
            }
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < 4; i++)
            {
                Arms[i].Leg.transform.Rotate(new Vector3(Genetic2[i * 2], 0, 0));
                Arms[i].LegHinge.transform.Rotate(new Vector3(Genetic2[i * 2 + 1], 0, 0));
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

}
