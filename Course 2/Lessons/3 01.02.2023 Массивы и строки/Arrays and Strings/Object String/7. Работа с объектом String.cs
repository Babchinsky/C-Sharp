using System;
namespace CSharp.String
{
    class MainClass
    {
        static void Main()
        {
            string s = "��� ������� ������";
            Console.WriteLine(s);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetWindowSize(50, 30);
            string result = string.Format("����� ������: {0}", s.Length);
            Console.WriteLine(result);

            string t = s.Substring(4, 7); // ���������� ��������� �� 7 ��������, ������� � 4 �������
            Console.WriteLine(t);
            Console.WriteLine(s[5]);

            s = s.Replace("�", "�");
            Console.WriteLine(s);

            s = s.Remove(4, 8); // ������� 8 ��������, ������� � ������� 4
            Console.WriteLine(s);

            //str[5] = '!'; // �����������: ������ ������ �� ������
            char[] ar = { '�', '�', '�', '�', '�', '�' };

            string s2 = new string(ar);
            Console.WriteLine(s2);
            Console.WriteLine("����� ������: " + s2.Length);

            string[] arstr = { " Platform ", " .NET ", " Framework " };
            for (int i = 0; i < arstr.Length; i++)
            {
                Console.WriteLine(arstr[i]);
            }
            arstr[0] = " Common ";
            arstr[1] = " Language ";
            arstr[2] = " Runtime ";
            for (int i = 0; i < arstr.Length; i++)
            {
                Console.WriteLine(arstr[i]);
            }

            Console.WriteLine("������� ������ ������: ");
            string s3 = Console.ReadLine();
            Console.WriteLine("������� ������ ������: ");
            string s4 = Console.ReadLine();
            if (s3.CompareTo(s4) > 0)
                Console.WriteLine(s3 + " ������, ��� " + s4);
            else if (s3.CompareTo(s4) < 0)
                Console.WriteLine(s4 + " ������, ��� " + s3);
            else
                Console.WriteLine("������ �����");

            string text = "To be or not be";
            Console.WriteLine(" ������ ������� ��������� ����� \"be\" = {0} ", text.IndexOf("be"));
            Console.WriteLine(" ������ ���������� ��������� ����� \"be\" = {0} ", text.LastIndexOf("be"));

            Console.WriteLine();
            string words = "���    ������  �����  �������  ��  ������  �����";
            string[] arrayOfString = words.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s5 in arrayOfString)
            {
                Console.WriteLine(s5);
            }
            string res = string.Join(" ", arrayOfString);
            Console.WriteLine(res);

            string str1 = "� ";
            string str2 = "��� ";
            string str3 = "C#";
            string str4 = str1 + str2 + str3;

            Console.WriteLine("{0} + {1} + {2} = {3}", str1, str2, str3, str4);

            str4 = str4.Replace("���", "������");
            Console.WriteLine(str4);

            str4 = str4.Insert(2, "������ ").ToUpper();
            Console.WriteLine(str4);

            if (str4.Contains("������"))
                Console.WriteLine("��� ���� ������ :)");
            else
                Console.WriteLine("��� ��� ����");
        }
    }
}