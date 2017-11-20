using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace PathnetCAzgq
{
    class loaddll
    {
       [DllImport("kernel32.dll")]      
       public extern static IntPtr LoadLibrary(string path);      
      
       [DllImport("kernel32.dll")]      
       public extern static IntPtr GetProcAddress(IntPtr lib, string funcName);      
      
       [DllImport("kernel32.dll")]      
       public extern static bool FreeLibrary(IntPtr lib);      
      
       [DllImport("kernel32.dll")]      
       public static extern IntPtr GetStdHandle(int nStdHandle);      
      
       [DllImport("user32", EntryPoint = "CallWindowProc")]      
       public static extern int CallWindowProc(IntPtr lpPrevWndFunc, int hwnd, int MSG, int wParam, int lParam);

    }
    public class LoadDllapi      
   {      
       IntPtr DllLib;//DLL文件名柄     
       #region 构造函数      
       public LoadDllapi()      
       { }      
       public LoadDllapi(string dllpath)      
       {      
           DllLib = loaddll.LoadLibrary(dllpath);      
       }     
       #endregion      
       /// <summary>      
       /// 析构函数      
       /// </summary>      
       ~LoadDllapi()      
       {      
           loaddll.FreeLibrary(DllLib);//释放名柄      
       }
        public void freeLoadDll()
        {
            
            loaddll.FreeLibrary(DllLib);//释放名柄      
            DllLib = IntPtr.Zero;
        }
       public IntPtr initPath(string dllpath)      
       {      
           if (DllLib == IntPtr.Zero)      
           {      
               DllLib = loaddll.LoadLibrary(dllpath);      
           }
           return DllLib;
       }      
       /// <summary>      
       /// 获取ＤＬＬ中一个方法的委托      
       /// </summary>      
       /// <param name="methodname"></param>      
       /// <param name="methodtype"></param>      
       /// <returns></returns>      
       public Delegate InvokeMethod(string methodname, Type methodtype)      
       {      
           IntPtr MethodPtr = loaddll.GetProcAddress(DllLib, methodname);      
                
           return (Delegate)Marshal.GetDelegateForFunctionPointer(MethodPtr, methodtype);      
       }      
   }     


}
