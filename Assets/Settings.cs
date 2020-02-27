using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static bool annotate;
    public static Material team1Mat;
    public static Material team2Mat;
    public static Material selectedMat;
    private void Start()
    {
        annotate = true;
        team1Mat = Resources.Load("NormalSphere", typeof(Material)) as Material;
        team2Mat = Resources.Load("NormalSphere", typeof(Material)) as Material;
        selectedMat = Resources.Load("SelectedSphere", typeof(Material)) as Material;
    }
}
