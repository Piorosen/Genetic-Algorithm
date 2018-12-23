using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GeneticType = System.Collections.Generic.List<System.Collections.Generic.List<float>>;
public class Dragon : MonoBehaviour
{
    List<Arm> Arms;
    public int State = 0;
    public GeneticType Genetic;

    int GeneticSize = 4;
    public List<float> Move = new List<float>();

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
        StartCoroutine(Running());
    }
    

    public void CreateGenetic(float transition = 0, GeneticType geneticData = null)
    {
        Genetic = new List<List<float>>();
        Genetic.Clear();
        if (geneticData == null)
        {
            for (int k = 0; k < GeneticSize; k++)
            {
                List<float> tmp = new List<float>();
                for (int i = 0; i < 4; i++)
                {
                    tmp.Add(Random.Range(-15.0f, 30.0f));
                    tmp.Add(Random.Range(-60.0f, 60.0f));
                }
                Genetic.Add(tmp);
            }
        }
        else
        {
            Genetic = geneticData;

            for (int i = 0; i < Genetic.Count; i++)
            {
                for (int k = 0; k < Genetic[i].Count / 2; k++)
                {
                    float tmp = Random.value;
                    float tmp2 = Random.value;
                    if (transition >= tmp) 
                    {
                        Genetic[i][k * 2] = Random.Range(-15.0f, 30.0f);
                        Debug.Log($"Genetic[{i}][{k * 2}]");
                    }
                    if (transition >= tmp2)
                    {
                        Genetic[i][k * 2] = Random.Range(-60.0f, 60.0f);
                        Debug.Log($"Genetic[{i}][{k * 2}]");
                    }
                }
            }
            
        }
    }

    IEnumerator Running()
    {
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                var leg = Arms[i].Leg.GetComponent<HingeJoint>();
                var legHinge = Arms[i].LegHinge.GetComponent<HingeJoint>();
                var t = new JointMotor
                {
                    force = 2000,
                    freeSpin = false,
                    targetVelocity = Genetic[State][i * 2] > Arms[i].Leg.transform.rotation.x ? 200 : -200
                };

                leg.motor = t;

                t.targetVelocity = Genetic[State][i * 2 + 1] > Arms[i].LegHinge.transform.rotation.x ? 200 : -200;
                Arms[i].LegHinge.GetComponent<HingeJoint>().motor = t;

                var joint = new JointLimits
                {
                    bounciness = 0.0f,
                    bounceMinVelocity = 0.002f,
                    contactDistance = 0,
                    max = 60f,
                    min = -60f
                };

                if (Genetic[State][i * 2] > Arms[i].LegHinge.transform.rotation.x)
                {
                    joint.max = Genetic[State][i * 2];
                }
                else
                {
                    joint.min = Genetic[State][i * 2];
                }

                leg.limits = joint;

                joint = new JointLimits
                {
                    bounciness = 0.0f,
                    bounceMinVelocity = 0.002f,
                    contactDistance = 0,
                    max = 60f,
                    min = -60f
                };
                if (Genetic[State][i * 2 + 1] > Arms[i].LegHinge.transform.rotation.x)
                {
                    joint.max = Genetic[State][i * 2 + 1];
                }
                else
                {
                    joint.min = Genetic[State][i * 2 + 1];
                }
                legHinge.limits = joint;
            }

            State++;
            if (State == Genetic.Count)
            {
                State = 0;
            }
            yield return new WaitForSeconds(0.25f);

        }
    }
    

}
