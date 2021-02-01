/*******************************************************************************************
*
*   raylib [shapes] example - following eyes
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2013-2019 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace Examples
{
    public class shapes_following_eyes
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [shapes] example - following eyes");

            Vector2 scleraLeftPosition = new Vector2(GetScreenWidth() / 2 - 100, GetScreenHeight() / 2);
            Vector2 scleraRightPosition = new Vector2(GetScreenWidth() / 2 + 100, GetScreenHeight() / 2);
            float scleraRadius = 80;

            Vector2 irisLeftPosition = new Vector2(GetScreenWidth() / 2 - 100, GetScreenHeight() / 2);
            Vector2 irisRightPosition = new Vector2(GetScreenWidth() / 2 + 100, GetScreenHeight() / 2);
            float irisRadius = 24;

            float angle = 0.0f;
            float dx = 0.0f, dy = 0.0f, dxx = 0.0f, dyy = 0.0f;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                irisLeftPosition = GetMousePosition();
                irisRightPosition = GetMousePosition();

                // Check not inside the left eye sclera
                if (!CheckCollisionPointCircle(irisLeftPosition, scleraLeftPosition, scleraRadius - 20))
                {
                    dx = irisLeftPosition.X - scleraLeftPosition.X;
                    dy = irisLeftPosition.Y - scleraLeftPosition.Y;

                    angle = (float)Math.Atan2(dy, dx);

                    dxx = (scleraRadius - irisRadius) * (float)Math.Cos(angle);
                    dyy = (scleraRadius - irisRadius) * (float)Math.Sin(angle);

                    irisLeftPosition.X = scleraLeftPosition.X + dxx;
                    irisLeftPosition.Y = scleraLeftPosition.Y + dyy;
                }

                // Check not inside the right eye sclera
                if (!CheckCollisionPointCircle(irisRightPosition, scleraRightPosition, scleraRadius - 20))
                {
                    dx = irisRightPosition.X - scleraRightPosition.X;
                    dy = irisRightPosition.Y - scleraRightPosition.Y;

                    angle = (float)Math.Atan2(dy, dx);

                    dxx = (scleraRadius - irisRadius) * (float)Math.Cos(angle);
                    dyy = (scleraRadius - irisRadius) * (float)Math.Sin(angle);

                    irisRightPosition.X = scleraRightPosition.X + dxx;
                    irisRightPosition.Y = scleraRightPosition.Y + dyy;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(RAYWHITE);

                DrawCircleV(scleraLeftPosition, scleraRadius, LIGHTGRAY);
                DrawCircleV(irisLeftPosition, irisRadius, BROWN);
                DrawCircleV(irisLeftPosition, 10, BLACK);

                DrawCircleV(scleraRightPosition, scleraRadius, LIGHTGRAY);
                DrawCircleV(irisRightPosition, irisRadius, DARKGREEN);
                DrawCircleV(irisRightPosition, 10, BLACK);

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