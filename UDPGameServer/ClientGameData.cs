using System.Collections.Generic;

namespace UDPGameServer
{
    public class ClientGameData
    {
        public string Guid;

        public CreatureData PlayerData;
        public List<ProjectileData> ProjectileDatas;
    }
}