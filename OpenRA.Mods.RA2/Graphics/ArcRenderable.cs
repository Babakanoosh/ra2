#region Copyright & License Information
/*
 * Copyright 2007-2019 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System.Linq;
using OpenRA.Graphics;
using OpenRA.Primitives;

namespace OpenRA.Mods.RA2.Graphics
{
	public struct ArcRenderable : IRenderable, IFinalizedRenderable
	{
		readonly Color color;
		readonly WPos a, b;
		readonly WAngle angle;
		readonly int zOffset;
		readonly WDist width;
		readonly int segments;

		public ArcRenderable(WPos a, WPos b, int zOffset, WAngle angle, Color color, WDist width, int segments)
		{
			this.a = a;
			this.b = b;
			this.angle = angle;
			this.color = color;
			this.zOffset = zOffset;
			this.width = width;
			this.segments = segments;
		}

		public WPos Pos { get { return a; } }
		public PaletteReference Palette { get { return null; } }
		public int ZOffset { get { return zOffset; } }
		public bool IsDecoration { get { return true; } }

		public IRenderable WithPalette(PaletteReference newPalette) { return new ArcRenderable(a, b, zOffset, angle, color, width, segments); }
		public IRenderable WithZOffset(int newOffset) { return new ArcRenderable(a, b, zOffset, angle, color, width, segments); }
		public IRenderable OffsetBy(WVec vec) { return new ArcRenderable(a + vec, b + vec, zOffset, angle, color, width, segments); }
		public IRenderable AsDecoration() { return this; }

		public IFinalizedRenderable PrepareRender(WorldRenderer wr) { return this; }
		public void Render(WorldRenderer wr)
		{
			var screenWidth = wr.ScreenVector(new WVec(width, WDist.Zero, WDist.Zero))[0];

			var points = new float3[segments + 1];
			for (var i = 0; i <= segments; i++)
				points[i] = wr.Screen3DPosition(WPos.LerpQuadratic(a, b, angle, i, segments));

			Game.Renderer.WorldRgbaColorRenderer.DrawLine(points, screenWidth, color, false);
		}

		public void RenderDebugGeometry(WorldRenderer wr) { }
		public Rectangle ScreenBounds(WorldRenderer wr) { return Rectangle.Empty; }
	}
}
