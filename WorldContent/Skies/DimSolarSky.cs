﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics;
using Terraria.Graphics.Effects;

namespace Auralite.WorldContent.Skies
{
	internal class DimSolarSky : CustomSky
	{
		private struct Meteor
		{
			public Vector2 Position;

			public float Depth;

			public int FrameCounter;

			public float Scale;

			public float StartX;
		}

		private Random _random = new Random();

		private Texture2D _planetTexture;

		private Texture2D _bgTexture;

		private Texture2D _meteorTexture;

		private bool _isActive;

		private DimSolarSky.Meteor[] _meteors;

		private float _fadeOpacity;

		public override void OnLoad()
		{
			this._planetTexture = ModLoader.GetTexture("Terraria/Misc/SolarSky/Planet");
			this._bgTexture = ModLoader.GetTexture("Terraria/Misc/SolarSky/Background");
			this._meteorTexture = ModLoader.GetTexture("Terraria/Misc/SolarSky/Meteor");
		}

		public override void Update(GameTime gametime)
		{
			if (this._isActive)
			{
				this._fadeOpacity = Math.Min(1f, 0.01f + this._fadeOpacity);
			}
			else
			{
				this._fadeOpacity = Math.Max(0f, this._fadeOpacity - 0.01f);
			}
			float num = 20f;
			for (int i = 0; i < this._meteors.Length; i++)
			{
				DimSolarSky.Meteor[] expr_60_cp_0_cp_0 = this._meteors;
				int expr_60_cp_0_cp_1 = i;
				expr_60_cp_0_cp_0[expr_60_cp_0_cp_1].Position.X = expr_60_cp_0_cp_0[expr_60_cp_0_cp_1].Position.X - num;
				DimSolarSky.Meteor[] expr_7E_cp_0_cp_0 = this._meteors;
				int expr_7E_cp_0_cp_1 = i;
				expr_7E_cp_0_cp_0[expr_7E_cp_0_cp_1].Position.Y = expr_7E_cp_0_cp_0[expr_7E_cp_0_cp_1].Position.Y + num;
				if ((double)this._meteors[i].Position.Y > Main.worldSurface * 16.0)
				{
					this._meteors[i].Position.X = this._meteors[i].StartX;
					this._meteors[i].Position.Y = -10000f;
				}
			}
		}

		public override Color OnTileColor(Color inColor)
		{
			Vector4 vector = inColor.ToVector4();
			return new Color(Vector4.Lerp(vector, Vector4.One, this._fadeOpacity * 0.5f));
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
			{
				spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * this._fadeOpacity);
				spriteBatch.Draw(this._bgTexture, new Rectangle(0, Math.Max(0, (int)((Main.worldSurface * 16.0 - (double)Main.screenPosition.Y - 2400.0) * 0.10000000149011612)), Main.screenWidth, Main.screenHeight), Color.White * Math.Min(1f, (Main.screenPosition.Y - 800f) / 1000f * this._fadeOpacity));
				Vector2 vector = new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
				Vector2 vector2 = 0.01f * (new Vector2((float)Main.maxTilesX * 8f, (float)Main.worldSurface / 2f) - Main.screenPosition);
				spriteBatch.Draw(this._planetTexture, vector + new Vector2(-200f, -200f) + vector2, null, Color.White * 0.9f * this._fadeOpacity, 0f, new Vector2((float)(this._planetTexture.Width >> 1), (float)(this._planetTexture.Height >> 1)), 1f, 0, 1f);
			}
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < this._meteors.Length; i++)
			{
				float depth = this._meteors[i].Depth;
				if (num == -1 && depth < maxDepth)
				{
					num = i;
				}
				if (depth <= minDepth)
				{
					break;
				}
				num2 = i;
			}
			if (num == -1)
			{
				return;
			}
			float num3 = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
			Vector2 vector3 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
			Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
			for (int j = num; j < num2; j++)
			{
				Vector2 vector4 = new Vector2(1f / this._meteors[j].Depth, 0.9f / this._meteors[j].Depth);
				Vector2 vector5 = (this._meteors[j].Position - vector3) * vector4 + vector3 - Main.screenPosition;
				int num4 = this._meteors[j].FrameCounter / 3;
				this._meteors[j].FrameCounter = (this._meteors[j].FrameCounter + 1) % 12;
				if (rectangle.Contains((int)vector5.X, (int)vector5.Y))
				{
					spriteBatch.Draw(this._meteorTexture, vector5, new Rectangle?(new Rectangle(0, num4 * (this._meteorTexture.Height / 4), this._meteorTexture.Width, this._meteorTexture.Height / 4)), Color.White * num3 * this._fadeOpacity, 0f, Vector2.Zero, vector4.X * 5f * this._meteors[j].Scale, 0, 0f);
				}
			}
		}

		public override float GetCloudAlpha()
		{
			return (1f - this._fadeOpacity) * 0.3f + 0.7f;
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			this._fadeOpacity = 0.002f;
			this._isActive = true;
			this._meteors = new DimSolarSky.Meteor[150];
			for (int i = 0; i < this._meteors.Length; i++)
			{
				float num = (float)i / (float)this._meteors.Length;
				this._meteors[i].Position.X = num * ((float)Main.maxTilesX * 16f) + this._random.NextFloat() * 40f - 20f;
				this._meteors[i].Position.Y = this._random.NextFloat() * -((float)Main.worldSurface * 16f + 10000f) - 10000f;
				if (this._random.Next(3) != 0)
				{
					this._meteors[i].Depth = this._random.NextFloat() * 3f + 1.8f;
				}
				else
				{
					this._meteors[i].Depth = this._random.NextFloat() * 5f + 4.8f;
				}
				this._meteors[i].FrameCounter = this._random.Next(12);
				this._meteors[i].Scale = this._random.NextFloat() * 0.5f + 1f;
				this._meteors[i].StartX = this._meteors[i].Position.X;
			}
			Array.Sort<DimSolarSky.Meteor>(this._meteors, new Comparison<DimSolarSky.Meteor>(this.SortMethod));
		}

		private int SortMethod(DimSolarSky.Meteor meteor1, DimSolarSky.Meteor meteor2)
		{
			return meteor2.Depth.CompareTo(meteor1.Depth);
		}

		public override void Deactivate(params object[] args)
		{
			this._isActive = false;
		}

		public override void Reset()
		{
			this._isActive = false;
		}

		public override bool IsActive()
		{
			return this._isActive || this._fadeOpacity > 0.001f;
		}
	}
}
