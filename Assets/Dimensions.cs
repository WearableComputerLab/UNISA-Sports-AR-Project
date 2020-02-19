using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dimensions : MonoBehaviour
{
   
    // Values related to the csv data
    public static double centrePointLong = 138.495485;
    public static double centrePointLat = 34.879921;

    /*
     * Probably need to change some or all of the following, if changing size of virtual stadium
     */
    // Values for simulation
    public static int scaleFactor = 400000;
    public static int textBoxLowBoundX = -69;
    public static int textBoxHighBoundX = 83;
    public static float cameraDefaultX = 7;
    public static float cameraDefaultY = 255;
    public static float cameraDefaultZ = -445;
    public static int rewindFactor = 100;

    public static float sphereElevation = 19;


}
