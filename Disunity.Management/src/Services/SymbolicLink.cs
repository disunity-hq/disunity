using System;
using System.IO;
using System.IO.Abstractions;
using System.Runtime.InteropServices;
using System.Text;

using BindingAttributes;

using Disunity.Management.Util;

using Microsoft.Win32.SafeHandles;


namespace Disunity.Management.Services {

    [AsSingleton(typeof(ISymbolicLink))]
    public class SymbolicLink : ISymbolicLink {

        private readonly IFileSystem _fileSystem;
        private const uint GenericReadAccess = 0x80000000;

        private const uint FileFlagsForOpenReparsePointAndBackupSemantics = 0x02200000;

        private const int IoctlCommandGetReparsePoint = 0x000900A8;

        private const uint OpenExisting = 0x3;

        private const uint PathNotAReparsePointError = 0x80071126;

        private const uint ShareModeAll = 0x7; // Read, Write, Delete

        private const uint SymLinkTag = 0xA000000C;

        private const int TargetIsAFile = 0;

        private const int TargetIsADirectory = 1;

        public SymbolicLink(IFileSystem fileSystem) {
            _fileSystem = fileSystem;

        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern SafeFileHandle CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, int dwFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            int nInBufferSize,
            IntPtr lpOutBuffer,
            int nOutBufferSize,
            out int lpBytesReturned,
            IntPtr lpOverlapped);

        public void CreateDirectoryLink(string linkPath, string targetPath) {
            if (CreateSymbolicLink(linkPath, targetPath, TargetIsADirectory) && Marshal.GetLastWin32Error() == 0) {
                return;
            }

            try {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            catch (COMException exception) {
                throw new IOException(exception.Message, exception);
            }
        }

        public void CreateFileLink(string linkPath, string targetPath) {
            if (!CreateSymbolicLink(linkPath, targetPath, TargetIsAFile)) {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }

        public bool Exists(string path) {
            if (!Directory.Exists(path) && !File.Exists(path)) {
                return false;
            }

            string target = GetTarget(path);
            return target != null;
        }

        private SafeFileHandle GetFileHandle(string path) {
            return CreateFile(path, GenericReadAccess, ShareModeAll, IntPtr.Zero, OpenExisting,
                              FileFlagsForOpenReparsePointAndBackupSemantics, IntPtr.Zero);
        }

        public string GetTarget(string path) {
            SymbolicLinkReparseData reparseDataBuffer;

            using (SafeFileHandle fileHandle = GetFileHandle(path)) {
                if (fileHandle.IsInvalid) {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }

                int outBufferSize = Marshal.SizeOf(typeof(SymbolicLinkReparseData));
                IntPtr outBuffer = IntPtr.Zero;

                try {
                    outBuffer = Marshal.AllocHGlobal(outBufferSize);
                    int bytesReturned;

                    bool success = DeviceIoControl(
                        fileHandle.DangerousGetHandle(), IoctlCommandGetReparsePoint, IntPtr.Zero, 0,
                        outBuffer, outBufferSize, out bytesReturned, IntPtr.Zero);

                    fileHandle.Close();

                    if (!success) {
                        if (((uint) Marshal.GetHRForLastWin32Error()) == PathNotAReparsePointError) {
                            return null;
                        }

                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                    }

                    reparseDataBuffer = (SymbolicLinkReparseData) Marshal.PtrToStructure(
                        outBuffer, typeof(SymbolicLinkReparseData));
                }
                finally {
                    Marshal.FreeHGlobal(outBuffer);
                }
            }

            if (reparseDataBuffer.ReparseTag != SymLinkTag) {
                return null;
            }

            string target = Encoding.Unicode.GetString(reparseDataBuffer.PathBuffer,
                                                       reparseDataBuffer.PrintNameOffset, reparseDataBuffer.PrintNameLength);

            return target;
        }

    }
    
    /// 
    /// Refer to http://msdn.microsoft.com/en-us/library/windows/hardware/ff552012%28v=vs.85%29.aspx
    /// 
    [StructLayout(LayoutKind.Sequential)]
    internal struct SymbolicLinkReparseData
    {
        // Not certain about this!
        private const int maxUnicodePathLength = 260 * 2;

        public uint ReparseTag;
        public ushort ReparseDataLength;
        public ushort Reserved;
        public ushort SubstituteNameOffset;
        public ushort SubstituteNameLength;
        public ushort PrintNameOffset;
        public ushort PrintNameLength;
        public uint Flags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = maxUnicodePathLength)]
        public byte[] PathBuffer;
    }

}