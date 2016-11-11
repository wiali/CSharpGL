﻿using System;
namespace CSharpGL
{
    /// <summary>
    /// Teapot.
    /// <para>Uses <see cref="OneIndexBuffer"/></para>
    /// </summary>
    public class Teapot : IBufferable
    {
        /// <summary>
        ///
        /// </summary>
        public Teapot()
        {
            this.model = new TeapotModel();
        }

        /// <summary>
        ///
        /// </summary>
        public const string strPosition = "position";

        /// <summary>
        ///
        /// </summary>
        public const string strColor = "color";

        /// <summary>
        ///
        /// </summary>
        public const string strNormal = "normal";

        private TeapotModel model;
        private VertexAttributeBuffer positionBuffer;
        private VertexAttributeBuffer colorBuffer;
        private VertexAttributeBuffer normalBuffer;

        /// <summary>
        ///
        /// </summary>
        /// <param name="bufferName"></param>
        /// <param name="varNameInShader"></param>
        /// <returns></returns>
        public VertexAttributeBuffer GetVertexAttributeBuffer(string bufferName, string varNameInShader)
        {
            if (bufferName == strPosition)
            {
                if (this.positionBuffer == null)
                {
                    float[] positions = model.GetPositions();
                    int length = positions.Length;
                    VertexAttributeBuffer bufferPtr = VertexAttributeBuffer.Create(typeof(float), length, VertexAttributeConfig.Vec3, BufferUsage.StaticDraw, varNameInShader);
                    unsafe
                    {
                        IntPtr pointer = bufferPtr.MapBuffer(MapBufferAccess.WriteOnly);
                        var array = (float*)pointer;
                        for (int i = 0; i < positions.Length; i++)
                        {
                            array[i] = positions[i];
                        }
                        bufferPtr.UnmapBuffer();
                    }
                    this.positionBuffer = bufferPtr;
                }
                return this.positionBuffer;
            }
            else if (bufferName == strColor)
            {
                if (this.colorBuffer == null)
                {
                    float[] normals = model.GetNormals();
                    int length = normals.Length;
                    VertexAttributeBuffer bufferPtr = VertexAttributeBuffer.Create(typeof(float), length, VertexAttributeConfig.Vec3, BufferUsage.StaticDraw, varNameInShader);
                    unsafe
                    {
                        IntPtr pointer = bufferPtr.MapBuffer(MapBufferAccess.WriteOnly);
                        var array = (float*)pointer;
                        for (int i = 0; i < normals.Length; i++)
                        {
                            array[i] = normals[i];
                        }
                        bufferPtr.UnmapBuffer();
                    }
                    this.colorBuffer = bufferPtr;
                }
                return this.colorBuffer;
            }
            else if (bufferName == strNormal)
            {
                if (this.normalBuffer == null)
                {
                    float[] normals = model.GetNormals();
                    int length = normals.Length;
                    VertexAttributeBuffer bufferPtr = VertexAttributeBuffer.Create(typeof(float), length, VertexAttributeConfig.Vec3, BufferUsage.StaticDraw, varNameInShader);
                    unsafe
                    {
                        IntPtr pointer = bufferPtr.MapBuffer(MapBufferAccess.WriteOnly);
                        var array = (float*)pointer;
                        for (int i = 0; i < normals.Length; i++)
                        {
                            array[i] = normals[i];
                        }
                        bufferPtr.UnmapBuffer();
                    }
                    this.normalBuffer = bufferPtr;
                }
                return this.normalBuffer;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IndexBuffer GetIndexBuffer()
        {
            if (indexBuffer == null)
            {
                ushort[] faces = model.GetFaces();
                int length = faces.Length;
                OneIndexBuffer bufferPtr = OneIndexBuffer.Create(BufferUsage.StaticDraw, DrawMode.Triangles, IndexElementType.UShort, length);
                unsafe
                {
                    IntPtr pointer = bufferPtr.MapBuffer(MapBufferAccess.WriteOnly);
                    var array = (ushort*)pointer;
                    for (int i = 0; i < faces.Length; i++)
                    {
                        array[i] = (ushort)(faces[i] - 1);
                    }
                    bufferPtr.UnmapBuffer();
                }
                this.indexBuffer = bufferPtr;
            }

            return indexBuffer;
        }

        private IndexBuffer indexBuffer = null;

        /// <summary>
        /// Uses <see cref="ZeroIndexBuffer"/> or <see cref="OneIndexBuffer"/>.
        /// </summary>
        /// <returns></returns>
        public bool UsesZeroIndexBuffer() { return false; }

        /// <summary>
        ///
        /// </summary>
        public vec3 Lengths { get { return new vec3(6.42963028f, 3.15f, 4.0f); } }
    }
}