/*******************************************************************************************
*
*   raylib [core] example - 3d camera first person
*
*   This example has been created using raylib 1.3 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2015 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.CameraType;
using static Raylib_cs.CameraMode;
using static Raylib_cs.Color;

namespace Examples
{
    public class core_3d_camera_first_person
    {
        public const int MAX_COLUMNS = 20;

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [core] example - 3d camera first person");

            // Define the camera to look into our 3d world (position, target, up vector)
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(4.0f, 2.0f, 4.0f);
            camera.target = new Vector3(0.0f, 1.8f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 60.0f;
            camera.type = (int)CAMERA_PERSPECTIVE;

            // Generates some random columns
            float[] heights = new float[MAX_COLUMNS];
            Vector3[] positions = new Vector3[MAX_COLUMNS];
            Color[] colors = new Color[MAX_COLUMNS];

            for (int i = 0; i < MAX_COLUMNS; i++)
            {
                heights[i] = (float)GetRandomValue(1, 12);
                positions[i] = new Vector3(GetRandomValue(-15, 15), heights[i] / 2, GetRandomValue(-15, 15));
                colors[i] = new Color(GetRandomValue(20, 255), GetRandomValue(10, 55), 30, 255);
            }

            SetCameraMode(camera, CAMERA_FIRST_PERSON); // Set a first person camera mode

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

                DrawPlane(new Vector3(0.0f, 0.0f, 0.0f), new Vector2(32.0f, 32.0f), LIGHTGRAY); // Draw ground
                DrawCube(new Vector3(-16.0f, 2.5f, 0.0f), 1.0f, 5.0f, 32.0f, BLUE);     // Draw a Color.blue wall
                DrawCube(new Vector3(16.0f, 2.5f, 0.0f), 1.0f, 5.0f, 32.0f, LIME);      // Draw a Color.green wall
                DrawCube(new Vector3(0.0f, 2.5f, 16.0f), 32.0f, 5.0f, 1.0f, GOLD);      // Draw a Color.yellow wall

                // Draw some cubes around
                for (int i = 0; i < MAX_COLUMNS; i++)
                {
                    DrawCube(positions[i], 2.0f, heights[i], 2.0f, colors[i]);
                    DrawCubeWires(positions[i], 2.0f, heights[i], 2.0f, MAROON);
                }

                EndMode3D();

                DrawRectangle(10, 10, 220, 70, Fade(SKYBLUE, 0.5f));
                DrawRectangleLines(10, 10, 220, 70, BLUE);

                DrawText("First person camera default controls:", 20, 20, 10, BLACK);
                DrawText("- Move with keys: W, A, S, D", 40, 40, 10, DARKGRAY);
                DrawText("- Mouse move to look around", 40, 60, 10, DARKGRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}