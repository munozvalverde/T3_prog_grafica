using Formas;
using Core;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Runtime.Serialization;

namespace Core
{
    public class MainWindow : GameWindow
    {
        private readonly ObjectManager _objectManager;
        private readonly ShaderManager _shaderManager = new();
        private float _rotation = 0.0f;
        private Vector3 _cameraPosition = new Vector3(0, 0, 4);
 
        private float _moveSpeed = 0.5f; // Añadido
        private int _selectedObjectIndex = 0; // Añadido

        public MainWindow() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            Size = new Vector2i(800, 600);
            Title = "3D Shapes with OpenTK";
            _objectManager = new ObjectManager(_shaderManager);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            _shaderManager.LoadShaders();

            _objectManager.AddShape(new Cubos(-1,0,0)
            {
                Position = new Vector3(0, 0, 0),
                Scale = new Vector3(0.5f),
                // Rotation = new Vector3(0, 45, 0)
            }); ;

            _objectManager.AddShape(new Us(0, 0, 0)
            {
                Position = new Vector3(0, 0, 0),
                Scale = new Vector3(0.6f),
                //  Rotation = new Vector3(0, 0, 0)
            }) ;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            var keyboardState = KeyboardState;
            var time = (float)args.Time;

            // Rotación continua (opcional)
            _rotation += (float)args.Time;

            // Cambiar objeto seleccionado con teclas numéricas
            if (keyboardState.IsKeyDown(Keys.D1)) _selectedObjectIndex = 0;
            if (keyboardState.IsKeyDown(Keys.D2)) _selectedObjectIndex = 1;
            if (keyboardState.IsKeyDown(Keys.D3)) _selectedObjectIndex = 2;

            // Mover el objeto seleccionado
            if (_objectManager.Formas.Count > _selectedObjectIndex)
            {
                var selectedObject = _objectManager.Formas[_selectedObjectIndex];
                var newPosition = selectedObject.Position;

                if (keyboardState.IsKeyDown(Keys.B)) newPosition.Z -= _moveSpeed * time;
                if (keyboardState.IsKeyDown(Keys.F)) newPosition.Z += _moveSpeed * time;
                if (keyboardState.IsKeyDown(Keys.Left)) newPosition.X -= _moveSpeed * time;
                if (keyboardState.IsKeyDown(Keys.Right)) newPosition.X += _moveSpeed * time;
                if (keyboardState.IsKeyDown(Keys.Down)) newPosition.Y -= _moveSpeed * time;
                if (keyboardState.IsKeyDown(Keys.Up)) newPosition.Y += _moveSpeed * time;

                selectedObject.Position = newPosition;
            }

            
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 view = Matrix4.LookAt(_cameraPosition, Vector3.Zero, Vector3.UnitY);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100.0f);

            _objectManager.RenderAll(view, projection);

            SwapBuffers();
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
           
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
            base.OnUnload();
        }
    }
}