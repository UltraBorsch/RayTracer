# RayTracer
A fairly simply ray tracer using C#/HLSL/Unity.

Inspiration taken from Sebastian Lague's and Acerola's videos regarding the same topic, and to an extent a similarly themed class assignment.\
Although, I add my own twists, with the end goal for this to be a standalone creation.

# IMPLEMENTED
* Spheres, Planes, Axis-Aligned Bounding Boxes, Quadrics (kinda).
* Rotated grid super sampling anti-aliasing.
* Shadows.
* Basic light attenuation.

# TODO
* General refactoring is much needed.
* More standard shapes (implicits, bezier surface patches, etc).
* More complex shapes (meshes, hierarchies, constructive geometry, etc).
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
   * Potential visual bug with AABBs. Certain faces sometimes stop rendering at certain angles. I have noticed it specifically with the faces perpendicular to the y-axis, but does not happen always.
   * While not tested, im quite sure that there are bugs when using odd sample values, decimal sample values, and sample values >= 12. Realistically, any value > 8 will have increasing performance
     penalties at drastically reduced benefits.
