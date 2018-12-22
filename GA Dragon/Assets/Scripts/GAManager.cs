﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using GeneticType = System.Collections.Generic.List<System.Collections.Generic.List<float>>;
public class GAManager : MonoBehaviour
{
    public GameObject Dragon;

    List<Dragon> GenerateDragon;
    List<GeneticType> LivedDragon;

    public int CreateDragon = 20;
    public int Gen = 0;
    public float LiveRatio = 0.25f;
    public float Transition = 0.2f;
    public float NextGenTime = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        GenerateDragon = new List<Dragon>();
        LivedDragon = new List<GeneticType>();
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        while (true)
        {
            NextGenerate();
            yield return new WaitForSeconds(NextGenTime);
        }
    }

    void NextGenerate()
    {
        Gen++;
        if (GenerateDragon.Count != 0)
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
            Debug.Log($"이전 세대 최고 치 : {Gen - 1}\n{GenerateDragon[0].transform.GetChild(0).transform.position.z}");
            SurviveDragon();
        }
        CrossDragon();
    }

    void Save(int gen)
    {
        StreamWriter sw = new StreamWriter(@"C:\Users\aoika\Desktop\git\Genetic-Algorithm\data\" + gen + ".txt");

        for (int i = 0; i < LivedDragon.Capacity; i++)
        {
            sw.Write($"{i + 1} : ");
            for (int k = 0; k < LivedDragon[i].Count - 1; k++)
            {
                sw.Write($"{LivedDragon[i][k]}, ");
            }
            sw.WriteLine($"{LivedDragon[i][LivedDragon[i].Count - 1]}");
        }
        sw.Close();
    }

    void SurviveDragon()
    {
        for (int i = 0; i < (int)(GenerateDragon.Count * LiveRatio); i++)
        {
            LivedDragon.Add(GenerateDragon[i].Genetic);
        }
        for (int i = 0; i < GenerateDragon.Count; i++)
        {
            Destroy(GenerateDragon[i].gameObject);
        }
        
    }

    void CrossDragon()
    {
        GenerateDragon.Clear();
        for (int k = 0; k < CreateDragon; k++)
        {
            float Position = k * 10;
            var data = Instantiate(
                        Dragon,
                        new Vector3(Position, 1.3f, 0.0f),
                        Quaternion.Euler(0f,45f,0f)).GetComponent<Dragon>();
            if (LivedDragon.Count == 0)
            {
                data.CreateGenetic();
            }
            else
            {
                data.CreateGenetic(Transition, LivedDragon[(int)(k / (1.0f / LiveRatio))]);
            }

            GenerateDragon.Add(data);
        }
        LivedDragon.Clear();
    }
        
}
