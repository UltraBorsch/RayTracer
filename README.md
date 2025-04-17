# RayTracer
A fairly simply ray tracer using C#/HLSL/Unity.

![scene](https://github.com/user-attachments/assets/11a60649-36c3-444a-9550-43a962baf8e3)

Inspiration taken from Sebastian Lague's and Acerola's videos regarding the same topic, and to an extent a similarly themed class assignment.\
Although, I add my own twists, with the end goal for this to be a standalone creation.

Part of the challenge is to avoid using the built in ray-tracing features provided by the Unity engine.

# IMPLEMENTED
* Spheres, Planes, Axis-Aligned Bounding Boxes, Quadrics (kinda), Meshes whose values exist and are well formatted (norms, tris, verts, etc).
* Rotated grid super sampling anti-aliasing.
* Shadows.
* Basic light attenuation.

# TODO
* General refactoring is much needed.
* More standard shapes (implicits, bezier surface patches, metabals, etc).
* More complex shapes (oriented bounding boxes, hierarchies, constructive geometry, etc).
    * Asserting that said shapes conform, e.g. generating normals when meshes do not provide them, no trailing vertices, etc.
* Transformation matrices for applicable geometry types (notably not spheres and planes).
* Bounds for certain geometry (could possibly use for all geometry types, but in particular for planes and quadrics so that they're not infinite).
* Additional features:
    * More complex/efficient light attenuation;
    * More Anti-Aliasing options (jittering, post-processing options, MSAA, etc).
    * Mirror/fresnel reflection.
    * Refraction.
    * Motion blur.
    * Depth of field blur.
    * Area lights + path tracing.
    * Shading options (e.g. Gourand, toon, pixelated, etc).
* Non ray-tracer features
    * Environment maps (cube/sphere).
    * Texture maps (additionally, adaptive sampling/mipmaps).
    * Perlin/simplex/procedual noise/textures.
    * Allow settings to be modifiable while the simulation is running.
    * Simpler means of setting variables/setting scenes. E.g. attenuation coefficients are awkwardly small and difficult to adjust. 
* Optimization
    * Hierarchical Bounding volumes for complex meshes.
    * Spatial hashing/ray marching.
    * Culling, probably by giving every (complex) geometry a bounding box.
    * Precomputing certain values when possible/worthwhile.
# Known Issues
   * Bugs with some quadrics. Notably, elliptic cones clearly work but ellipsoids do not look like ellipsoids.
   * While not tested, im quite sure that there are bugs when using odd sample values, decimal sample values, and sample values >= 12. Realistically, any value > 8 will have increasing performance
     penalties at drastically reduced benefits.
   * Performance is not-ideal with meshes. Will be working on improving it.
