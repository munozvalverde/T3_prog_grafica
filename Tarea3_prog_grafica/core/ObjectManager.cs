using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Formas;
using System.Collections.Generic;

namespace Core
{
    public class ObjectManager
    {
        private readonly List<IShape> _formas = new();
        private readonly ShaderManager _shaderManager;

        public ObjectManager(ShaderManager shaderManager)
        {
            _shaderManager = shaderManager;
        }

        // Expone la lista de formas como propiedad pública de solo lectura
        public IReadOnlyList<IShape> Formas => _formas.AsReadOnly();

        public void AddShape(IShape forma)
        {
            forma.SetupBuffers();
            _formas.Add(forma);
        }

        public void RenderAll(Matrix4 view, Matrix4 projection)
        {
            GL.UseProgram(_shaderManager.ShaderProgram);

            foreach (var forma in _formas)
            {
                Matrix4 model = Matrix4.CreateScale(forma.Scale) *
                              Matrix4.CreateRotationX(forma.Rotation.X) *
                              Matrix4.CreateRotationY(forma.Rotation.Y) *
                              Matrix4.CreateRotationZ(forma.Rotation.Z) *
                              Matrix4.CreateTranslation(forma.Position);

                GL.UniformMatrix4(GL.GetUniformLocation(_shaderManager.ShaderProgram, "model"), false, ref model);
                GL.UniformMatrix4(GL.GetUniformLocation(_shaderManager.ShaderProgram, "view"), false, ref view);
                GL.UniformMatrix4(GL.GetUniformLocation(_shaderManager.ShaderProgram, "projection"), false, ref projection);

                GL.BindVertexArray(forma.VertexArrayObject);
                GL.DrawElements(PrimitiveType.Triangles, forma.Indices.Length, DrawElementsType.UnsignedInt, 0);
            }
        }
    }
}