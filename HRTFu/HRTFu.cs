using System.Collections.Generic;
using UnityEngine;
using Eigen_HRTF_plugin;
using UnityEngine.Profiling;

public class HRTFu : MonoBehaviour
{
    private float[] filter_l;
    private float[] filter_r;
    private float[] filter_l2;
    private float[] filter_r2;
    private float[] buffer_l;
    private float[] buffer_r;
    private float[] signal_buffer;
    private int[] delays;
    private int[] prev_delays;
    private int[] idxs;
    private int[] prev_idxs;
    private float distance = 0;
    private float elevation = 0;
    private float azimuth = 0;
    [Tooltip("Define a 'centimeter' in the virtual envirioment.")]
    public float scale = 0.01f;
    public enum Pinnas : int { Small = 0, Large = 1 };
    public Pinnas pinna;
    public GameObject listener = null;
    [Tooltip("Gain in dB. Modification of the gain may change the subjective perception of distance. We strongly recommend do not modify this parameter. Use it at your own discretion. Default value 0 dB")]
    public float gain = 0f;    
    private AudioSource audio_source;
    private bool _isPlaying = false;
    private bool _isVirtual = false;    
    //Listener object    
    // Start is called before the first frame update  


    void Awake()
    {
        filter_l = new float[257*2];
        filter_r = new float[257*2];
        filter_l2 = new float[257 * 2];
        filter_r2 = new float[257 * 2];        
        buffer_l = new float[589];
        buffer_r = new float[589];
        signal_buffer = new float[128];
        delays = new int[2];
        prev_delays = new int[2];
        idxs = new int[2];
        prev_idxs = new int[2];
        idxs[0] = -2;
        idxs[1] = -2;
        prev_idxs[0] = -1;
        prev_idxs[1] = -1;
        delays[0] = 0;
        delays[1] = 0;
        prev_delays[0] = 0;
        prev_delays[1] = 0;        
       
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
        Eigen_HRTF.eigen_init(Application.streamingAssetsPath+"/HRTFu/");        
        _isPlaying = audio_source.isPlaying;        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _isPlaying = audio_source.isPlaying;
        _isVirtual = audio_source.isVirtual;        
        if (!_isPlaying) return;        
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
        if (tmp_d == distance && tmp_e == elevation && tmp_a == azimuth) {            
        return; }
        Eigen_HRTF.get_filters(distance, elevation, azimuth, (int)pinna, filter_l, filter_r, delays, idxs);                

    }
    public void reset_buffer()
    {
        buffer_l=new float[589];
        buffer_r = new float[589];
    }

    private void CheckAudioSource()
    {
        if (this.GetComponent<AudioSource>() == null)
            gameObject.AddComponent<AudioSource>();
        audio_source = this.GetComponent<AudioSource>();
    }
    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!_isPlaying) return;                
        Eigen_HRTF.DSP(data, data.Length, filter_l, filter_r, filter_l2, filter_r2, buffer_l, buffer_r, prev_delays, delays, prev_idxs, idxs, gain);           
    }  

}
