﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointerSizeHelpers.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using SharpDX;

namespace HelixToolkit.SharpDX.Helpers
{
    /// <summary>
    /// Helpers methods for <see cref="PointerSize"/>.
    /// </summary>
    public static class PointerSizeHelpers
    {
        /// <summary>
        /// Converts a <see cref="PointerSize"/> to a 64-bit unsigned integer.
        /// </summary>
        /// <param name="ptr">The pointer to convert.</param>
        /// <returns>An <c>unsigned long</c>.</returns>
        public static ulong ToUInt64(this PointerSize ptr)
        {
            if (UIntPtr.Size == 8)
            {
                return (ulong)(long)ptr;
            }
            else
            {
                return (uint)(int)ptr;
            }
        }
    }
}
