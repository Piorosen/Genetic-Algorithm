using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GeneticType = System.Collections.Generic.List<System.Collections.Generic.List<int>>;
using GeneticSmall = System.Collections.Generic.List<int>;

public class Human : MonoBehaviour
{
    public int State = 0;
    public GeneticType Genetic;
    
    int GeneticSize = 2;

    public float MaxSpeed;

    Rigidbody rigid;

    /// <summary>
    /// 관절 데이터를 얻어옵니다.
    /// </summary>
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
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
                GeneticSmall tmp = new GeneticSmall();
                for (int i = 0; i < 24; i++)
                {
                    tmp.Add(Random.Range(-25, 25));
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
                GeneticSmall q = new GeneticSmall();
                for (int e = 0; e < geneticData[w].Count; e++)
                {
                    q.Add(geneticData[w][e]);
                }
                type.Add(q);
            }

            Genetic = type;

            for (int i = 0; i < Genetic.Count; i++)
            {
                for (int k = 0; k < Genetic[i].Count; k++)
                {
                    float tmp = Random.value;
                    if (transition >= tmp)
                    {
                        Genetic[i][k] = Random.Range(-25, 25);
                    }
                }
            }
        }
    }

    public void Update()
    {
        if (MaxSpeed < rigid.velocity.magnitude)
        {
            MaxSpeed = rigid.velocity.magnitude;
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
            State = 0;
            for (; State < Genetic.Count; State++)
            {
                for (int k = 0; k < Genetic[State].Count; k++)
                {
                    rigid.AddForce(Vector3.forward * Genetic[State][k]);
                    yield return new WaitForSeconds(1.0f / Genetic[State].Count);
                }
            }

        }
    }
}
