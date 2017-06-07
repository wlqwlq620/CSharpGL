﻿using System;

namespace CSharpGL
{
    // http://blog.csdn.net/csxiaoshui/article/details/32101977
    /// <summary>
    /// Buffer object that not work as input variable in shader.
    /// </summary>
    public partial class UniformBuffer : GLBuffer
    {
        private static OpenGL.glUniformBlockBinding glUniformBlockBinding;
        //private static OpenGL.glBindBufferRange glBindBufferRange;
        private static OpenGL.glBindBufferBase glBindBufferBase;

        /// <summary>
        /// Target that this buffer should bind to.
        /// </summary>
        public override BufferTarget Target
        {
            get { return BufferTarget.UniformBuffer; }
        }

        /// <summary>
        /// pixel unpack buffer's pointer.
        /// </summary>
        /// <param name="bufferId">用glGenBuffers()得到的VBO的Id。<para>Id got from glGenBuffers();</para></param>
        /// <param name="length">此VBO含有多个个元素？<para>How many elements?</para></param>
        /// <param name="byteLength">此VBO中的数据在内存中占用多少个字节？<para>How many bytes in this buffer?</para></param>
        internal UniformBuffer(
            uint bufferId, int length, int byteLength)
            : base(bufferId, length, byteLength)
        {

        }

        /// <summary>
        /// Bind this uniform buffer object and a uniform block to the same binding point.
        /// </summary>
        /// <param name="program">shader program.</param>
        /// <param name="uniformBlockIndex">index of uniform block got by (glGetUniformBlockIndex).</param>
        /// <param name="bindingPoint">binding point maintained by OpenGL context.</param>
        public void Binding(ShaderProgram program, uint uniformBlockIndex, uint bindingPoint)
        {
            if (glBindBufferBase == null) { glBindBufferBase = OpenGL.GetDelegateFor<OpenGL.glBindBufferBase>(); }
            if (glUniformBlockBinding == null) { glUniformBlockBinding = OpenGL.GetDelegateFor<OpenGL.glUniformBlockBinding>(); }

            // 将 缓冲区 绑定到binding point
            glBindBufferBase(OpenGL.GL_UNIFORM_BUFFER, bindingPoint, this.BufferId);
            // 将 Uniform Block 绑定到binding point
            glUniformBlockBinding(program.ProgramId, uniformBlockIndex, bindingPoint);
        }

        /// <summary>
        /// Creates a <see cref="UniformBuffer"/> object directly in server side(GPU) without initializing its value.
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="usage"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static UniformBuffer Create(Type elementType, int length, BufferUsage usage)
        {
            return (GLBuffer.Create(IndependentBufferTarget.UniformBuffer, elementType, length, usage) as UniformBuffer);
        }
    }
}