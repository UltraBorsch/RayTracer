# RayTracer
Inspiration taken from Sebastian Lague's and Acerola's videos regarding the same topic, and to an extent a similarly themed class assignment.\
Although, I add my own twists, with the end goal for this to be a standalone creation.

# TODO
* General refactoring is much needed.
* Code in more standardized shapes (cubes, planes, quadrics, implicits, bezier surface patches, etc).
* Code in complex shapes (meshes, hierarchies, constructive geometry, etc).
    * Asserting that said shapes conform, e.g. generating normals when meshes do not provide them, no trailing vertices, etc.
* Additional features (some of which may already be done):
    * Anti-Aliasing/super sampling (uniform grid, stochastic pattern, adaptive, jittering).
    * Mirror/fresnel reflection.
    * Refraction.
    * Motion blur.
    * Depth of field blur.
    * Area lights + path tracing.
* Non ray-tracer features
    * Environment maps (cube and or sphere).
    * Texture maps (additionally, adaptive sampling/mipmaps).
    * Perlin/simplex/procedual noise/textures.
* Optimization
    * Hierarchical Bounding volumes for complex meshes.
    * Spatial hashing/ray marching.
# Known Issues

