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
    public static int fieldLowBoundX = -69; // Used to determine where textbox should stop, to avoid it going ot of view
    public static int fieldHighBoundX = 83; // Used to determine where textbox should stop, to avoid it going ot of view
    public static float cameraDefaultX = 7;
    public static float cameraDefaultY = 353;
    public static float cameraDefaultZ = -507;
    public static float cameraRotX = 30.6f;
    public static float cameraRotY = 0;
    public static float cameraRotZ = 0;

    public static int rewindFactor = 100;

    public static float sphereElevation = 95;

    public static float runOnX = -400;
    public static float runOnZ = 0;

    public static float UIRunThresholdX = 60;  // Determines how fast a player must move in order for the details UI to activate
    public static float UIRunThresholdZ = 60;

}
