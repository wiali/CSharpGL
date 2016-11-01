﻿using System;
using System.Runtime.InteropServices;

namespace CSharpGL
{
    /// <summary>
    /// an 3D axis
    /// </summary>
    public class Axis : IBufferable
    {
        private AxisModel model;

        /// <summary>
        /// an 3D axis
        /// </summary>
        /// <param name="partCount"></param>
        /// <param name="radius"></param>
        public Axis(int partCount = 24, float radius = 0.5f)
        {
            if (partCount < 2) { throw new ArgumentException(); }
            this.model = new AxisModel(partCount, radius);
            this.ModelSize = new vec3(radius * 2, radius * 2, radius * 2);
        }

        /// <summary>
        ///
        /// </summary>
        public const string strPosition = "position";

        /// <summary>
        ///
        /// </summary>
        public const string strColor = "color";

        private VertexAttributeBufferPtr positionBufferPtr;
        private VertexAttributeBufferPtr colorBufferPtr;

        /// <summary>
        ///
        /// </summary>
        /// <param name="bufferName"></param>
        /// <param name="varNameInShader"></param>
        /// <returns></returns>
        public VertexAttributeBufferPtr GetVertexAttributeBufferPtr(string bufferName, string varNameInShader)
        {
            if (bufferName == strPosition)
            {
                if (positionBufferPtr == null)
                {
                    //using (var buffer = new VertexAttributeBuffer<vec3>(
                    //    varNameInShader, VertexAttributeConfig.Vec3, BufferUsage.StaticDraw))
                    //{
                    //    buffer.Alloc(this.model.positions.Length);
                    //    unsafe
                    //    {
                    //        var array = (vec3*)buffer.Header.ToPointer();
                    //        for (int i = 0; i < this.model.positions.Length; i++)
                    //        {
                    //            array[i] = this.model.positions[i];
                    //        }
                    //    }

                    //    positionBufferPtr = buffer.GetBufferPtr();
                    //}
                    int length = this.model.positions.Length;
                    VertexAttributeBufferPtr ptr = VertexAttributeBufferPtr.Create(typeof(vec3), length, VertexAttributeConfig.Vec3, BufferUsage.StaticDraw, varNameInShader);
                    unsafe
                    {
                        IntPtr pointer = ptr.MapBuffer(MapBufferAccess.WriteOnly);
                        var array = (vec3*)pointer.ToPointer();
                        for (int i = 0; i < length; i++)
                        {
                            array[i] = this.model.positions[i];
                        }
                        ptr.UnmapBuffer();
                    }
                    this.positionBufferPtr = ptr;
                }
                return positionBufferPtr;
            }
            else if (bufferName == strColor)
            {
                if (colorBufferPtr == null)
                {
                    using (var buffer = new VertexAttributeBuffer<vec3>(
                        varNameInShader, VertexAttributeConfig.Vec3, BufferUsage.StaticDraw))
                    {
                        buffer.Alloc(this.model.colors.Length);
                        unsafe
                        {
                            var array = (vec3*)buffer.Header.ToPointer();
                            for (int i = 0; i < this.model.colors.Length; i++)
                            {
                                array[i] = this.model.colors[i];
                            }
                        }

                        colorBufferPtr = buffer.GetBufferPtr();
                    }
                }
                return colorBufferPtr;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IndexBufferPtr GetIndexBufferPtr()
        {
            if (indexBufferPtr == null)
            {
                OneIndexBufferPtr bufferPtr = OneIndexBufferPtr.Create(BufferUsage.StaticDraw, this.model.mode, IndexElementType.UInt, this.model.indexes.Length);
                unsafe
                {
                    IntPtr pointer = bufferPtr.MapBuffer(MapBufferAccess.WriteOnly);
                    var array = (uint*)pointer.ToPointer();
                    for (int i = 0; i < this.model.indexes.Length; i++)
                    {
                        array[i] = this.model.indexes[i];
                    }
                    bufferPtr.UnmapBuffer();
                }

                this.indexBufferPtr = bufferPtr;
            }

            return indexBufferPtr;
        }

        /// <summary>
        /// Uses <see cref="ZeroIndexBuffer"/> or <see cref="OneIndexBuffer"/>.
        /// </summary>
        /// <returns></returns>
        public bool UsesZeroIndexBuffer() { return false; }

        private IndexBufferPtr indexBufferPtr = null;

        /// <summary>
        ///
        /// </summary>
        public vec3 ModelSize { get; private set; }
    }
}