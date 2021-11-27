﻿using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System;
using OpenTKGuion.common;
using OpenTK;

namespace OpenTKGuion.model
{
    [Serializable()]
    class Cara
    {

        private int _vertexBufferObject;
        private int _elementBufferObject;
        private int _vertexArrayObject;
        private Shader _shader;

        public float[] _vertices;
        public uint[] _indices;
        public float r, g, b, a;

        //Los puntos se deben de nombrar en sentido contrario a las manecillas del reloj
        public void init(List<float> list, float x, float y, float z)
        {
            _vertices = new float[list.Count];
            _indices = new uint[(list.Count / 3 - 2) * 3];

            for (int i = 0; i < list.Count / 3; i++)
            {
                _vertices[i * 3 + 0] = list[i * 3 + 0] + x;
                _vertices[i * 3 + 1] = list[i * 3 + 1] + y;
                _vertices[i * 3 + 2] = list[i * 3 + 2] + z;
            }

            for (uint i = 0; i < list.Count / 3 - 2; i++)
            {
                _indices[0 + 3 * i] = 0;
                _indices[1 + 3 * i] = i + 1;
                _indices[2 + 3 * i] = i + 2;
            }
        }

        public void initOpenTK()
        {
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            _shader = new Shader("../../Shaders/shader.vert", "../../Shaders/shader.frag");
            _shader.Use();

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }

        public Cara()
        {
        }


        public Cara(List<float> list, float x, float y, float z, float r, float g, float b, float a)
        {

            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;

            init(list, x, y, z);
            initOpenTK();
        }

        public void render(Camera _camera, Matrix4 model)
        {
            if (_shader == null)
                initOpenTK();


            GL.BindVertexArray(_vertexArrayObject);
            _shader.Use();

            int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");
            GL.Uniform4(vertexColorLocation, r, g, b, a);


            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void dispose()
        {
            if (_shader != null)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindVertexArray(0);
                GL.UseProgram(0);

                GL.DeleteBuffer(_vertexBufferObject);
                GL.DeleteBuffer(_elementBufferObject);
                GL.DeleteVertexArray(_vertexArrayObject);
                GL.DeleteProgram(_shader.Handle);
            }
        }


    }
}
