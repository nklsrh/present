using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMUtils
{
    public static bool isEditingGameplay = false;
    public static bool autoPlayWithAI = false;
    public bool testAutoPlayWithAI = false;
    public bool bowlInsteadOfBat = false;

    public int opponentOverall = 0;

    const string DEBUG_MODE_STRING = "DIBIGMIDE";
    public static bool isDebugMode
    {
        get
        {
            return PlayerPrefs.GetInt(DEBUG_MODE_STRING, 0) != 0;
        }
        set
        {
            PlayerPrefs.SetInt(DEBUG_MODE_STRING, value ? 1 : 0);
        }
    }

    public const long SECONDS_PER_HOUR = 3600;
    public const long HOURS_PER_QUICKMATCH = 24;
    public const long TICKS_PER_SECOND = 10000000;

    void Awake()
    {
        if (DMUtils.isDebugMode && fpsCounter)
        {
            fpsCounter.gameObject.SetActive(false);
        }

        Time.timeScale = 1;
    }

    public UnityEngine.UI.Text fpsCounter;

    public void GoToScene(string s)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(s);
    }

    public void GoToScene(int i)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(i);
    }

    public void LoadLevelMatchi()
    {
        DMUtils.LoadLevelMatch();
    }
    public void LoadLevelLobbyi()
    {
        DMUtils.LoadLevelLobby();
    }
    public void LoadLevelSquadi()
    {
        DMUtils.LoadLevelSquad();
    }

    internal static void LoadLevelMatch()
    {
        Application.targetFrameRate = -1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("match");
    }
    internal static void LoadLevelLobby()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("lobby");
    }
    internal static void LoadLevelSquad()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("squad");
    }
    internal static void LoadLevelArtStyle()
    {
        Application.targetFrameRate = -1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("artstyle");
    }

    internal static string StringArray(object[] arrayString)
    {
        string final = "";
        for (int i = 0; i < arrayString.Length; i++)
        {
            final += arrayString[i] + (i == arrayString.Length - 1 ? "" : ", ");
        }
        return final;
    }

    internal static void LoadLevelShoes()
    {
        Application.targetFrameRate = -1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("shoe-test");
    }


    public void ToggleDebugMode()
    {
        DMUtils.ToggleDebug();
    }
    public static void ToggleDebug()
    {
        isDebugMode = !isDebugMode;
        //MenuSystemController.Instance.Question("DEBUG MODE", "DEBUG MODE IS NOW : " + isDebugMode, null, null, true);
        //MenuSystemController.Instance.topBar.Refresh();
    }

    public void DebugGimmePack()
    {
        //MenuSystemController.Instance.AwardPack(SCData.Pack.Random(), null, true);
    }

    public static void SetTimeMultiplierOverride(float value)
    {
        //TimeManager.SetTimeMultiplierUserMultiplier(value);
    }

    void Update()
    {
        //TimeManager.SetTargetDelta(Time.deltaTime);

        if (DMUtils.isDebugMode)
        {
            UpdateFPS(Time.deltaTime);
            if (fpsCounter != null)
            {
                SetFPSCounter();
            }
        }

#if UNITY_EDITOR
        DebugInputs();
#endif
    }

    private void DebugInputs()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
        }
    }

    public void SetFPSCounter()
    {
        fpsCounter.text = fpsToFloatString;
    }


    public static float FPS { get; private set; }
    public static string fpsToIntString { get { return string.Format("{0:F0} FPS", FPS); } }
    public static string fpsToFloatString { get { return string.Format("{0:F2} FPS", FPS); } }

    private static float updateInterval = 0.5f;

    private static float accum = 0;
    private static int frames = 0;
    private static float timeleft;

    private static float averageFrameValue;
    private static int counter = 0;

    public void UpdateFPS(float deltaTime)
    {
        timeleft -= deltaTime;
        accum += Time.timeScale / deltaTime;
        ++frames;
        averageFrameValue += deltaTime;
        counter++;

        if (timeleft <= 0.0f)
        {
            FPS = accum / frames;
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;

            averageFrameValue = 0f;
            counter = 0;
        }
    }

    public static float averageFrameTime
    {
        get { return averageFrameValue / counter; }
    }


    // match,  ball, data


    public static string ConvertOvers(int balls)
    {
        return Mathf.FloorToInt(balls / 6) + "." + Mathf.Max((balls) % 6, 0);
    }

    public static string CreateScoreFromData(int runs, int wickets)
    {
        return runs + "-" + wickets;
    }


    // general stuff

    public static bool ArrayContains(int[] array, int val)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == val)
            {
                return true;
            }
        }
        return false;
    }

    #region Transforms and UI

    public static void BuildList<T, R>(System.Action<T, R> actions, R[] objects, GameObject objPrototype, Transform parent)
    {
        int size = objects.Length;

        objPrototype.gameObject.SetActive(true);

        foreach (Transform t in parent)
        {
            T wt = t.GetComponent<T>();
            if (wt != null && t.gameObject != objPrototype)
            {
                GameObject.Destroy(t.gameObject);
            }
        }

        for (int i = 0; i < size; i++)
        {
            GameObject wt = GameObject.Instantiate(objPrototype);
            wt.transform.SetParent(parent, false);
            wt.transform.localScale = Vector3.one;
            wt.transform.localPosition = Vector3.zero;
            wt.transform.localRotation = Quaternion.identity;

            if (actions != null)
            {
                actions.Invoke(wt.GetComponent<T>(), objects[i]);
            }
        }

        objPrototype.gameObject.SetActive(false);
    }

    #endregion

    #region ColorUtilities

    // colors

    public static bool TryParseColor(string input, out Color color)
    {
        input = input.Trim();

        if (input.IndexOf("#") == 0)
        {
            if (input.Length == 7 || input.Length == 9)
            {
                string hex = input.Replace("#", "");
                byte b0;
                byte b1;
                byte b2;
                byte b3;

                if (byte.TryParse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, null as System.IFormatProvider, out b0))
                {
                    if (byte.TryParse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null as System.IFormatProvider, out b1))
                    {
                        if (byte.TryParse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, null as System.IFormatProvider, out b2))
                        {

                            if (input.Length == 9)
                            {
                                if (byte.TryParse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber, null as System.IFormatProvider, out b3))
                                {
                                    color = new Color((float)b1 / 255f, (float)b2 / 255f, (float)b3 / 255f, (float)b0 / 255f);
                                    return true;
                                }
                            }
                            else
                            {
                                color = new Color((float)b0 / 255f, (float)b1 / 255f, (float)b2 / 255f);
                                return true;
                            }
                        }
                    }
                }
            }
        }
        color = Color.black;
        return false;
    }


    public static class HSVUtil
    {

        public static HsvColor ConvertRgbToHsv(Color color)
        {
            return ConvertRgbToHsv((int)(color.r * 255), (int)(color.g * 255), (int)(color.b * 255));
        }

        //Converts an RGB color to an HSV color.
        public static HsvColor ConvertRgbToHsv(double r, double b, double g)
        {
            double delta, min;
            double h = 0, s, v;

            min = Math.Min(Math.Min(r, g), b);
            v = Math.Max(Math.Max(r, g), b);
            delta = v - min;

            if (v == 0.0)
                s = 0;
            else
                s = delta / v;

            if (s == 0)
                h = 360;
            else
            {
                if (r == v)
                    h = (g - b) / delta;
                else if (g == v)
                    h = 2 + (b - r) / delta;
                else if (b == v)
                    h = 4 + (r - g) / delta;

                h *= 60;
                if (h <= 0.0)
                    h += 360;
            }

            HsvColor hsvColor = new HsvColor();
            hsvColor.H = 360 - h;
            hsvColor.S = s;
            hsvColor.V = v / 255;

            return hsvColor;

        }

        // Converts an HSV color to an RGB color.
        public static Color ConvertHsvToRgb(double h, double s, double v, float alpha)
        {

            double r = 0, g = 0, b = 0;

            if (s == 0)
            {
                r = v;
                g = v;
                b = v;
            }

            else
            {
                int i;
                double f, p, q, t;


                if (h == 360)
                    h = 0;
                else
                    h = h / 60;

                i = (int)(h);
                f = h - i;

                p = v * (1.0 - s);
                q = v * (1.0 - (s * f));
                t = v * (1.0 - (s * (1.0f - f)));


                switch (i)
                {
                    case 0:
                        r = v;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = v;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = v;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = v;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = v;
                        break;

                    default:
                        r = v;
                        g = p;
                        b = q;
                        break;
                }

            }

            return new Color((float)r, (float)g, (float)b, alpha);

        }
    }


    // Describes a color in terms of
    // Hue, Saturation, and Value (brightness)

    #endregion ColorUtilities

    #region HsvColor
    public struct HsvColor
    {
        /// <summary>
        /// The Hue, ranges between 0 and 360
        /// </summary>
        public double H;

        /// <summary>
        /// The saturation, ranges between 0 and 1
        /// </summary>
        public double S;

        // The value (brightness), ranges between 0 and 1
        public double V;

        public float normalizedH
        {
            get
            {
                return (float)H / 360f;
            }

            set
            {
                H = (double)value * 360;
            }
        }

        public float normalizedS
        {
            get
            {
                return (float)S;
            }
            set
            {
                S = (double)value;
            }
        }

        public float normalizedV
        {
            get
            {
                return (float)V;
            }
            set
            {
                V = (double)value;
            }
        }

        public HsvColor(double h, double s, double v)
        {
            this.H = h;
            this.S = s;
            this.V = v;
        }

        public override string ToString()
        {
            return "{" + H.ToString("f2") + "," + S.ToString("f2") + "," + V.ToString("f2") + "}";
        }
    }
    #endregion HsvColor

}
