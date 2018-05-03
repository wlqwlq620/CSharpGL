﻿using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace CSharpGL
{
    // 没有显式索引时的渲染方法。
    /// <summary>
    /// Wraps glDrawArrays(uint mode, int first, int count).
    /// </summary>
    [Editor(typeof(PropertyGridEditor), typeof(UITypeEditor))]
    public class DrawArraysCmd : IDrawCommand
    {
        /// <summary>
        /// Wraps glDrawArrays(uint mode, int first, int count).
        /// </summary>
        /// <param name="mode">用哪种方式渲染各个顶点？（GL.GL_TRIANGLES etc.）</param>
        /// <param name="maxVertexCount">一共有多少个元素？<para>How many vertexes in total?</para></param>
        public DrawArraysCmd(DrawMode mode, int maxVertexCount)
            : this(mode, maxVertexCount, 0, maxVertexCount)
        { }

        /// <summary>
        /// Wraps glDrawArrays(uint mode, int first, int count).
        /// </summary>
        /// <param name="mode">用哪种方式渲染各个顶点？（GL.GL_TRIANGLES etc.）</param>
        /// <param name="maxVertexCount">一共有多少个元素？<para>How many vertexes in total?</para></param>
        /// <param name="firstVertex">要渲染的第一个顶点的位置。<para>Index of first vertex to be rendered.</para></param>
        /// <param name="vertexCount">要渲染多少个元素？<para>How many vertexes to be rendered?</para></param>
        public DrawArraysCmd(DrawMode mode, int maxVertexCount, int firstVertex, int vertexCount)
        {
            this.Mode = mode;
            this.MaxVertexCount = maxVertexCount;
            this.FirstVertex = firstVertex;
            this.VertexCount = vertexCount;
        }

        /// <summary>
        /// 此VBO含有多少个元素？
        /// <para>How many elements in thie buffer?</para>
        /// </summary>
        public int MaxVertexCount { get; private set; }

        /// <summary>
        /// 要渲染的第一个顶点的位置。<para>Index of first vertex to be rendered.</para>
        /// </summary>
        [Category("ControlMode.Random")]
        public int FirstVertex { get; set; }

        /// <summary>
        /// 要渲染多少个元素？<para>How many vertexes to be rendered?</para>
        /// </summary>
        [Category("ControlMode.Random")]
        public int VertexCount { get; set; }

        #region IDrawCommand

        /// <summary>
        /// 用哪种方式渲染各个顶点？（GL.GL_TRIANGLES etc.）
        /// </summary>
        public DrawMode Mode { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="indexAccessMode">index buffer is accessable randomly or only by frame.</param>
        public void Draw(IndexAccessMode indexAccessMode)
        {
            uint mode = (uint)this.Mode;
            int first = this.FirstVertex;
            int count = this.VertexCount;
            GL.Instance.DrawArrays(mode, first, count);
        }

        #endregion IDrawCommand

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var builder = new System.Text.StringBuilder();

            var mode = this.Mode;
            int vertexCount = this.MaxVertexCount;

            builder.AppendLine("ControlMode.ByFrame:");
            builder.AppendLine(string.Format("glDrawArrays(mode: {0}, first: {1}, count: {2});", mode, 0, vertexCount));

            builder.AppendLine("ControlMode.Random:");
            builder.AppendLine(string.Format("glDrawArrays(mode: {0}, first: {1}, count: {2});", mode, this.FirstVertex, this.VertexCount));

            return builder.ToString();
        }
    }
}