using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static bool annotate = false;
    public static Material team1Mat = Resources.Load("NormalSphere", typeof(Material)) as Material;
    public static Material team2Mat = Resources.Load("NormalSphere", typeof(Material)) as Material;
    public static Material selectedMat = Resources.Load("SelectedSphere", typeof(Material)) as Material;
}
