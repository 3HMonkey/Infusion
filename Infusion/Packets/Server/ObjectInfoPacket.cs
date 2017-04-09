﻿using Infusion.IO;

namespace Infusion.Packets.Server
{
    public class ObjectInfoPacket : MaterializedPacket
    {
        public ModelId Type { get; private set; }

        public uint Id { get; private set; }

        public ushort Amount { get; private set; }

        public Location3D Location { get; private set; }

        public ObjectFlag Flags { get; private set; }

        public Movement Facing { get; private set; }

        public override void Deserialize(Packet rawPacket)
        {
            this.rawPacket = rawPacket;
            var reader = new ArrayPacketReader(rawPacket.Payload);

            reader.Skip(3);
            uint rawId = reader.ReadUInt();
            ushort rawType = reader.ReadUShort();

            if ((rawId & 0x80000000) != 0)
            {
                Id = rawId - 0x80000000;
                Amount = reader.ReadUShort();
            }
            else
            {
                Amount = 1;
                Id = rawId;
            }

            if ((rawType & 0x8000) != 0)
            {
                throw new PacketParsingException(rawPacket, "Not implementated: Type & 0x8000");
            }
            else
            {
                Type = (ModelId) rawType;
            }

            ushort xloc = reader.ReadUShort();
            ushort yloc = reader.ReadUShort();

            if ((xloc & 0x8000) != 0)
            {
                xloc -= 0x8000;
                Facing = (Movement) reader.ReadByte();
            }

            byte zloc = reader.ReadByte();

            if ((yloc & 0x8000) != 0)
            {
                yloc -= 0x8000;
                Dye = (Color) reader.ReadUShort();
            }

            if ((yloc & 0x4000) != 0)
            {
                yloc -= 0x4000;
                this.Flags = (ObjectFlag)reader.ReadByte();
            }

            Location = new Location3D(xloc, yloc, zloc);
        }

        public Color Dye { get; private set; }

        private Packet rawPacket;

        public override Packet RawPacket => rawPacket;
    }
}