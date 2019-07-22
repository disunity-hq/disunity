using System;


namespace Disunity.Core {

    [Flags]
    public enum LogLevel {

        None = 0,
        Fatal = 1,
        Error = 2,
        Warning = 4,
        Message = 8,
        Info = 16, // 0x00000010
        Debug = 32, // 0x00000020
        All = Debug | Info | Message | Warning | Error | Fatal, // 0x0000003F

    }

}