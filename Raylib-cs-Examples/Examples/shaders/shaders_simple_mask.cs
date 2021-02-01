/*******************************************************************************************
*
*   raylib [shaders] example - Simple shader mask
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Chris Camacho (@codifies) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2019 Chris Camacho (@codifies) and Ramon Santamaria (@raysan5)
*
********************************************************************************************
*
*   After a model is loaded it has a default material, this material can be
*   modified in place rather than creating one from scratch...
*   While all of the maps have particular names, they can be used for any purpose
*   except for three maps that are applied as cubic maps (see below)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.Color;
using static Raylib_cs.CameraType;
using static Raylib_cs.MaterialMapType;
using static Raylib_cs.ShaderUniformDataType;
using static Raylib_cs.ShaderLocationIndex;

namespace Examples
{
    public class shaders_simple_mask
    {
        public unsafe static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib - simple shader mask");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(0.0f, 1.0f, 2.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.type = CAMERA_PERSPECTIVE;

            // Define our three models to show the shader on
            Mesh torus = GenMeshTorus(.3f, 1, 16, 32);
            Model model1 = LoadModelFromMesh(torus);

            Mesh cube = GenMeshCube(.8f, .8f, .8f);
            Model model2 = LoadModelFromMesh(cube);

            // Generate model to be shaded just to see the gaps in the other two
            Mesh sphere = GenMeshSphere(1, 16, 16);
            Model model3 = LoadModelFromMesh(sphere);

            // Load the shader
            Shader shader = LoadShader("resources/shaders/glsl330/mask.vs", "resources/shaders/glsl330/mask.fs");

            // Load and apply the diffuse texture (colour map)
            Texture2D texDiffuse = LoadTexture("resources/plasma.png");

            Material *materials = (Material*)model1.materials.ToPointer();
            MaterialMap* maps = (MaterialMap*)materials[0].maps.ToPointer();
            maps[(int)MAP_ALBEDO].texture = texDiffuse;

            materials = (Material*)model2.materials.ToPointer();
            maps = (MaterialMap*)materials[0].maps.ToPointer();
            maps[(int)MAP_ALBEDO].texture = texDiffuse;

            // Using MAP_EMISSION as a spare slot to use for 2nd texture
            // NOTE: Don't use MAP_IRRADIANCE, MAP_PREFILTER or  MAP_CUBEMAP
            // as they are bound as cube maps
            Texture2D texMask = LoadTexture("resources/mask.png");

            materials = (Material*)model1.materials.ToPointer();
            maps = (MaterialMap*)materials[0].maps.ToPointer();
            maps[(int)MAP_EMISSION].texture = texMask;

            materials = (Material*)model2.materials.ToPointer();
            maps = (MaterialMap*)materials[0].maps.ToPointer();
            maps[(int)MAP_EMISSION].texture = texMask;

            int *locs = (int*)shader.locs.ToPointer();
            locs[(int)LOC_MAP_EMISSION] = GetShaderLocation(shader, "mask");

            // Frame is incremented each frame to animate the shader
            int shaderFrame = GetShaderLocation(shader, "framesCounter");

            // Apply the shader to the two models
            materials = (Material*)model1.materials.ToPointer();
            materials[0].shader = shader;

            materials = (Material*)model2.materials.ToPointer();
            materials[0].shader = shader;

            int framesCounter = 0;
            Vector3 rotation = new Vector3(0, 0, 0);       // Model rotation angles

            SetTargetFPS(60);               // Set  to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                framesCounter++;
                rotation.X += 0.01f;
                rotation.Y += 0.005f;
                rotation.Z -= 0.0025f;

                // Send frames counter to shader for animation
                // SetShaderValue(shader, shaderFrame, IntPtr.Zero, UNIFORM_INT);

                // Rotate one of the models
                model1.transform = MatrixRotateXYZ(rotation);

                UpdateCamera(ref camera);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(DARKBLUE);

                BeginMode3D(camera);

                DrawModel(model1, new Vector3(0.5f, 0, 0), 1, WHITE);
                DrawModelEx(model2, new Vector3(-.5f, 0, 0), new Vector3(1, 1, 0), 50, new Vector3(1, 1, 1), WHITE);
                DrawModel(model3, new Vector3(0, 0, -1.5f), 1, WHITE);
                DrawGrid(10, 1.0f);        // Draw a grid

                EndMode3D();

                DrawRectangle(16, 698, MeasureText(string.Format("Frame: {0}", framesCounter), 20) + 8, 42, BLUE);
                DrawText(string.Format("Frame: {0}", framesCounter), 20, 700, 20, WHITE);

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadModel(model1);
            UnloadModel(model2);
            UnloadModel(model3);

            UnloadTexture(texDiffuse);  // Unload default diffuse texture
            UnloadTexture(texMask);     // Unload texture mask

            UnloadShader(shader);       // Unload shader

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}