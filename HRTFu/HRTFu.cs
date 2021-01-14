using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Eigen_HRTF_plugin;
using System.Text;
using System.IO;
using UnityEngine.Scripting;
using Unity.Audio;
using Unity.Jobs;
using UnityEngine.Profiling;

public class HRTFu : MonoBehaviour
{
    private float[] filter_l;
    private float[] filter_r;
    private float[] buffer_l;
    private float[] buffer_r;
    private float[] extra_buffer;
    private float[] buffer_signal;
    private bool first = true;
    private int[] delays;
    private int[] prev_delays;
    private int[] idxs;
    private float distance = 20;
    private float elevation = 0;
    private float azimuth = 0;
    public float scale = 0.01f;
    public enum Pinnas : int { Small = 0, Large = 1 };
    public Pinnas pinna;
    public GameObject listener = null;
    private AudioSource audio_source;
    private bool _isPlaying = false;    
    public List<float> samples;    

    void Awake()
    {
        filter_l = new float[257*2];
        filter_r = new float[257*2];        
        buffer_l = new float[1024];
        buffer_r = new float[1024];
        extra_buffer = new float[128];
        buffer_signal = new float[256];
        delays = new int[2];
        prev_delays = new int[2];
        idxs = new int[2];
        idxs[0] = -2;
        idxs[1] = -2;
        delays[0] = 0;
        delays[1] = 0;
        prev_delays[0] = 0;
        prev_delays[1] = 0;

        sampler = CustomSampler.Create("Extra");
        sampler2 = CustomSampler.Create("Audio_processing");

        AudioConfiguration config = AudioSettings.GetConfiguration();
        CheckAudioSource();
        if (listener == null)
        {
            //Seek audio listeners in scene
            AudioListener[] listeners = UnityEngine.Object.FindObjectsOfType<AudioListener>();
            if (listeners.Length == 0)
            {
                //The sound doesn't make sense without no one to hear it
                Debug.LogWarning("No Listner founds in this scene.");
            }
            else
            {
                //Set a listener
                listener = listeners[0].gameObject;
            }
        }
        Eigen_HRTF.eigen_init();        
        _isPlaying = audio_source.isPlaying;        
        samples = new List<float>();
    }

    // Update is called once per frame
    void LateUpdate()
    {        
        float tmp_d = distance;
        float tmp_e = elevation;
        float tmp_a = azimuth;
        //Calculate distance between listener and sound source	
        distance = Mathf.Abs(Vector3.Distance(transform.position, listener.transform.position)) / scale;
        //Calculate diretion vector between listener and sound source	
        Vector3 dir = (transform.position - listener.transform.position).normalized;        
        //Calculate angle of elevation between listener and sound source        
        if (Vector3.Cross(listener.transform.right, Vector3.ProjectOnPlane(dir, listener.transform.up)) == Vector3.zero)
        {
            Vector3 dirE = Vector3.ProjectOnPlane(dir, listener.transform.forward);
            elevation = Vector3.SignedAngle(listener.transform.right, dirE, listener.transform.forward);
        }        
        else
        {
            Vector3 dirE = Vector3.ProjectOnPlane(dir, listener.transform.right);
            elevation = -Vector3.SignedAngle(listener.transform.forward, dirE, listener.transform.right);
        }
        elevation = elevation % 180 == 0 ? 0 : elevation;        
        if (elevation < -90f)
        {
            elevation = -90 - (elevation % 90);
        }
        if (elevation > 90f)
        {
            elevation = 90 - (elevation % 90);
        }
        //Calculate angle of azimuth between listener and sound source
        Vector3 dirA = Vector3.ProjectOnPlane(dir, listener.transform.up);
        azimuth = Vector3.SignedAngle(listener.transform.forward, dirA, listener.transform.up);
        if (azimuth < 0f)
        {
            azimuth = 360f + azimuth;
        }        
        _isPlaying = audio_source.isPlaying;
        if (tmp_d == distance && tmp_e == elevation && tmp_a == azimuth) { sampler.End(); return; }        
        Eigen_HRTF.get_filters(distance, elevation, azimuth, (int)pinna, filter_l, filter_r, delays, idxs);

    }

    private void CheckAudioSource()
    {
        if (this.GetComponent<AudioSource>() == null)
            gameObject.AddComponent<AudioSource>();
        audio_source = this.GetComponent<AudioSource>();
    }
    void OnAudioFilterRead(float[] data, int channels)
    {        
        unsafe
        {
            if (_isPlaying)                
                Eigen_HRTF.DSP(data, data.Length, filter_l, filter_r, buffer_l, buffer_r, prev_delays, delays);
        }                
    }    

}