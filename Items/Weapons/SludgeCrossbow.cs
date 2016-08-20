using Terraria.ID;
using Terraria;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Auralite.Items.Weapons
{
	public class SludgeCrossbow : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Sludge crossbow";
			item.damage = 14;
			item.ranged = true;
			item.width = 40;
			item.height = 20;
			item.toolTip = "Converts arrows into sludge arrows";
			item.useTime = 33;
			item.useAnimation = 33;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 4;
            item.value = 30000;
            item.rare = 3;
            item.useSound = 5;
			item.autoReuse = false;
			item.shoot = 3; //idk why but all the guns in the vanilla source have this
			item.shootSpeed = 8f;
			item.useAmmo = 1;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("SludgeArrow"), damage, knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}