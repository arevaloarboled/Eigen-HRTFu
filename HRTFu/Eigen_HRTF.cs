using System;
using System.Runtime.InteropServices;

namespace Eigen_HRTF_plugin {
    //[InitializeOnLoad]
    static class Eigen_HRTF
    {
        [DllImport("HRTFu_plugin", EntryPoint = "get_filters", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void get_filters( float d,  float e,  float a,  int pinna,  float[] filter_l,  float[] filter_r,  int[] delays,  int[] idxs);

        //[DllImport("HRTFu_plugin", EntryPoint = "get_filters", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        //public static extern unsafe void get_filters(float d, float e, float a, int pinna, float* filter_l, float* filter_r, int* delays, int* idxs);

        //[DllImport("HRTFu_plugin", EntryPoint = "convolution", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        //public static extern void convolution([Out] float[] signal, [In] float[] filter);

        //[DllImport("HRTFu_plugin", EntryPoint = "convolution", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        //public static extern unsafe void convolution( float* signal, float* filter);

        [DllImport("HRTFu_plugin", EntryPoint = "eigen_init", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void eigen_init();

        [DllImport("HRTFu_plugin", EntryPoint = "DSP", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public unsafe static extern void DSP(float[] data, int data_size, float[] filter_l, float[] filter_r, float[] buffer_l, float[] buffer_r, int[] prev_delays, int[] delays);

        //[DllImport("HRTFu_plugin", EntryPoint = "DSP", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        //public unsafe static extern void DSP(float[] data, int data_size, float[] filter_l, float[] filter_r, float[] buffer_l, float[] buffer_r, int[] prev_delays, int[] delays, float[] extra_buffer);

        //[DllImport("HRTFu_plugin", EntryPoint = "DSP", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        //public static extern unsafe void DSP( float[] data, int data_size, float[] filter_l, float[] filter_r,  float[] buffer_l,  float[] buffer_r,  int[] prev_delays,  int[] delays);

        //[DllImport("HRTFu_plugin", EntryPoint = "DSP", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        //public static extern unsafe void DSP(float* data, int data_size, float* filter_l, float* filter_r, float* buffer_l, float* buffer_r, int* prev_delays, int* delays);

        //[DllImport("HRTFu_plugin", EntryPoint = "DSP_delay", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        //public static extern void DSP_delay([Out] float[] data,[In] int data_size,[In] bool first, [In] float[] filter_l, [In] float[] filter_r, [Out] float[] buffer_signal, [Out] float[] buffer_l, [Out] float[] buffer_r, [Out] int[] delays);
        //float* data,int data_size,bool first,float* filter_l,float* filter_r,float* buffer_signal,float* buffer_l,float* buffer_r,int* delays
    }
}
