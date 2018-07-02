// ReSharper disable ArrangeMethodOrOperatorBody


using System.Collections.Generic;
using Aimtec;

#pragma warning disable 1587

namespace AIO.Utilities
{
    /// <summary>
    ///     The Drawing class.
    /// </summary>
    internal static class DrawingClass
    {
        #region Static Fields

        /// <summary>
        ///     A list of the names of the champions who have a different healthbar type.
        /// </summary>
        public static readonly List<string> SpecialChampions = new List<string> { "Annie", "Jhin", "Corki", "Xayah" };

        /// <summary>
        ///     The default enemy HP bar height offset.
        /// </summary>
        public static int SHeight = 8;

        /// <summary>
        ///     The default enemy HP bar width offset.
        /// </summary>
        public static int SWidth = 103;

        /// <summary>
        ///     The jungle HP bar offset list.
        /// </summary>
        internal static readonly List<JungleHpBarOffset> JungleHpBarOffsetList = new List<JungleHpBarOffset>
                                                                                     {
                                                                                         new JungleHpBarOffset { UnitSkinName = "SRU_Dragon_Air", Width = 140, Height = 6, XOffset = 9, YOffset = 24 },
                                                                                         new JungleHpBarOffset { UnitSkinName = "SRU_Dragon_Fire", Width = 140, Height = 6, XOffset = 9, YOffset = 24 },
                                                                                         new JungleHpBarOffset { UnitSkinName = "SRU_Dragon_Water", Width = 140, Height = 6, XOffset = 9, YOffset = 24 },
                                                                                         new JungleHpBarOffset { UnitSkinName = "SRU_Dragon_Earth", Width = 140, Height = 6, XOffset = 9, YOffset = 24 },
                                                                                         new JungleHpBarOffset { UnitSkinName = "SRU_Dragon_Elder", Width = 140, Height = 6, XOffset = 9, YOffset = 24 },
                                                                                         new JungleHpBarOffset { UnitSkinName = "SRU_Baron", Width = 190, Height = 5, XOffset = 17, YOffset = 12 },
                                                                                         new JungleHpBarOffset { UnitSkinName = "SRU_RiftHerald", Width = 140, Height = 4, XOffset = 10, YOffset = 24 },
                                                                                         new JungleHpBarOffset { UnitSkinName = "SRU_Red", Width = 139, Height = 4, XOffset = 10, YOffset = 24 },
                                                                                         new JungleHpBarOffset { UnitSkinName = "SRU_Blue", Width = 140, Height = 4, XOffset = 10, YOffset = 24 },
                                                                                         new JungleHpBarOffset { UnitSkinName = "SRU_Gromp", Width = 90, Height = 2, XOffset = 2, YOffset = 7 },
                                                                                         new JungleHpBarOffset { UnitSkinName = "Sru_Crab", Width = 60, Height = 2, XOffset = 0, YOffset = 5 },
                                                                                         new JungleHpBarOffset { UnitSkinName = "SRU_Krug", Width = 72, Height = 2, XOffset = 1, YOffset = 7 },
                                                                                         new JungleHpBarOffset { UnitSkinName = "SRU_Razorbeak", Width = 90, Height = 2, XOffset = 1, YOffset = 7 },
                                                                                         new JungleHpBarOffset { UnitSkinName = "SRU_Murkwolf", Width = 90, Height = 2, XOffset = 1, YOffset = 7 }
                                                                                     };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The default enemy HP bar x offset.
        /// </summary>
        public static int SxOffset(Obj_AI_Hero target)
        {
            return SpecialChampions.Contains(target.ChampionName) ? 33 : 30;
        }

        /// <summary>
        ///     The default enemy HP bar y offset.
        /// </summary>
        public static int SyOffset(Obj_AI_Hero target)
        {
            return SpecialChampions.Contains(target.ChampionName) ? 7 : 2;
        }

        #endregion

        /// <summary>
        ///     The jungle HP bar offset.
        /// </summary>
        internal class JungleHpBarOffset
        {
            #region Fields

            /// <summary>
            ///     The height.
            /// </summary>
            internal int Height;

            /// <summary>
            ///     The name.
            /// </summary>
            internal string UnitSkinName;

            /// <summary>
            ///     The width.
            /// </summary>
            internal int Width;

            /// <summary>
            ///     The XOffset.
            /// </summary>
            internal int XOffset;

            /// <summary>
            ///     The YOffset.
            /// </summary>
            internal int YOffset;

            #endregion
        }
    }
}