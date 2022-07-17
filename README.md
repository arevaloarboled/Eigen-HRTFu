<p  align="center">  <img  src="https://alguien14.github.io/HRTFu.png"  width="100"/>  </p>
<p  align="center">
  <a  href="https://unity3d.com/es/unity"  target="_blank">
    <img  src="https://img.shields.io/badge/Unity-2020.1.6-blue.svg"  alt="Unity 2020.1.6">
  </a>
  <a  href="#"  target="_blank">
    <img  src="https://img.shields.io/badge/Plugin-C-green.svg"  alt="C">
  </a>
  <a  href="http://www.fftw.org/"  target="_blank">
    <img  src="https://img.shields.io/badge/Library-FFTW-red.svg"  alt="FFTW">
  </a>
</p>

Eigen-HRTFu is an audio spatializer for a near field of the listener in Unity engine focused for VR, is capable of spatialization between distance 20 ⩽ d ⩽ 160 cm, azimuth 0° ⩽ θ < 360°, and elevation -40° ⩽ ϕ ⩽ 90° base on an Eigen decomposition of the database created by Qu et al. in [1]. Methods used to create this database is described here [2].

# Getting start

This API is tested in Unity 2020.1.6, to get it, just copy the HRTFu folder in the `Assets` folder.

Currently, this spatializer only works for MacOS in 64 bits architecture. **Windows coming soon!**

>If you are using Plugins in your project, mind that Eigen-HRTFu has its own Plugins directory. You will need to move the Plugin of HRTFu in your own Plugins directory, or move your Plugins in the HRTFu Plugin directory.

To work with this spatializer, we recommend to set up this Audio setting in Unity (`Edit > Project Settings > Audio`):

* Default speaker mode as **Stereo**
* DSP Buffer Size as **Best latency** (i.e., 256 buffer size)

# Usage

To spatialize an `Audio Source` with this spatializer you need to:
* Attach the `HRTFu` script to an object that has an `Audio Source` property in Unity.
* Disable any spatialization option in the `Audio Source` options (i.e., `Spatial Bend` and `Spatialize` options).

Here is a description of the properties you can edit for this spatializer:

### Properties

|Propiertie |Description
|-------------------------------|-----------------------------|
|Scale | Is a scale of the distance from the listener object to the audio source, according to the units used to define the geometry of objects. By default is `.01`|
|Pinna | This spatializer have 2 pinnae simulators included, you can select `Large` or `Small` pinnae simulator. By default is `Small`.|
|Listener | This property specifies the listener in the scene, by default is the `Audio Listener` in the scene.|

# References
  [1] T.  Qu,  Z.  Xiao,  M.  Gong,  Y.  Huang,  X.  Li,  and  X.  Wu,  “Distance-Dependent Head-Related Transfer Functions Measured With High Spa-tial  Resolution  Using  a  Spark  Gap,”IEEE Trans. on Audio, Speech &Language Processing, vol. 17, no. 6, pp. 1124–1132, 2009.\
  [2] Arevalo, Camilo & Villegas, Julián. (2020). Compressing Head-Related Transfer Function databases by Eigen decomposition. 1-6. 10.1109/MMSP48831.2020.9287134. 
