using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Valve.VR;

public class main : MonoBehaviour
{
    public GameObject m_left;
    public GameObject m_right;
    public Text m_text;

    private float m_lastTime;
    private float m_delay;
    private float m_elapsed;

    private Vector3 m_v3BestSignal;
    private Vector3 m_delta;

    private Vector3 m_v3Target;

    private VideoPlayer m_video;

    private VideoPlayer videoPlayer;

    public Renderer noise;

    static public float angleBetweenTwoVectors = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_lastTime = Time.time;
        m_delay = 30.0f;

        m_v3Target = new Vector3(0.5f, 0.5f, 0.5f);
        m_v3BestSignal  = new Vector3(0, 0, 0);
        m_delta = m_v3Target - m_v3BestSignal;
        m_delta = m_delta / m_delay;

        Debug.Log("Starting delta " + m_delta.x.ToString());


        // Video player

        //videoPlayer.targetTexture.Release();
        videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();
        //videoPlayer.url = "https://mixer.com/embed/player/GlobalGameJam?disableLowLatency=1";

        // Restart from beginning when done.
        videoPlayer.isLooping = true;

        videoPlayer.Play ();
    }

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Device: " + UnityEngine.XR.XRSettings.loadedDeviceName);

        GameObject ControllerLeft = GameObject.Find("controllerLeft");

        //Vector3 position = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.CenterEye);

        Vector3 leftPosition = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.LeftHand);
        // Debug.Log("Position left " + position.x.ToString());

        Vector3 rightPosition = UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightHand);
        // Debug.Log("Position left " + position.x.ToString());

        Quaternion leftRotation = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.LeftHand);
        Quaternion rightRotation = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.RightHand);

        // I've assigned debug cubes to copy the VR controller position and rotation
        m_left.transform.position = leftPosition;
        m_left.transform.rotation = leftRotation;

        m_right.transform.position = rightPosition;
        m_right.transform.rotation = rightRotation;

        // Code to randomise the direction of the signal every so often
        Vector3 v3CurrentAerial = new Vector3(0, 0, 1);

        v3CurrentAerial = m_left.transform.rotation * v3CurrentAerial;

        // How long have we been seeking our goal
        m_elapsed = Time.time - m_lastTime;

        if (m_elapsed > m_delay)
        {
            // Reset the timer
            m_lastTime = Time.time;

            m_v3Target = new Vector3(Random.Range(-1.0f, 1.0f),
                                      Random.Range(-1.0f, 1.0f),
                                      Random.Range(-1.0f, 1.0f));

            // Elapsed goes between 0..n
            // Lets calculate the new delta
            m_delta = m_v3Target - m_v3BestSignal;
            m_delta = m_delta / m_delay;
        }
        else
        {
            // Change the goalpost
            m_v3BestSignal = m_v3BestSignal + (m_delta * Time.deltaTime);
        }

        angleBetweenTwoVectors = Vector3.Angle(m_v3BestSignal, v3CurrentAerial);

        m_text.text = "";

        //m_text.text = "Current best signal: " + m_v3BestSignal.x.ToString("#.00") + "," + m_v3BestSignal.y.ToString("#.00") + "," + m_v3BestSignal.z.ToString("#.00");
        //m_text.text += "\nFuture best signal:  " + m_v3Target.x.ToString("#.00") + "," + m_v3Target.y.ToString("#.00") + "," + m_v3Target.z.ToString("#.00");
        //m_text.text += "\nCount down " + m_elapsed.ToString("#.00");
        m_text.text += "\nAerial " + v3CurrentAerial.x.ToString("#.00") + "," + v3CurrentAerial.y.ToString("#.00") + "," + v3CurrentAerial.z.ToString("#.00");
        m_text.text += "\nAngle " + angleBetweenTwoVectors.ToString("#.00");
        //m_text.text += "\nAngle " + noise.ToString("#.00");
    }
}
