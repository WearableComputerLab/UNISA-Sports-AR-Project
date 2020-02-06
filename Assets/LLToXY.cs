using UnityEngine;
using System.Collections;

public class LLToXY
{
    /* aami stadium */

    // size of x/y plane
    private const int SCALE = 8;
    private const int ORIG_WIDTH = 142;
    private const int ORIG_HEIGHT = 182;
    private const int WIDTH = ORIG_WIDTH * SCALE;
    private const int HEIGHT = ORIG_HEIGHT * SCALE;

    // top left lat/long
    private const double TOP_LEFT_LAT = -34.879145d;
    private const double TOP_LEFT_LONG = 138.494660d;

    // bottom right lat/long
    private const double BOT_RIGHT_LAT = -34.880697d;
    private const double BOT_RIGHT_LONG = 138.496310d;

    // diff of ^
    private const double DIFF_LAT = TOP_LEFT_LAT - BOT_RIGHT_LAT;
    private const double DIFF_LONG = BOT_RIGHT_LONG - TOP_LEFT_LONG;

    /* mawson lakes

	private const int SCALE_ALT = 8;
	private const int ORIG_WIDTH_ALT = 51;
	private const int ORIG_HEIGHT_ALT = 46;
	private const int WIDTH_ALT = ORIG_WIDTH_ALT * SCALE_ALT;
	private const int HEIGHT_ALT = ORIG_HEIGHT_ALT * SCALE_ALT;

	private const double TOP_LEFT_LAT_ALT = -34.810307d;
	private const double TOP_LEFT_LONG_ALT = 138.620453d;

	private const double BOT_RIGHT_LAT_ALT = -34.810711d;
	private const double BOT_RIGHT_LONG_ALT = 138.621031d;

	private const double DIFF_LAT_ALT = TOP_LEFT_LAT_ALT - BOT_RIGHT_LAT_ALT;
	private const double DIFF_LONG_ALT = BOT_RIGHT_LONG_ALT - TOP_LEFT_LONG_ALT;*/

    /* house */

    private const int SCALE_ALT = 8;
    private const int ORIG_WIDTH_ALT = 14;
    private const int ORIG_HEIGHT_ALT = 11;
    private const int WIDTH_ALT = ORIG_WIDTH_ALT * SCALE_ALT;
    private const int HEIGHT_ALT = ORIG_HEIGHT_ALT * SCALE_ALT;

    private const double TOP_LEFT_LAT_ALT = -34.920033d;
    private const double TOP_LEFT_LONG_ALT = 138.497629d;

    private const double BOT_RIGHT_LAT_ALT = -34.920130d;
    private const double BOT_RIGHT_LONG_ALT = 138.497765d;

    private const double DIFF_LAT_ALT = TOP_LEFT_LAT_ALT - BOT_RIGHT_LAT_ALT;
    private const double DIFF_LONG_ALT = BOT_RIGHT_LONG_ALT - TOP_LEFT_LONG_ALT;

    public static double LongToX(double lon)
    {
        return ((BOT_RIGHT_LONG - lon) / DIFF_LONG) * WIDTH;
    }

    public static double LatToY(double lat)
    {
        return ((TOP_LEFT_LAT - lat) / DIFF_LAT) * HEIGHT;
    }

    public static int LongToXAlt(double lon)
    {
        return System.Convert.ToInt32(((BOT_RIGHT_LONG_ALT - lon) / DIFF_LONG_ALT) * WIDTH_ALT);
    }

    public static int LatToYAlt(double lat)
    {
        return System.Convert.ToInt32(((TOP_LEFT_LAT_ALT - lat) / DIFF_LAT_ALT) * HEIGHT_ALT);
    }

    public static Vector3 LongLatTOXZ(double Longitute, double Latitude, float FieldSizeX, float FieldSizeZ)
    {
        Debug.Log("Longditude: " + Longitute);
    //    double xDifference = (TOP_LEFT_LONG - Longitute ) * (BOT_RIGHT_LONG - TOP_LEFT_LONG);
      //  Debug.Log("xDifference: " + xDifference);
        double xScaled = Longitute * (DIFF_LONG);
        //double xScaled = LongToX(Longditute) / 100000000f;
        Debug.Log("XScaled: " + xScaled);
        float newX = ((float)xScaled) * FieldSizeX;

        Debug.Log("Latitude: " + Latitude);
        double yScaled = Latitude * (DIFF_LAT); 
        //double yScaled = LatToY(Latitude) / 100000000f;
        Debug.Log("YScaled: " + yScaled);
        float newZ = ((float)yScaled) * FieldSizeZ;

        Debug.Log("X: " + newX + ", Y: " + newZ);
        return new Vector3(newX, 0.78f, newZ);
    }
}
