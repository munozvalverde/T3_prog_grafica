using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Formas
{
    public class Cubos : IShape
    {
        public float[] VerticesIniciales { get; } = {
            // Cara frontal (Z = -1)
            -0.5f, -0.5f, -0.5f,  1.0f, 0.0f, 0.0f, // 0
             0.5f, -0.5f, -0.5f,  1.0f, 0.0f, 0.0f, // 1
             0.5f,  0.5f, -0.5f,  1.0f, 0.0f, 0.0f, // 2
            -0.5f,  0.5f, -0.5f,  1.0f, 0.0f, 0.0f, // 3
            
            // Cara trasera (Z = 1)
            -0.5f, -0.5f,  0.5f,  0.0f, 1.0f, 0.0f, // 4
             0.5f, -0.5f,  0.5f,  0.0f, 1.0f, 0.0f, // 5
             0.5f,  0.5f,  0.5f,  0.0f, 1.0f, 0.0f, // 6
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f, 0.0f, // 7
        };

        public float[] Vertices { get; private set; }

        public uint[] Indices { get; } = {
            // Cara frontal
            0, 1, 2, 2, 3, 0,
            // Cara trasera
            4, 5, 6, 6, 7, 4,
            // Caras laterales
            0, 3, 7, 7, 4, 0,
            1, 2, 6, 6, 5, 1,
            // Caras superior/inferior
            0, 1, 5, 5, 4, 0,
            2, 3, 7, 7, 6, 2
        };

        private Vector3 _centro;
        public Vector3 Centro
        {
            get => _centro;
            set
            {
                _centro = value;
                UpdateVertices(); // Aquí va tu lógica adicional
            }
        }

        private void UpdateVertices()
        {
            Vertices = new float[VerticesIniciales.Length];
            for (int i = 0; i < VerticesIniciales.Length; i += 6)
            {
                Vertices[i + 0] = VerticesIniciales[i + 0] + Centro.X; // x
                Vertices[i + 1] = VerticesIniciales[i + 1] + Centro.Y; // y
                Vertices[i + 2] = VerticesIniciales[i + 2] + Centro.Z; // z

                // color
                Vertices[i + 3] = VerticesIniciales[i + 3];
                Vertices[i + 4] = VerticesIniciales[i + 4];
                Vertices[i + 5] = VerticesIniciales[i + 5];
            }
        }


        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector3 Rotation { get; set; } = Vector3.Zero;
        public Vector3 Scale { get; set; } = Vector3.One;
        public int VertexArrayObject { get; set; }
        public int VertexBufferObject { get; set; }
        public int ElementBufferObject { get; set; }

        public Cubos(float x, float y, float z)
        {
            Centro = new Vector3(x, y, z); // Usa el campo privado directamente
            UpdateVertices(); // Actualiza los vértices
        }



        public void SetupBuffers()
        {
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
        }
    }
}