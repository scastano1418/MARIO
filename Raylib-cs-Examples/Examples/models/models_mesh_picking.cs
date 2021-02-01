/*******************************************************************************************
*
*   raylib [models] example - Mesh picking in 3d mode, ground plane, triangle, mesh
*
*   This example has been created using raylib 1.7 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2015 Ramon Santamaria (@raysan5)
*   Example contributed by Joel Davis (@joeld42)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.Color;
using static Raylib_cs.CameraType;
using static Raylib_cs.CameraMode;
using static Raylib_cs.MaterialMapType;

namespace Examples
{
    public class models_mesh_picking
    {
        public const float FLT_MAX = 3.40282347E+38F;

        public unsafe static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - mesh picking");

            // Define the camera to look into our 3d world
            Camera3D camera;
            camera.position = new Vector3(20.0f, 20.0f, 20.0f); // Camera3D position
            camera.target = new Vector3(0.0f, 8.0f, 0.0f);      // Camera3D looking at point
            camera.up = new Vector3(0.0f, 1.6f, 0.0f);          // Camera3D up vector (rotation towards target)
            camera.fovy = 45.0f;                                // Camera3D field-of-view Y
            camera.type = CAMERA_PERSPECTIVE;                   // Camera3D mode type

            Ray ray = new Ray();        // Picking ray

            Model tower = LoadModel("resources/models/turret.obj");                     // Load OBJ model
            Texture2D texture = LoadTexture("resources/models/turret_diffuse.png");     // Load model texture
            Utils.SetMaterialTexture(ref tower, 0, MAP_ALBEDO, ref texture);            // Set map diffuse texture

            Vector3 towerPos = new Vector3(0.0f, 0.0f, 0.0f);       // Set model position
            Mesh* meshes = (Mesh*)tower.meshes.ToPointer();
            BoundingBox towerBBox = MeshBoundingBox(meshes[0]);     // Get mesh bounding box
            bool hitMeshBBox = false;
            bool hitTriangle = false;

            // Test triangle
            Vector3 ta = new Vector3(-25.0f, 0.5f, 0.0f);
            Vector3 tb = new Vector3(-4.0f, 2.5f, 1.0f);
            Vector3 tc = new Vector3(-8.0f, 6.5f, 0.0f);

            Vector3 bary = new Vector3(0.0f, 0.0f, 0.0f);

            SetCameraMode(camera, CAMERA_FREE); // Set a free camera mode

            SetTargetFPS(60);                   // Set our game to run at 60 frames-per-second

            //----------------------------------------------------------------------------------
            // Main game loop
            //--------------------------------------------------------------------------------------
            while (!WindowShouldClose())        // Detect window close button or ESC key
            {
                //----------------------------------------------------------------------------------
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);          // Update camera

                // Display information about closest hit
                RayHitInfo nearestHit = new RayHitInfo();
                string hitObjectName = "None";
                nearestHit.distance = FLT_MAX;
                nearestHit.hit = false;
                Color cursorColor = WHITE;

                // Get ray and test against ground, triangle, and mesh
                ray = GetMouseRay(GetMousePosition(), camera);

                // Check ray collision aginst ground plane
                RayHitInfo groundHitInfo = GetCollisionRayGround(ray, 0.0f);

                if ((groundHitInfo.hit) && (groundHitInfo.distance < nearestHit.distance))
                {
                    nearestHit = groundHitInfo;
                    cursorColor = GREEN;
                    hitObjectName = "Ground";
                }

                // Check ray collision against test triangle
                RayHitInfo triHitInfo = GetCollisionRayTriangle(ray, ta, tb, tc);

                if ((triHitInfo.hit) && (triHitInfo.distance < nearestHit.distance))
                {
                    nearestHit = triHitInfo;
                    cursorColor = PURPLE;
                    hitObjectName = "Triangle";

                    bary = Vector3Barycenter(nearestHit.position, ta, tb, tc);
                    hitTriangle = true;
                }
                else hitTriangle = false;

                RayHitInfo meshHitInfo = new RayHitInfo();

                // Check ray collision against bounding box first, before trying the full ray-mesh test
                if (CheckCollisionRayBox(ray, towerBBox))
                {
                    hitMeshBBox = true;

                    // Check ray collision against model
                    // NOTE: It considers model.transform matrix!
                    meshHitInfo = GetCollisionRayModel(ray, tower);

                    if ((meshHitInfo.hit) && (meshHitInfo.distance < nearestHit.distance))
                    {
                        nearestHit = meshHitInfo;
                        cursorColor = ORANGE;
                        hitObjectName = "Mesh";
                    }

                }
                hitMeshBBox = false;
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(RAYWHITE);

                BeginMode3D(camera);

                // Draw the tower
                // WARNING: If scale is different than 1.0f,
                // not considered by GetCollisionRayModel()
                DrawModel(tower, towerPos, 1.0f, WHITE);

                // Draw the test triangle
                DrawLine3D(ta, tb, PURPLE);
                DrawLine3D(tb, tc, PURPLE);
                DrawLine3D(tc, ta, PURPLE);

                // Draw the mesh bbox if we hit it
                if (hitMeshBBox) DrawBoundingBox(towerBBox, LIME);

                // If we hit something, draw the cursor at the hit point
                if (nearestHit.hit)
                {
                    DrawCube(nearestHit.position, 0.3f, 0.3f, 0.3f, cursorColor);
                    DrawCubeWires(nearestHit.position, 0.3f, 0.3f, 0.3f, RED);

                    Vector3 normalEnd;
                    normalEnd.X = nearestHit.position.X + nearestHit.normal.X;
                    normalEnd.Y = nearestHit.position.Y + nearestHit.normal.Y;
                    normalEnd.Z = nearestHit.position.Z + nearestHit.normal.Z;

                    DrawLine3D(nearestHit.position, normalEnd, RED);
                }

                DrawRay(ray, MAROON);

                DrawGrid(10, 10.0f);

                EndMode3D();

                // Draw some debug GUI text
                DrawText(string.Format("Hit Object: {0}", hitObjectName), 10, 50, 10, BLACK);

                if (nearestHit.hit)
                {
                    int ypos = 70;

                    var x = string.Format("Distance: {0:000.00}", nearestHit.distance);
                    DrawText(string.Format("Distance: {0:000.00}", nearestHit.distance), 10, ypos, 10, BLACK);

                    DrawText(string.Format("Hit Pos: {0:000.00} {1:000.00} {2:000.00}",
                                        nearestHit.position.X,
                                        nearestHit.position.Y,
                                        nearestHit.position.Z), 10, ypos + 15, 10, BLACK);

                    DrawText(string.Format("Hit Norm: {0:000.00} {1:000.00} {2:000.00}",
                                        nearestHit.normal.X,
                                        nearestHit.normal.Y,
                                        nearestHit.normal.Z), 10, ypos + 30, 10, BLACK);

                    if (hitTriangle) DrawText(string.Format("Barycenter:{0:000.00} {1:000.00} {2:000.00}", bary.X, bary.Y, bary.Z), 10, ypos + 45, 10, BLACK);
                }

                DrawText("Use Mouse to Move Camera", 10, 430, 10, GRAY);

                DrawText("(c) Turret 3D model by Alberto Cano", screenWidth - 200, screenHeight - 20, 10, GRAY);

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadModel(tower);         // Unload model
            UnloadTexture(texture);     // Unload texture

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}