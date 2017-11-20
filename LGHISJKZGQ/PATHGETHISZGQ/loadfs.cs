using System;
using System.Collections.Generic;
using System.Text;
using System.IO; // ���ļ��Ķ�д��Ҫ�õ��������ռ� 

using System.Reflection; // ʹ�� Assembly �����ô������ռ� 

using System.Reflection.Emit;
 


namespace LGHISJKZGQ
{
    class loadfs
    {
        static Assembly MyAssembly;
        private byte[] LoadDll(string lpFileName)
        {

           //Assembly NowAssembly = Assembly.GetEntryAssembly();

            Assembly NowAssembly = Assembly.GetExecutingAssembly();

            Stream fs = null;

            try
            {// ���Զ�ȡ��Դ�е� DLL 

                    fs = Assembly.GetExecutingAssembly().GetManifestResourceStream("LGHISJK"+"."+lpFileName);                 
                

            }

            finally
            {// �����Դû������� DLL ���Ͳ鿴Ӳ������û�У��еĻ��Ͷ�ȡ 

                if (fs == null && !File.Exists(lpFileName)) throw (new Exception(" �Ҳ����ļ� :" + lpFileName));

                else if (fs == null && File.Exists(lpFileName))
                {

                    FileStream Fs = new FileStream(lpFileName, FileMode.Open);

                    fs = (Stream)Fs;

                }

            }

            byte[] buffer = new byte[(int)fs.Length];

            fs.Read(buffer, 0, buffer.Length);

            fs.Close();

            return buffer; // �� byte[] ���ض����� DLL 

        }
        public void UnLoadDll()
        {// ʹ MyAssembly ָ�� 

            MyAssembly = null;

        }
        public object Invoke(string lpFileName, string Namespace, string ClassName, string lpProcName, object[] ObjArray_Parameter)
        {

            try
            {// �ж� MyAssembly �Ƿ�Ϊ�ջ� MyAssembly �������ռ䲻����Ҫ���÷����������ռ䣬�������Ϊ�棬���� Assembly.Load �������� DLL ��Ϊ���� 

                if (MyAssembly == null || MyAssembly.GetName().Name != Namespace)

                    MyAssembly = Assembly.Load(LoadDll(lpFileName));

                Type[] type = MyAssembly.GetTypes();

                foreach (Type t in type)
                {

                    if (t.Namespace == Namespace && t.Name == ClassName)
                    {

                        MethodInfo m = t.GetMethod(lpProcName);

                        if (m != null)
                        {// ���ò����� 

                            object o = Activator.CreateInstance(t);

                            return m.Invoke(o, ObjArray_Parameter);

                        }

                        else

                            System.Windows.Forms.MessageBox.Show(" װ�س��� !");

                    }

                }

            }

            catch (System.NullReferenceException e)
            {

                System.Windows.Forms.MessageBox.Show(e.Message);

            }

            return (object)0;

        } 


    }
}
