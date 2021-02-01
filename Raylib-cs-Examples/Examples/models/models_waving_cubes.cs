/*******************************************************************************************
*
*   raylib [models] example - Waving cubes
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Codecat (@codecat) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2019 Codecat (@codecat) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.CameraType;

namespace Examples
{
    public class models_waving_cubes
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - waving cubes");

            // Initialize the camera
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(30.0f, 20.0f, 30.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 70.0f;
            camera.type = CAMERA_PERSPECTIVE;

            // Specify the amount of blocks in each direction
            const int numBlocks = 15;

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                double time = GetTime();

                // Calculate time scale for cube position and size
                float scale = (2.0f + (float)Math.Sin(time)) * 0.7f;

                // Move camera around the scene
                double cameraTime = time * 0.3;
                camera.position.X = (float)Math.Cos(cameraTime) * 40.0f;
                camera.position.Z = (float)Math.Sin(cameraTime) * 40.0f;
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(RAYWHITE);

                BeginMode3D(camera);

                DrawGrid(10, 5.0f);

                for (int x = 0; x < numBlocks; x++)
                {
                    for (int y = 0; y < numBlocks; y++)
                    {
                        for (int z = 0; z < numBlocks; z++)
                        {
                            // Scale of the blocks depends on x/y/z positions
                            float blockScale = (x + y + z) / 30.0f;

                            // Scatter makes the waving effect by adding blockScale over time
                            float scatter = (float)Math.Sin(blockScale * 20.0f + (float)(time * 4.0f));

                            // Calculate the cube position
                            Vector3 cubePos = new Vector3(
                                (float)(x - numBlocks/2)*(scale*3.0f) + scatter,
                                (float)(y - numBlocks/2)*(scale*2.0f) + scatter,
                                (float)(z - numBlocks/2)*(scale*3.0f) + scatter
                            );

                            // Pick a color with a hue depending on cube position for the rainbow color effect
                            Color cubeColor = ColorFromHSV(new Vector3((float)(((x + y + z) * 18) % 360), 0.75f, 0.9f));

                            // Calculate cube size
                            float cubeSize = (2.4f - scale) * blockScale;

                            // And finally, draw the cube!
                            DrawCube(cubePos, cubeSize, cubeSize, cubeSize, cubeColor);
                        }
                    }
                }

                EndMode3D();

                DrawFPS(10, 10);

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