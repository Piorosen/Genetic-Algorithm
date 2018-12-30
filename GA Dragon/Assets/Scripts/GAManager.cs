using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using GeneticType = System.Collections.Generic.List<System.Collections.Generic.List<float>>;
public class GAManager : MonoBehaviour
{
    public GameObject Dragon;

    List<Dragon> GenerateDragon;
    List<Dragon> LivedDragon;

    public bool DoYouWantWatchToBestSolusion = false;

    GeneticType FarTest = new GeneticType();
    /// <summary>
    /// 생성할 드라군의 갯수
    /// </summary>
    public int CreateDragon;

    /// <summary>
    /// 현재 세대
    /// </summary>
    public int Gen;

    /// <summary>
    /// 생존 비율
    /// </summary>
    public float LiveRatio;

    /// <summary>
    /// 변이율
    /// </summary>
    public float Transition;

    /// <summary>
    /// 생존시간
    /// </summary>
    public float NextGenTime;

    /// <summary>
    /// 시작
    /// </summary>
    void Start()
    {
        FarTest.Add(new List<float>
        {
            -14.77732f, -14.70525f, -10.50904f, 34.67912f, -12.65775f, -25.13045f, 24.35605f, 32.6796f,
        });
        FarTest.Add(new List<float>
        {
            -9.569241f, 33.28701f, 20.17009f, 34.26298f, 19.53046f, 38.34041f, 12.75441f, -55.01688f
        });
        FarTest.Add(new List<float>
        {
            -4.35068f, 18.98749f, 3.297448f, -37.4022f, 18.82948f, 56.68201f, 10.95352f, 44.01205f
        });
        FarTest.Add(new List<float>
        {
            14.8979f, -14.26002f, 26.40041f, -32.22816f, 0.8945265f, -44.06739f, -10.30839f, -2.96925f,
        });

        GenerateDragon = new List<Dragon>();
        LivedDragon = new List<Dragon>();
        StartCoroutine(Run());
    }

    /// <summary>
    /// 드라군의 생존이야기가 시작됩니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator Run()
    {
        while (true)
        {
            NextGenerate();
            yield return new WaitForSeconds(NextGenTime);
        }
    }

    /// <summary>
    /// 세대 진화 시작
    /// </summary>
    void NextGenerate()
    {
        Gen++;
        if (GenerateDragon.Count != 0)
        {
            DragonSort();
            Debug.Log($"이전 세대 최고 치 : {Gen - 1}\n{GenerateDragon[0].transform.GetChild(0).transform.position.z}");
            Save(Gen);
            SurviveDragon();
            
        }
        CrossDragon();
    }

    void DragonSort()
    {
        GenerateDragon.Sort((x, y) =>
        {
            var Com1 = x.transform.GetChild(0).transform.position;
            var Com2 = y.transform.GetChild(0).transform.position;
            if (Com1.z > Com2.z)
            {
                return -1;
            }
            else if (Com1.z < Com2.z)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        });
    }

    /// <summary>
    /// 파일로 저장하는 코드
    /// </summary>
    /// <param name="gen"></param>
    void Save(int gen)
    {
        StreamWriter sw = new StreamWriter(@"data\" + (gen - 1) + ".txt");

        List<Dragon> Data = GenerateDragon;

        for (int i = 0; i < Data.Count; i++)
        {
            sw.WriteLine($"{i + 1} Genetic : {Data[i].transform.GetChild(0).transform.position.z} Moved");
            for (int w = 0; w < Data[i].Genetic.Count; w++)
            {
                sw.Write($"{w + 1} : ");
                for (int k = 0; k < Data[i].Genetic[w].Count; k++)
                {
                    sw.Write($"{Data[i].Genetic[w][k]}, ");
                }
                sw.WriteLine();
            }
            sw.WriteLine();

        }
        sw.Close();
    }

    /// <summary>
    /// 드라군의 생존비율에 맞게끔 수정
    /// </summary>
    void SurviveDragon()
    {
        LivedDragon.Clear();

        for (int i = 0; i < (int)(GenerateDragon.Count * LiveRatio); i++)
        {
            LivedDragon.Add(GenerateDragon[i]);
        }
        for (int i = 0; i < GenerateDragon.Count; i++)
        {
            Destroy(GenerateDragon[i].gameObject);
        }
        GenerateDragon.Clear();
    }

    /// <summary>
    /// 드라군 생성
    /// </summary>
    void CrossDragon()
    {
        GenerateDragon.Clear();

        for (int k = 0; k < CreateDragon * LiveRatio; k++)
        {
            float w = (1.0f / LiveRatio);
            for (int i = 0; i < w; i++)
            {
                float Position = k * 10 * w + i * 10;
                var data = Instantiate(
                            Dragon,
                            new Vector3(Position, 1.3f, 0.0f),
                            Quaternion.Euler(0f, 45f, 0f)).GetComponent<Dragon>();
                if (LivedDragon.Count == 0)
                {
                    data.CreateGenetic();
                }
                else
                {
                    if (i == 0)
                    {
                        data.CreateGenetic(0, LivedDragon[k].Genetic);
                    }
                    else
                    {
                        data.CreateGenetic(Transition, LivedDragon[k].Genetic);
                    }
                    if (DoYouWantWatchToBestSolusion)
                    {
                        data.CreateGenetic(0, FarTest);
                    }
                }
                GenerateDragon.Add(data);
            }
        }
    }


}