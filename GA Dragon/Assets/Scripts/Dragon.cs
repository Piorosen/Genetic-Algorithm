using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GeneticType = System.Collections.Generic.List<System.Collections.Generic.List<float>>;


public class Dragon : MonoBehaviour
{
    public int State = 0;
    public GeneticType Genetic;

    public List<float> Move = new List<float>();

    int GeneticSize = 4;
    List<Arm> Arms;
    
    /// <summary>
    /// 관절 데이터를 얻어옵니다.
    /// </summary>
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
    
    /// <summary>
    /// 유전자 생성, () 이 될 경우 랜덤으로 생성
    /// </summary>
    /// <param name="transition">변이율</param>
    /// <param name="geneticData">이전 데이터</param>
    public void CreateGenetic(float transition = 0, GeneticType geneticData = null)
    {
        Genetic = new GeneticType();
        Genetic.Clear();
        // 유전 데이터 생성
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
            // 유전 데이터 클론
            GeneticType type = new GeneticType();
            for (int w = 0; w < geneticData.Count; w++)
            {
                List<float> q = new List<float>();
                for (int e = 0; e < geneticData[w].Count; e++)
                {
                    q.Add(geneticData[w][e]);
                }
                type.Add(q);
            }

            Genetic = type;

            for (int i = 0; i < Genetic.Count; i++)
            {
                for (int k = 0; k < Genetic[i].Count / 2; k++)
                {
                    float tmp = Random.value;
                    float tmp2 = Random.value;
                    if (transition >= tmp) 
                    {
                        Genetic[i][k * 2] = Random.Range(-15.0f, 30.0f);
                    }
                    if (transition >= tmp2)
                    {
                        Genetic[i][k * 2 + 1] = Random.Range(-60.0f, 60.0f);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 시작
    /// </summary>
    /// <returns>리턴값, 대기하는 함수임</returns>
    IEnumerator Running()
    {
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                var leg = Arms[i].Leg.GetComponent<HingeJoint>();
                var legHinge = Arms[i].LegHinge.GetComponent<HingeJoint>();
                #region 관절 이동할 방향 설정
                leg.motor = new JointMotor
                {
                    force = 2000,
                    targetVelocity = Genetic[State][i * 2] > Arms[i].Leg.transform.rotation.x ? 200 : -200
                };
                
                legHinge.motor = new JointMotor
                {
                    force = 2000,
                    targetVelocity = Genetic[State][i * 2 + 1] > Arms[i].LegHinge.transform.rotation.x ? 200 : -200
                };
                #endregion

                #region 관절의 최대 회전 각 설정
                leg.limits = new JointLimits
                {
                    max = (Genetic[State][i * 2] > Arms[i].LegHinge.transform.rotation.x) ? Genetic[State][i * 2] : 30f,
                    min = (Genetic[State][i * 2] > Arms[i].LegHinge.transform.rotation.x) ? -15f : Genetic[State][i * 2]
                };
                legHinge.limits = new JointLimits
                {
                    max = (Genetic[State][i * 2 + 1] > Arms[i].LegHinge.transform.rotation.x) ? Genetic[State][i * 2 + 1] : 60f,
                    min = (Genetic[State][i * 2 + 1] > Arms[i].LegHinge.transform.rotation.x) ? -60f : Genetic[State][i * 2 + 1]
                };
                #endregion
            }
            Move = Genetic[State];
            State++;
            if (State == Genetic.Count)
            {
                State = 0;
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
    

}
