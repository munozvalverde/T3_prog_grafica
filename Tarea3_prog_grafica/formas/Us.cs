using Formas;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Formas
{
    public class Us : IShape
    {
        public float[] VerticesIniciales { get; } = {
            // Columna izquierda                   // nro vértice 
            -0.35f, 0.5f, 0.1f,  0.0f, 0.0f, 1.0f, // 0
            -0.20f, 0.5f, 0.1f,  0.0f, 0.0f, 1.0f, // 1
            -0.20f, -0.5f, 0.1f,  0.0f, 0.0f, 1.0f, // 2
            -0.35f, -0.5f, 0.1f,  0.0f, 0.0f, 1.0f, // 3    color azul

            -0.35f, 0.5f, -0.1f,  1.0f, 0.0f, 0.0f, // 4
            -0.20f, 0.5f, -0.1f,  1.0f, 0.0f, 0.0f, // 5
            -0.20f, -0.5f, -0.1f,  1.0f, 0.0f, 0.0f, // 6
            -0.35f, -0.5f, -0.1f,  1.0f, 0.0f, 0.0f, // 7   color rojo


            // Columna derecha
             0.20f, 0.5f, 0.1f,  0.0f, 0.0f, 1.0f, // 8
             0.35f, 0.5f, 0.1f,  0.0f, 0.0f, 1.0f, // 9
             0.35f, -0.5f, 0.1f,  0.0f, 0.0f, 1.0f, //10
             0.20f, -0.5f, 0.1f,  0.0f, 0.0f, 1.0f, //11   color azul

             0.20f, 0.5f, -0.1f,  1.0f, 0.0f, 0.0f, //12
             0.35f, 0.5f, -0.1f,  1.0f, 0.0f, 0.0f, //13
             0.35f, -0.5f, -0.1f,  1.0f, 0.0f, 0.0f, //14
             0.20f, -0.5f, -0.1f,  1.0f, 0.0f, 0.0f, //15  color rojo


            // Base de la U
            -0.35f, -0.45f, 0.1f,  0.0f, 0.0f, 1.0f, //16
             0.35f, -0.45f, 0.1f,  0.0f, 0.0f, 1.0f, //17
            0.35f, -0.6f, 0.1f,  0.0f, 0.0f, 1.0f, //18
            -0.35f, -0.6f, 0.1f,  0.0f, 0.0f, 1.0f, //19  color azuul

            -0.35f, -0.45f, -0.1f,  1.0f, 0.0f, 0.0f, //20
             0.35f, -0.45f, -0.1f,  1.0f, 0.0f, 0.0f, //21
             0.35f, -0.6f, -0.1f,  1.0f, 0.0f, 0.0f, //22
            -0.35f, -0.6f, -0.1f,  1.0f, 0.0f, 0.0f  //23   color rojo

        };

        public float[] Vertices { get; private set; }

        public uint[] Indices { get; } = {
            // Caras columna izquierda
            0, 1, 2, 2, 3, 0, // cara frontal
            4, 5, 6, 6, 7, 4, // cara trasera
            0, 1, 5, 5, 4, 0, // cara superior
            2, 3, 7, 7, 6, 2, // cara inferiorrr
            0, 3, 7, 7, 4, 0, // cara izquierda
            1, 2, 6, 6, 5, 1, // cara derechha

            // Caras columna derecha
            8, 9, 10, 10, 11, 8,
            12, 13, 14, 14, 15, 12,
            8, 9, 13, 13, 12, 8,
            10, 11, 15, 15, 14, 10,
            8, 11, 15, 15, 12, 8,
            9, 10, 14, 14, 13, 9,

            // Caras base
            16, 17, 18, 18, 19, 16,
            20, 21, 22, 22, 23, 20,
            16, 17, 21, 21, 20, 16,
            18, 19, 23, 23, 22, 18,
            16, 19, 23, 23, 20, 16,
            17, 18, 22, 22, 21, 17
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

        public Us(float x, float y, float z)
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