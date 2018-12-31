using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using GeneticType = System.Collections.Generic.List<System.Collections.Generic.List<int>>;
public class GAManager : MonoBehaviour
{
    public GameObject Dragon;
    public GameObject Swing;

    List<Sheet> GenerateDragon;
    List<Sheet> LivedDragon;

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

        GenerateDragon = new List<Sheet>();
        LivedDragon = new List<Sheet>();
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
            Debug.Log($"이전 세대 최고 치 : {Gen - 1}\n{GenerateDragon[0].human.MaxSpeed}");
            Save(Gen);
            SurviveDragon();
            
        }
        CrossDragon();
    }

    void DragonSort()
    {
        GenerateDragon.Sort((x, y) =>
        {
            var Com1 = x.human.MaxSpeed;
            var Com2 = y.human.MaxSpeed;
            if (Com1 > Com2)
            {
                return -1;
            }
            else if (Com1 < Com2)
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

        var Data = GenerateDragon;

        for (int i = 0; i < Data.Count; i++)
        {
            sw.WriteLine($"{i + 1} Genetic : {Data[i].human.MaxSpeed} Speed");
            for (int w = 0; w < Data[i].human.Genetic.Count; w++)
            {
                sw.Write($"{w + 1} : ");
                for (int k = 0; k < Data[i].human.Genetic[w].Count; k++)
                {
                    sw.Write($"{Data[i].human.Genetic[w][k]}, ");
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
            Destroy(GenerateDragon[i].human.gameObject);
            Destroy(GenerateDragon[i].swing.gameObject);
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
                var swing = Instantiate(Swing, new Vector3(Position, 2.5f, 0), Quaternion.identity);
                var data = Instantiate(
                            Dragon,
                            new Vector3(Position, 4.3f, 0.0f),
                            Quaternion.identity).GetComponent<Human>();
                if (LivedDragon.Count == 0)
                {
                    data.CreateGenetic();
                }
                else
                {
                    if (i == 0)
                    {
                        data.CreateGenetic(0, LivedDragon[k].human.Genetic);
                    }
                    else
                    {
                        data.CreateGenetic(Transition, LivedDragon[k].human.Genetic);
                    }
                    if (DoYouWantWatchToBestSolusion)
                    {
                        data.CreateGenetic(0, FarTest);
                    }
                }
                GenerateDragon.Add(new Sheet {human = data, swing = swing });
            }
        }
    }


}