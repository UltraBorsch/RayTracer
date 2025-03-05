# RayTracer
Inspiration taken from Sebastian Lague's and Acerola's videos regarding the same topic, and to an extent a similarly themed class assignment.\
Although, I add my own twists, with the end goal for this to be a standalone creation.

# TODO
* General refactoring is much needed.
* More standardized shapes (implicits, bezier surface patches, etc).
* More complex shapes (meshes, hierarchies, constructive geometry, etc).
    * Asserting that said shapes conform, e.g. generating normals when meshes do not provide them, no trailing vertices, etc.
* Transformation matrices for applicable geometry types (notably not spheres and planes).
* Bounds for certain geometry (could possibly use for all geometry types, but in particular for planes and quadrics so that they're not infinite).
* Additional features:
    * Light attenuation.
    * Anti-Aliasing/super sampling (uniform grid, stochastic pattern, adaptive, jittering, etc).
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
* Optimization
    * Hierarchical Bounding volumes for complex meshes.
    * Spatial hashing/ray marching.
    * Culling, probably by giving every (complex) geometry a bounding box.
    * Precomputing certain values when possible/worthwhile.
# Known Issues
   * Bugs with some quadrics. Notably, elliptic cones clearly work but ellipsoids do not look like ellipsoids.
   * Potential visual bug with AABBs. Certain faces sometimes stop rendering at certain angles. I have noticed it specifically with the faces perpendicular to the y-axis, but does not happen always.
