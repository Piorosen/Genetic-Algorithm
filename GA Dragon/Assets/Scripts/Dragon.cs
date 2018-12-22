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
    List<float> Move = new List<float>();

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
                    Genetic[i][k * 2] += Random.Range(-15.0f, 30.0f) * transition;
                    Genetic[i][k * 2 + 1] += Random.Range(-60.0f, 60.0f) * transition;
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
                var t = new JointMotor
                {
                    force = 2000,
                    freeSpin = false,
                    targetVelocity = Genetic[State][i * 2] > Arms[i].Leg.transform.rotation.x ? 200 : -200
                };
                Arms[i].Leg.GetComponent<HingeJoint>().useMotor = true;
                Arms[i].Leg.GetComponent<HingeJoint>().motor = t;
                t.targetVelocity = Genetic[State][i * 2 + 1] > Arms[i].LegHinge.transform.rotation.x ? 200 : -200;
                Arms[i].LegHinge.GetComponent<HingeJoint>().useMotor = true;
                Arms[i].LegHinge.GetComponent<HingeJoint>().motor = t;
            }

            State++;
            if (State == Genetic.Count)
            {
                State = 0;
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void Update()
    {
        if (Genetic == null)
        {
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            var leg = Arms[i].Leg.GetComponent<HingeJoint>();
            var leghinge = Arms[i].LegHinge.GetComponent<HingeJoint>();

            if (leg.motor.targetVelocity > 0)
            {
                if (leg.transform.rotation.x > Genetic[State][i * 2])
                {
                    leg.useMotor = false;
                }
            }
            else
            {
                if (leg.transform.rotation.x < Genetic[State][i * 2])
                {
                    leg.useMotor = false;
                }
            }

            if (leghinge.motor.targetVelocity > 0)
            {
                if (leghinge.transform.rotation.x > Genetic[State][i * 2 + 1])
                {
                    leg.useMotor = false;
                }
            }
            else
            {
                if (Arms[i].LegHinge.transform.rotation.x < Genetic[State][i * 2 + 1])
                {
                    leg.useMotor = false;
                }
            }

        }

    }    

}
