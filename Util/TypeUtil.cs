﻿using System;

namespace CM3D2.AlwaysColorChangeEx.Plugin.Util
{
    /// <summary>
    /// Description of TypeUtil.
    /// </summary>
    public class TypeUtil
    {
        public TypeUtil() {
        }
        public const int BODY_START = (int)MPN_TYPE_RANGE.BODY_START;
        public const int BODY_END   = (int)MPN_TYPE_RANGE.BODY_END;
        public const int WEAR_START = (int)MPN_TYPE_RANGE.WEAR_START;
        public const int WEAR_END   = (int)MPN_TYPE_RANGE.WEAR_END;
        public static bool IsBody(MPN mpn) {
            int mpnNo = (int)mpn;
            return  (mpnNo >= BODY_START && mpnNo <= BODY_END);
        }
        public static bool IsWear(MPN mpn) {
            int mpnNo = (int)mpn;
            return  (mpnNo >= WEAR_START && mpnNo <= WEAR_END);
        }
    }
}