﻿using Infusion.IO;

namespace Infusion.Packets.Client
{
    public class DoubleClickRequest : MaterializedPacket
    {
        private Packet rawPacket;

        public DoubleClickRequest()
        {
        }

        public DoubleClickRequest(uint itemId)
        {
            byte[] payload = new byte[5];

            var writer = new ArrayPacketWriter(payload);
            writer.WriteByte((byte)PacketDefinitions.DoubleClick.Id);
            writer.WriteUInt(itemId);
            rawPacket = new Packet(PacketDefinitions.DoubleClick.Id, payload);

            ItemId = itemId;
        }

        public override Packet RawPacket => rawPacket;

        public override void Deserialize(Packet rawPacket)
        {
            this.rawPacket = rawPacket;

            var reader = new ArrayPacketReader(rawPacket.Payload);
            reader.Skip(1);
            ItemId = reader.ReadUInt();
        }

        public uint ItemId { get; private set; }
    }
}