/*******************************************************************************************
*
*   raylib [models] example - Plane rotations (yaw, pitch, roll)
*
*   This example has been created using raylib 1.8 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example based on Berni work on Raspberry Pi:
*   http://forum.raylib.com/index.php?p=/discussion/124/line-versus-triangle-drawing-order
*
*   Copyright (c) 2017 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.Color;
using static Raylib_cs.CameraType;
using static Raylib_cs.MaterialMapType;
using static Raylib_cs.BlendMode;
using static Raylib_cs.KeyboardKey;

namespace Examples
{
    public class models_yaw_pitch_roll
    {
        //----------------------------------------------------------------------------------
        // Main entry point
        //----------------------------------------------------------------------------------
        public unsafe static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - plane rotations (yaw, pitch, roll)");

            Texture2D texAngleGauge = LoadTexture("resources/angle_gauge.png");
            Texture2D texBackground = LoadTexture("resources/background.png");
            Texture2D texPitch = LoadTexture("resources/pitch.png");
            Texture2D texPlane = LoadTexture("resources/plane.png");

            RenderTexture2D framebuffer = LoadRenderTexture(192, 192);

            // Model loading
            Model model = LoadModel("resources/plane.obj");      // Load OBJ model

            // Set map diffuse texture
            Material *materials = (Material*)model.materials.ToPointer();
            MaterialMap* maps = (MaterialMap*)materials[0].maps.ToPointer();
            maps[(int)MAP_ALBEDO].texture = LoadTexture("resources/plane_diffuse.png");

            GenTextureMipmaps(ref maps[(int)MAP_ALBEDO].texture);

            Camera3D camera = new Camera3D();
            camera.position = new Vector3(0.0f, 60.0f, -120.0f);  // Camera3D position perspective
            camera.target = new Vector3(0.0f, 12.0f, 0.0f);       // Camera3D looking at point
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);            // Camera3D up vector (rotation towards target)
            camera.fovy = 30.0f;                                  // Camera3D field-of-view Y
            camera.type = CAMERA_PERSPECTIVE;                     // Camera3D type

            float pitch = 0.0f;
            float roll = 0.0f;
            float yaw = 0.0f;

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------

                // Plane roll (x-axis) controls
                if (IsKeyDown(KEY_LEFT)) roll += 1.0f;
                else if (IsKeyDown(KEY_RIGHT)) roll -= 1.0f;
                else
                {
                    if (roll > 0.0f) roll -= 0.5f;
                    else if (roll < 0.0f) roll += 0.5f;
                }

                // Plane yaw (y-axis) controls
                if (IsKeyDown(KEY_S)) yaw += 1.0f;
                else if (IsKeyDown(KEY_A)) yaw -= 1.0f;
                else
                {
                    if (yaw > 0.0f) yaw -= 0.5f;
                    else if (yaw < 0.0f) yaw += 0.5f;
                }

                // Plane pitch (z-axis) controls
                if (IsKeyDown(KEY_DOWN)) pitch += 0.6f;
                else if (IsKeyDown(KEY_UP)) pitch -= 0.6f;
                else
                {
                    if (pitch > 0.3f) pitch -= 0.3f;
                    else if (pitch < -0.3f) pitch += 0.3f;
                }

                // Wraps the phase of an angle to fit between -180 and +180 degrees
                int pitchOffset = (int)pitch;
                while (pitchOffset > 180) pitchOffset -= 360;
                while (pitchOffset < -180) pitchOffset += 360;
                pitchOffset *= 10;

                Matrix transform = MatrixIdentity();

                transform = MatrixMultiply(transform, MatrixRotateZ(DEG2RAD * roll));
                transform = MatrixMultiply(transform, MatrixRotateX(DEG2RAD * pitch));
                transform = MatrixMultiply(transform, MatrixRotateY(DEG2RAD * yaw));

                model.transform = transform;
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(RAYWHITE);

                // Draw framebuffer texture (Ahrs Display)
                int centerX = framebuffer.texture.width / 2;
                int centerY = framebuffer.texture.height / 2;
                float scaleFactor = 0.5f;

                BeginTextureMode(framebuffer);

                BeginBlendMode((int)BLEND_ALPHA);

                DrawTexturePro(texBackground, new Rectangle(0, 0, texBackground.width, texBackground.height),
                               new Rectangle(centerX, centerY, texBackground.width * scaleFactor, texBackground.height * scaleFactor),
                               new Vector2(texBackground.width / 2 * scaleFactor, texBackground.height / 2 * scaleFactor + pitchOffset * scaleFactor), roll, WHITE);

                DrawTexturePro(texPitch, new Rectangle(0, 0, texPitch.width, texPitch.height),
                               new Rectangle(centerX, centerY, texPitch.width * scaleFactor, texPitch.height * scaleFactor),
                               new Vector2(texPitch.width / 2 * scaleFactor, texPitch.height / 2 * scaleFactor + pitchOffset * scaleFactor), roll, WHITE);

                DrawTexturePro(texPlane, new Rectangle(0, 0, texPlane.width, texPlane.height),
                               new Rectangle(centerX, centerY, texPlane.width * scaleFactor, texPlane.height * scaleFactor),
                               new Vector2(texPlane.width / 2 * scaleFactor, texPlane.height / 2 * scaleFactor), 0, WHITE);

                EndBlendMode();

                EndTextureMode();

                // Draw 3D model (recomended to draw 3D always before 2D)
                BeginMode3D(camera);

                DrawModel(model, new Vector3(0, 6.0f, 0), 1.0f, WHITE);   // Draw 3d model with texture
                DrawGrid(10, 10.0f);

                EndMode3D();

                // Draw 2D GUI stuff
                DrawAngleGauge(texAngleGauge, 80, 70, roll, "roll", RED);
                DrawAngleGauge(texAngleGauge, 190, 70, pitch, "pitch", GREEN);
                DrawAngleGauge(texAngleGauge, 300, 70, yaw, "yaw", SKYBLUE);

                DrawRectangle(30, 360, 260, 70, Fade(SKYBLUE, 0.5f));
                DrawRectangleLines(30, 360, 260, 70, Fade(DARKBLUE, 0.5f));
                DrawText("Pitch controlled with: KEY_UP / KEY_DOWN", 40, 370, 10, DARKGRAY);
                DrawText("Roll controlled with: KEY_LEFT / KEY_RIGHT", 40, 390, 10, DARKGRAY);
                DrawText("Yaw controlled with: KEY_A / KEY_S", 40, 410, 10, DARKGRAY);

                // Draw framebuffer texture
                DrawTextureRec(framebuffer.texture, new Rectangle(0, 0, framebuffer.texture.width, -framebuffer.texture.height),
                               new Vector2(screenWidth - framebuffer.texture.width - 20, 20), Fade(WHITE, 0.8f));

                DrawRectangleLines(screenWidth - framebuffer.texture.width - 20, 20, framebuffer.texture.width, framebuffer.texture.height, DARKGRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------

            // Unload all loaded data
            UnloadModel(model);

            UnloadRenderTexture(framebuffer);

            UnloadTexture(texAngleGauge);
            UnloadTexture(texBackground);
            UnloadTexture(texPitch);
            UnloadTexture(texPlane);

            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }

        // Draw angle gauge controls
        static void DrawAngleGauge(Texture2D angleGauge, int x, int y, float angle, string title, Color color)
        {
            Rectangle srcRec = new Rectangle(0, 0, angleGauge.width, angleGauge.height);
            Rectangle dstRec = new Rectangle(x, y, angleGauge.width, angleGauge.height);
            Vector2 origin = new Vector2(angleGauge.width / 2, angleGauge.height / 2);
            int textSize = 20;

            DrawTexturePro(angleGauge, srcRec, dstRec, origin, angle, color);

            DrawText(string.Format("{0:00000.0}", angle), x - MeasureText(string.Format("0:00000.0", angle), textSize) / 2, y + 10, textSize, DARKGRAY);
            DrawText(title, x - MeasureText(title, textSize) / 2, y + 60, textSize, DARKGRAY);
        }
    }
}