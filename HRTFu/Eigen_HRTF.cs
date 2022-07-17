using System;
using System.Runtime.InteropServices;

namespace Eigen_HRTF_plugin {
    //[InitializeOnLoad]
    static class Eigen_HRTF
    {
        [DllImport("HRTFu_plugin", EntryPoint = "get_filters", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void get_filters( float d,  float e,  float a,  int pinna,  float[] filter_l,  float[] filter_r,  int[] delays,  int[] idxs);        

        [DllImport("HRTFu_plugin", EntryPoint = "eigen_init", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void eigen_init([In] [MarshalAs(UnmanagedType.LPStr)] string path);        

        [DllImport("HRTFu_plugin", EntryPoint = "full_DSP", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public unsafe static extern void full_DSP(float[] data, int data_size, float[] signal_buffer, float[] filter_l, float[] filter_r, float[] buffer_l, float[] buffer_r, int[] delays, float gain);

        [DllImport("HRTFu_plugin", EntryPoint = "DSP", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public unsafe static extern void DSP(float[] data, int data_size, float[] filter_l, float[] filter_r, float[] filter_l2, float[] filter_r2, float[] buffer_l, float[] buffer_r, int[] prev_delays, int[] delays, int[] prev_ids, int[] ids, float gain);

    }
}
