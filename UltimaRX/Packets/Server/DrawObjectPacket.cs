﻿using System.Collections.Generic;
using UltimaRX.IO;

namespace UltimaRX.Packets.Server
{
    public class DrawObjectPacket : MaterializedPacket
    {
        private Packet rawPacket;

        public int Id { get; private set; }
        public ushort Type { get; private set; }
        public Location3D Location { get; private set; }

        public override Packet RawPacket => rawPacket;
        public Direction Direction { get; private set; }
        public Color Color { get; private set; }
        public Notoriety Notoriety { get; private set; }
        public IEnumerable<Item> Items { get; private set; }

        public override void Deserialize(Packet rawPacket)
        {
            this.rawPacket = rawPacket;
            var reader = new ArrayPacketReader(rawPacket.Payload);
            reader.Position = 3;

            Id = reader.ReadInt();
            Type = reader.ReadUShort();
            Location = new Location3D(reader.ReadUShort(), reader.ReadUShort(), reader.ReadByte());
            Direction = (Direction) reader.ReadByte();
            Color = (Color) reader.ReadUShort();
            var ignoredFlag = reader.ReadByte();
            Notoriety = (Notoriety) reader.ReadByte();

            var items = new List<Item>();
            var itemId = reader.ReadInt();
            while (itemId != 0x00000000)
            {
                var type = reader.ReadUShort();
                var layer = (Layer) reader.ReadByte();
                Color? color = null;

                if ((type & 0x8000) != 0)
                {
                    type -= 0x8000;
                    color = (Color) reader.ReadUShort();
                }

                var item = new Item(itemId, type, 1, new Location3D(0, 0, 0), containerId: Id,
                    layer: layer, color: color);

                items.Add(item);

                itemId = reader.ReadInt();
            }

            Items = items.ToArray();
        }
    }
}