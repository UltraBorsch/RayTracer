# RayTracer
Inspiration taken from Sebastian Lague's and Acerola's videos regarding the same topic, and to an extent a similarly themed class assignment.\
Although I add my own twists, with the end goal for this to be a standalone creation.

# TODO
* Move the ray tracing code onto a compute shader, so that the user can move the camera around with reasonable FPS.
* Update the current ComputeHelper with a new one I've made for other projects.
* Code in more standardized shapes (cubes, planes, quadrics, implicits, bezier surface patches, etc).
* Code in complex shapes (meshs, hierarchies, constructive geometry, etc).
* Additional features (some of which may already be done):
*   Anti-Aliasing/super sampling (uniform grid, stochastic pattern, adaptive, jittering).
*   mirror/fresnel reflection.
*   refraction.
*   motion blur.
*   depth of field blur.
*   area lights + path tracing.
* Non ray-tracer features
*   Environment maps (cube and or sphere).
*   texture maps (additionally, adaptive sampling/mipmaps).
*   perlin/simplex/procedual noise/textures.
* Optimization
*   Hierarchical Bounding volumes for complex meshes.
*   spatial hashing/ray marching.
