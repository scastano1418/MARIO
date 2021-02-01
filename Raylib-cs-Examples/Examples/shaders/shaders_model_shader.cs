/*******************************************************************************************
*
*   raylib [shaders] example - Apply a shader to a 3d model
*
*   NOTE: This example requires raylib OpenGL 3.3 or ES2 versions for shaders support,
*         OpenGL 1.1 does not support shaders, recompile raylib to OpenGL 3.3 version.
*
*   NOTE: Shaders used in this example are #version 330 (OpenGL 3.3), to test this example
*         on OpenGL ES 2.0 platforms (Android, Raspberry Pi, HTML5), use #version 100 shaders
*         raylib comes with shaders ready for both versions, check raylib/shaders install folder
*
*   This example has been created using raylib 1.3 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2014 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.CameraType;
using static Raylib_cs.CameraMode;
using static Raylib_cs.MaterialMapType;

namespace Examples
{
    public class shaders_model_shader
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            SetConfigFlags(ConfigFlag.FLAG_MSAA_4X_HINT);      // Enable Multi Sampling Anti Aliasing 4x (if available)

            InitWindow(screenWidth, screenHeight, "raylib [shaders] example - model shader");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(4.0f, 4.0f, 4.0f);
            camera.target = new Vector3(0.0f, 1.0f, -1.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.type = CAMERA_PERSPECTIVE;

            Model model = LoadModel("resources/models/watermill.obj");                   // Load OBJ model
            Texture2D texture = LoadTexture("resources/models/watermill_diffuse.png");   // Load model texture
            Shader shader = LoadShader("resources/shaders/glsl330/base.vs",
                                       "resources/shaders/glsl330/grayscale.fs");   // Load model shader

            Utils.SetMaterialShader(ref model, 0, ref shader);  // Set shader effect to 3d model
            Utils.SetMaterialTexture(ref model, 0, MAP_ALBEDO, ref texture);    // Bind texture to model

            Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);    // Set model position

            SetCameraMode(camera, CAMERA_FREE);         // Set an orbital camera mode

            SetTargetFPS(60);                           // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())                // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera);                  // Update camera
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(RAYWHITE);

                BeginMode3D(camera);

                DrawModel(model, position, 0.2f, WHITE);   // Draw 3d model with texture

                DrawGrid(10, 1.0f);     // Draw a grid

                EndMode3D();

                DrawText("(c) Watermill 3D model by Alberto Cano", screenWidth - 210, screenHeight - 20, 10, GRAY);

                DrawText(string.Format("Camera3D position: ({0:0.00}, {0:0.00}, {0:0.00})", camera.position.X, camera.position.Y, camera.position.Z), 600, 20, 10, BLACK);
                DrawText(string.Format("Camera3D target: ({0:0.00}, {0:0.00}, {0:0.00})", camera.target.X, camera.target.Y, camera.target.Z), 600, 40, 10, GRAY);

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadShader(shader);       // Unload shader
            UnloadTexture(texture);     // Unload texture
            UnloadModel(model);         // Unload model

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}