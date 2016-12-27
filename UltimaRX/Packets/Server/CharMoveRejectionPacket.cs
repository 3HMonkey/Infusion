﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimaRX.IO;

namespace UltimaRX.Packets.Server
{
    public class CharMoveRejectionPacket : MaterializedPacket
    {
        private Packet rawPacket;
        public byte SequenceKey { get; private set; }

        public Location3D Location { get; private set; }

        public Direction Direction { get; private set; }

        public override void Deserialize(Packet rawPacket)
        {
            this.rawPacket = rawPacket;
            var reader = new ArrayPacketReader(rawPacket.Payload);
            reader.Skip(1);

            SequenceKey = reader.ReadByte();
            ushort xloc = reader.ReadUShort();
            ushort yloc = reader.ReadUShort();
            Direction = (Direction)reader.ReadByte();
            byte zloc = reader.ReadByte();

            Location = new Location3D(xloc, yloc, zloc);
        }

        public override Packet RawPacket => this.rawPacket;
    }
}