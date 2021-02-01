/*******************************************************************************************
*
*   Physac - Physics demo
*
*   NOTE 1: Physac requires multi-threading, when InitPhysics() a second thread is created to manage physics calculations.
*   NOTE 2: Physac requires static C library linkage to avoid dependency on MinGW DLL (-static -lpthread)
*
*   Copyright (c) 2016-2018 Victor Fisac
*
********************************************************************************************/

using System;
using System.Memory;
using System.Runtime.InteropServices;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Physac;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples
{
    public class physics_demo
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            SetConfigFlags(ConfigFlag.FLAG_MSAA_4X_HINT);
            InitWindow(screenWidth, screenHeight, "Physac [raylib] - Physics demo");

            // Physac logo drawing position
            int logoX = screenWidth - MeasureText("Physac", 30) - 10;
            int logoY = 15;
            bool needsReset = false;

            // Initialize physics and default physics bodies
            InitPhysics();

            // Create floor rectangle physics body
            var data = CreatePhysicsBodyRectangle(new Vector2(screenWidth / 2, screenHeight), 500, 100, 10);
            Span<PhysicsBodyData> floor = new Span<PhysicsBodyData>(data);
            floor.enabled = false; // Disable body state to convert it to static (no dynamics, but collisions)
            floor.isGrounded = true;

            Console.WriteLine(Marshal.SizeOf<PhysicsBodyData>());
            Console.WriteLine(Marshal.SizeOf<PhysicsShape>());
            Console.WriteLine(Marshal.SizeOf<PolygonData>());
            Console.WriteLine(Marshal.SizeOf<bool>());

            // Create obstacle circle physics body
            var circle = CreatePhysicsBodyCircle(new Vector2(screenWidth / 2, screenHeight / 2), 45, 10);
            circle.enabled = false; // Disable body state to convert it to static (no dynamics, but collisions)

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                // Delay initialization of variables due to physics reset async
                RunPhysicsStep();

                if (needsReset)
                {
                    floor = CreatePhysicsBodyRectangle(new Vector2(screenWidth / 2, screenHeight), 500, 100, 10);
                    floor.enabled = false;

                    circle = CreatePhysicsBodyCircle(new Vector2(screenWidth / 2, screenHeight / 2), 45, 10);
                    circle.enabled = false;
                }

                // Reset physics input
                if (IsKeyPressed(KEY_R))
                {
                    ResetPhysics();
                    needsReset = true;
                }

                // Physics body creation inputs
                if (IsMouseButtonPressed(MOUSE_LEFT_BUTTON)) CreatePhysicsBodyPolygon(GetMousePosition(), GetRandomValue(20, 80), GetRandomValue(3, 8), 10);
                else if (IsMouseButtonPressed(MOUSE_RIGHT_BUTTON)) CreatePhysicsBodyCircle(GetMousePosition(), GetRandomValue(10, 45), 10);

                // Destroy falling physics bodies
                int bodiesCount = GetPhysicsBodiesCount();
                for (int i = bodiesCount - 1; i >= 0; i--)
                {
                    var body = GetPhysicsBody(i);
                    if (!body.Equals(default(PhysicsBodyData)) && (body.position.y > screenHeight * 2))
                        DestroyPhysicsBody(body);
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(BLACK);

                DrawFPS(screenWidth - 90, screenHeight - 30);

                // Draw created physics bodies
                bodiesCount = GetPhysicsBodiesCount();
                for (int i = 0; i < bodiesCount; i++)
                {
                    var body = GetPhysicsBody(i);

                    if (body.id != 0) // can't check struct for null
                    {
                        int vertexCount = GetPhysicsShapeVerticesCount(i);
                        for (int j = 0; j < vertexCount; j++)
                        {
                            // Get physics bodies shape vertices to draw lines
                            // Note: GetPhysicsShapeVertex() already calculates rotation transformations
                            Vector2 vertexA = GetPhysicsShapeVertex(body, j);

                            int jj = (((j + 1) < vertexCount) ? (j + 1) : 0);   // Get next vertex or first to close the shape
                            Vector2 vertexB = GetPhysicsShapeVertex(body, jj);

                            DrawLineV(vertexA, vertexB, GREEN);     // Draw a line between two vertex positions
                        }
                    }
                }

                DrawText("Left mouse button to create a polygon", 10, 10, 10, WHITE);
                DrawText("Right mouse button to create a circle", 10, 25, 10, WHITE);
                DrawText("Press 'R' to reset example", 10, 40, 10, WHITE);

                DrawText("Physac", logoX, logoY, 30, WHITE);
                DrawText("Powered by", logoX + 50, logoY - 7, 10, WHITE);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            ClosePhysics();       // Unitialize physics

            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
