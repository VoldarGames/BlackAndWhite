using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Assets.Scripts.Utils
{
    public static class Global
    {
        public const float CanbedamagedCooldownTime = 0.1f;
        public const int CheckpointCellWidth = 1;
        public const int CheckpointCellHeight = 2;
        public const int RandomPathOffset = 3;
        public const float NavMeshAgentDestinationTolerance = 1.0f;
        public const float LandTeamPercentage = 0.3f;
        public const int MaxNumberOfSoundType = 10;
        public const float CooldownSoundType = 0.1f;
        public const float SoundTypeMinimumSpacingFactor = 0.25f;
        public const float SoundTypeMaximumSpacingFactor = 0.5f;
        public const float CanSpawnOriginOffsetForRaycast = 10.0f;

        public struct TeamId
        {
            public static readonly int A = 0;
            public static readonly int B = 1;
        }

        public struct TeamLayerId  
        {
            public static readonly int A = 8;
            public static readonly int B = 9;
        }

        public struct BulletLayerId
        {
            public static readonly int A = 10;
            public static readonly int B = 11;
        }


        
    }
}
