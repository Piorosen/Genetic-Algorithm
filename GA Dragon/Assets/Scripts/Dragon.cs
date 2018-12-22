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
                    tmp.Add(Random.Range(-20.0f, 40.0f));
                    tmp.Add(Random.Range(-80.0f, 80.0f));
                }
                Genetic.Add(tmp);
            }
        }
        else
        {
            Genetic = geneticData;

            for (int i = 0; i < Genetic.Count; i++)
            {
                for (int k = 0; k < Genetic[i].Count; k++)
                {
                    Genetic[i][k] += Random.Range(-10.0f, 10.0f) * transition;
                }
            }
            
        }
    }

    IEnumerator Running()
    {
        for (int i = 0; i < 4; i++)
        {
            var t = new JointMotor
            {
                force = 10000,
                freeSpin = false,
                targetVelocity = Genetic[State][i * 2] > Arms[i].Leg.transform.rotation.x ? 1000 : -1000
            };
            // Arms[i].Leg.GetComponent<HingeJoint>().motor = t;
            Arms[i].Leg.transform.RotateAround(Vector3.zero, new Vector3(1, 0, 0), Genetic[State][i * 2]);
            t.targetVelocity = Genetic[State][i * 2 + 1];
            // Arms[i].LegHinge.GetComponent<HingeJoint>().motor = t;
            Arms[i].Leg.transform.RotateAround(Vector3.zero, new Vector3(1, 0, 0), Genetic[State][i * 2 + 1]);
        }
    }

    int frame = 0;
    private void FixedUpdate()
    {
        frame++;
        if (Move.Count != 0)
        {
            for (int i = 0; i < 4; i++)
            {
                Arms[i].Leg.transform.Rotate(new Vector3(1, 0, 0), Move[i * 2] * Time.fixedDeltaTime * 5);
                Arms[i].Leg.transform.Rotate(new Vector3(1, 0, 0), Move[i * 2 + 1] * Time.fixedDeltaTime * 5);
            }
        }
        if (frame % 10 == 0)
        {
            State++;
            Move.Clear();
            if (State == Genetic.Count)
            {
                State = 0;
            }


            for (int i = 0; i < 4; i++)
            {
                Move.Add(Arms[i / 2].Leg.transform.rotation.x - Genetic[State][i]);
                Move.Add(Arms[i / 2].LegHinge.transform.rotation.x - Genetic[State][i + 1]);
            }
            frame = 0;
        }

    }    

}
