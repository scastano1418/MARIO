/*******************************************************************************************
*
*   raylib [shapes] example - bouncing ball
*
*   This example has been created using raylib 1.0 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2013 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples
{
    public class shapes_bouncing_ball
    {
        public static int Main()
        {
            // Initialization
            //---------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [shapes] example - bouncing ball");

            Vector2 ballPosition = new Vector2(GetScreenWidth() / 2, GetScreenHeight() / 2);
            Vector2 ballSpeed = new Vector2(5.0f, 4.0f);
            int ballRadius = 20;

            bool pause = false;
            int framesCounter = 0;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
                                            //----------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //-----------------------------------------------------
                if (IsKeyPressed(KEY_SPACE)) pause = !pause;

                if (!pause)
                {
                    ballPosition.X += ballSpeed.X;
                    ballPosition.Y += ballSpeed.Y;

                    // Check walls collision for bouncing
                    if ((ballPosition.X >= (GetScreenWidth() - ballRadius)) || (ballPosition.X <= ballRadius)) ballSpeed.X *= -1.0f;
                    if ((ballPosition.Y >= (GetScreenHeight() - ballRadius)) || (ballPosition.Y <= ballRadius)) ballSpeed.Y *= -1.0f;
                }
                else framesCounter++;
                //-----------------------------------------------------

                // Draw
                //-----------------------------------------------------
                BeginDrawing();

                ClearBackground(RAYWHITE);

                DrawCircleV(ballPosition, ballRadius, MAROON);
                DrawText("PRESS SPACE to PAUSE BALL MOVEMENT", 10, GetScreenHeight() - 25, 20, LIGHTGRAY);

                // On pause, we draw a blinking message
                if (pause && ((framesCounter / 30) % 2) == 0) DrawText("PAUSED", 350, 200, 30, GRAY);

                DrawFPS(10, 10);

                EndDrawing();
                //-----------------------------------------------------
            }

            // De-Initialization
            //---------------------------------------------------------
            CloseWindow();        // Close window and OpenGL context
            //----------------------------------------------------------

            return 0;
        }
    }
}