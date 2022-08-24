using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Media.Imaging;

namespace RZFileExplorer.Icons {
    public static class ShellEx {
        // Constants that we need in the function call

        private const int SHGFI_ICON = 0x100;

        private const int SHGFI_SMALLICON = 0x1;

        private const int SHGFI_LARGEICON = 0x0;

        private const int SHIL_JUMBO = 0x4;
        private const int SHIL_EXTRALARGE = 0x2;

        // This structure will contain information about the file

        public struct SHFILEINFO {

            // Handle to the icon representing the file

            public IntPtr hIcon;

            // Index of the icon within the image list

            public int iIcon;

            // Various attributes of the file

            public uint dwAttributes;

            // Path to the file

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]

            public string szDisplayName;

            // File type

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]

            public string szTypeName;

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("kernel32.dll", SetLastError=true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        private struct IMAGELISTDRAWPARAMS {
            public int cbSize;
            public IntPtr himl;
            public int i;
            public IntPtr hdcDst;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int xBitmap; // x offest from the upperleft of bitmap
            public int yBitmap; // y offset from the upperleft of bitmap
            public int rgbBk;
            public int rgbFg;
            public int fStyle;
            public int dwRop;
            public int fState;
            public int Frame;
            public int crEffect;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IMAGEINFO {
            public IntPtr hbmImage;
            public IntPtr hbmMask;
            public int Unused1;
            public int Unused2;
            public RECT rcImage;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT {
            public int X;
            public int Y;

            public POINT(int x, int y) {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator System.Drawing.Point(POINT p) {
                return new System.Drawing.Point(p.X, p.Y);
            }

            public static implicit operator POINT(System.Drawing.Point p) {
                return new POINT(p.X, p.Y);
            }
        }

        #region Private ImageList COM Interop (XP)

        [ComImportAttribute()]
        [GuidAttribute("46EB5926-582E-4017-9FDF-E8998DAA0950")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        //helpstring("Image List"),
        interface IImageList {
            [PreserveSig] int Add(IntPtr hbmImage, IntPtr hbmMask, ref int pi);
            [PreserveSig] int ReplaceIcon(int i, IntPtr hicon, ref int pi);
            [PreserveSig] int SetOverlayImage(int iImage, int iOverlay);
            [PreserveSig] int Replace(int i, IntPtr hbmImage, IntPtr hbmMask);
            [PreserveSig] int AddMasked(IntPtr hbmImage, int crMask, ref int pi);
            [PreserveSig] int Draw(ref IMAGELISTDRAWPARAMS pimldp);
            [PreserveSig] int Remove(int i);
            [PreserveSig] int GetIcon(int i, int flags, ref IntPtr picon);
            [PreserveSig] int GetImageInfo(int i, ref IMAGEINFO pImageInfo);
            [PreserveSig] int Copy(int iDst, IImageList punkSrc, int iSrc, int uFlags);
            [PreserveSig] int Merge(int i1, IImageList punk2, int i2, int dx, int dy, ref Guid riid, ref IntPtr ppv);
            [PreserveSig] int Clone(ref Guid riid, ref IntPtr ppv);
            [PreserveSig] int GetImageRect(int i, ref RECT prc);
            [PreserveSig] int GetIconSize(ref int cx, ref int cy);
            [PreserveSig] int SetIconSize(int cx, int cy);
            [PreserveSig] int GetImageCount(ref int pi);
            [PreserveSig] int SetImageCount(int uNewCount);
            [PreserveSig] int SetBkColor(int clrBk, ref int pclr);
            [PreserveSig] int GetBkColor(ref int pclr);
            [PreserveSig] int BeginDrag(int iTrack, int dxHotspot, int dyHotspot);
            [PreserveSig] int EndDrag();
            [PreserveSig] int DragEnter(IntPtr hwndLock, int x, int y);
            [PreserveSig] int DragLeave(IntPtr hwndLock);
            [PreserveSig] int DragMove(int x, int y);
            [PreserveSig] int SetDragCursorImage(ref IImageList punk, int iDrag, int dxHotspot, int dyHotspot);
            [PreserveSig] int DragShowNolock(int fShow);
            [PreserveSig] int GetDragImage(ref POINT ppt, ref POINT pptHotspot, ref Guid riid, ref IntPtr ppv);
            [PreserveSig] int GetItemFlags(int i, ref int dwFlags);
            [PreserveSig] int GetOverlayImage(int iOverlay, ref int piIndex);
        }

        public enum CSIDL {
            CSIDL_ADMINTOOLS        = 0x0030,
            CSIDL_ALTSTARTUP        = 0x001d,
            CSIDL_APPDATA           = 0x001a,
            CSIDL_BITBUCKET         = 0x000a,
            CSIDL_CDBURN_AREA           = 0x003b,
            CSIDL_COMMON_ADMINTOOLS     = 0x002f,
            CSIDL_COMMON_ALTSTARTUP     = 0x001e,
            CSIDL_COMMON_APPDATA        = 0x0023,
            CSIDL_COMMON_DESKTOPDIRECTORY   = 0x0019,
            CSIDL_COMMON_DOCUMENTS      = 0x002e,
            CSIDL_COMMON_FAVORITES      = 0x001f,
            CSIDL_COMMON_MUSIC          = 0x0035,
            CSIDL_COMMON_OEM_LINKS      = 0x003a,
            CSIDL_COMMON_PICTURES       = 0x0036,
            CSIDL_COMMON_PROGRAMS       = 0X0017,
            CSIDL_COMMON_STARTMENU      = 0x0016,
            CSIDL_COMMON_STARTUP        = 0x0018,
            CSIDL_COMMON_TEMPLATES      = 0x002d,
            CSIDL_COMMON_VIDEO          = 0x0037,
            CSIDL_COMPUTERSNEARME       = 0x003d,
            CSIDL_CONNECTIONS           = 0x0031,
            CSIDL_CONTROLS          = 0x0003,
            CSIDL_COOKIES           = 0x0021,
            CSIDL_DESKTOP           = 0x0000,
            CSIDL_DESKTOPDIRECTORY      = 0x0010,
            CSIDL_DRIVES            = 0x0011,
            CSIDL_FAVORITES         = 0x0006,
            CSIDL_FLAG_CREATE           = 0x8000,
            CSIDL_FLAG_DONT_VERIFY      = 0x4000,
            CSIDL_FLAG_MASK         = 0xFF00,
            CSIDL_FLAG_NO_ALIAS         = 0x1000,
            CSIDL_FLAG_PER_USER_INIT    = 0x0800,
            CSIDL_FONTS             = 0x0014,
            CSIDL_HISTORY           = 0x0022,
            CSIDL_INTERNET          = 0x0001,
            CSIDL_INTERNET_CACHE        = 0x0020,
            CSIDL_LOCAL_APPDATA         = 0x001c,
            CSIDL_MYDOCUMENTS           = 0x000c,
            CSIDL_MYMUSIC           = 0x000d,
            CSIDL_MYPICTURES        = 0x0027,
            CSIDL_MYVIDEO           = 0x000e,
            CSIDL_NETHOOD           = 0x0013,
            CSIDL_NETWORK           = 0x0012,
            CSIDL_PERSONAL          = 0x0005,
            CSIDL_PRINTERS          = 0x0004,
            CSIDL_PRINTHOOD         = 0x001b,
            CSIDL_PROFILE           = 0x0028,
            CSIDL_PROGRAM_FILES         = 0x0026,
            CSIDL_PROGRAM_FILES_COMMON      = 0x002b,
            CSIDL_PROGRAM_FILES_COMMONX86   = 0x002c,
            CSIDL_PROGRAM_FILESX86      = 0x002a,
            CSIDL_PROGRAMS          = 0x0002,
            CSIDL_RECENT            = 0x0008,
            CSIDL_RESOURCES         = 0x0038,
            CSIDL_RESOURCES_LOCALIZED       = 0x0039,
            CSIDL_SENDTO            = 0x0009,
            CSIDL_STARTMENU         = 0x000b,
            CSIDL_STARTUP           = 0x0007,
            CSIDL_SYSTEM            = 0x0025,
            CSIDL_SYSTEMX86         = 0x0029,
            CSIDL_TEMPLATES         = 0x0015,
            CSIDL_WINDOWS           = 0x0024
        }

        #endregion

        ///
        /// SHGetImageList is not exported correctly in XP.  See KB316931
        /// http://support.microsoft.com/default.aspx?scid=kb;EN-US;Q316931
        /// Apparently (and hopefully) ordinal 727 isn't going to change.
        ///
        [DllImport("shell32.dll", EntryPoint = "#727")]
        private extern static int SHGetImageList(int iImageList, ref Guid riid, out IImageList ppv);

        // The signature of SHGetFileInfo (located in Shell32.dll)
        [DllImport("Shell32.dll")]
        public static extern int SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, uint uFlags);

        [DllImport("Shell32.dll")]
        public static extern int SHGetFileInfo(IntPtr pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, uint uFlags);

        [DllImport("shell32.dll", SetLastError = true)]
        static extern int SHGetSpecialFolderLocation(
            IntPtr hwndOwner,
            Int32 nFolder,
            ref IntPtr ppidl);

        [DllImport("user32")]
        public static extern int DestroyIcon(IntPtr hIcon);

        public struct pair {
            public System.Drawing.Icon icon { get; set; }
            public IntPtr iconHandleToDestroy { set; get; }

        }

        public static int DestroyIcon2(IntPtr hIcon) {
            return DestroyIcon(hIcon);
        }

        private static BitmapSource bitmap_source_of_icon(System.Drawing.Icon ic) {
            BitmapSource ic2 = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(ic.Handle,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            ic2.Freeze();
            return (BitmapSource) ic2;
        }

        public static BitmapSource SystemIcon(bool small, CSIDL csidl) {

            IntPtr pidlTrash = IntPtr.Zero;
            int hr = SHGetSpecialFolderLocation(IntPtr.Zero, (int) csidl, ref pidlTrash);
            Debug.Assert(hr == 0);

            SHFILEINFO shinfo = new SHFILEINFO();

            uint SHGFI_USEFILEATTRIBUTES = 0x000000010;

            // Get a handle to the large icon
            uint flags;
            uint SHGFI_PIDL = 0x000000008;
            if (!small) {
                flags = SHGFI_PIDL | SHGFI_ICON | SHGFI_LARGEICON | SHGFI_USEFILEATTRIBUTES;
            }
            else {
                flags = SHGFI_PIDL | SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES;
            }

            int res = SHGetFileInfo(pidlTrash, 0, ref shinfo, Marshal.SizeOf(shinfo), flags);
            Debug.Assert(res != 0);

            Icon myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);
            Marshal.FreeCoTaskMem(pidlTrash);
            BitmapSource bs = bitmap_source_of_icon(myIcon);
            myIcon.Dispose();
            bs.Freeze(); // importantissimo se no fa memory leak
            DestroyIcon(shinfo.hIcon);
            CloseHandle(shinfo.hIcon);
            return bs;

        }

        public static BitmapSource icon_of_path(string FileName, bool small, bool checkDisk, bool addOverlay) {
            SHFILEINFO shinfo = new SHFILEINFO();

            uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
            uint SHGFI_LINKOVERLAY = 0x000008000;

            uint flags;
            if (small) {
                flags = SHGFI_ICON | SHGFI_SMALLICON;
            }
            else {
                flags = SHGFI_ICON | SHGFI_LARGEICON;
            }

            if (!checkDisk)
                flags |= SHGFI_USEFILEATTRIBUTES;
            if (addOverlay)
                flags |= SHGFI_LINKOVERLAY;

            int res = SHGetFileInfo(FileName, 0, ref shinfo, Marshal.SizeOf(shinfo), flags);
            if (res == 0) {
                throw new System.IO.FileNotFoundException();
            }

            Icon myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);

            BitmapSource bs = bitmap_source_of_icon(myIcon);
            myIcon.Dispose();
            bs.Freeze(); // importantissimo se no fa memory leak
            DestroyIcon(shinfo.hIcon);
            CloseHandle(shinfo.hIcon);
            return bs;

        }

        public static BitmapSource icon_of_path_large(string FileName, bool jumbo, bool checkDisk) {

            SHFILEINFO shinfo = new SHFILEINFO();

            const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
            const uint SHGFI_SYSICONINDEX = 0x4000;

            const int FILE_ATTRIBUTE_NORMAL = 0x80;

            uint flags = SHGFI_SYSICONINDEX;

            if (!checkDisk) // This does not seem to work. If I try it, a folder icon is always returned.
                flags |= SHGFI_USEFILEATTRIBUTES;

            int res = SHGetFileInfo(FileName, FILE_ATTRIBUTE_NORMAL, ref shinfo, Marshal.SizeOf(shinfo), flags);
            if (res == 0) {
                throw new System.IO.FileNotFoundException();
            }

            int iconIndex = shinfo.iIcon;

            // Get the System IImageList object from the Shell:
            Guid iidImageList = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");

            IImageList iml;
            int size = jumbo ? SHIL_JUMBO : SHIL_EXTRALARGE;
            int hres = SHGetImageList(size, ref iidImageList, out iml); // writes iml
            //if (hres == 0)
            //{
            //    throw (new System.Exception("Error SHGetImageList"));
            //}

            IntPtr hIcon = IntPtr.Zero;
            int ILD_TRANSPARENT = 1;
            hres = iml.GetIcon(iconIndex, ILD_TRANSPARENT, ref hIcon);
            //if (hres == 0)
            //{
            //    throw (new System.Exception("Error iml.GetIcon"));
            //}

            Icon myIcon = System.Drawing.Icon.FromHandle(hIcon);
            BitmapSource bs = bitmap_source_of_icon(myIcon);
            myIcon.Dispose();
            bs.Freeze(); // very important to avoid memory leak
            DestroyIcon(hIcon);

            try {
                CloseHandle(hIcon);
            }
            catch {
                int error = Marshal.GetLastWin32Error();
                if (error != 0) {
                    throw new Win32Exception(error);
                }
            }

            return bs;

        }
    }
}