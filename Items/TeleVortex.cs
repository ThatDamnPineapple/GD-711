using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Auralite.NPCs;
using Auralite.WorldContent;
using AlternateDimensions;

namespace Auralite.Items
{
	public class TeleVortex : ModItem
	{

		public override void SetDefaults()
		{
			item.name = "Vortex Teleport";
			item.maxStack = 1;
			item.width = 40;
			item.height = 40;
			item.toolTip = "Test item. Teleport to Vortex Dimension.";
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.rare = 7;
			item.useStyle = 1;
			item.useTime = 10;
			item.useAnimation = 10;
		}

		public override bool UseItem(Player player)
		{
			//return to overworld if not in dimension
			if(player.GetModPlayer<AuralitePlayer>(mod).ZoneVortex) {
				AlternateDimensionInterface.DimensionSwap("vanilla", "vanillaarea");
			} else if(player.whoAmI == Main.myPlayer) {
				AlternateDimensionInterface.DimensionSwapTeleport(mod.Name, "Vortex", 250, 100, true);
			}
			return true;
		}
	}
}