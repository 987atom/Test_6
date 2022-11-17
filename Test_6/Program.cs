using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;


namespace Test_6
{
    public class RedactFileExtensions
    {
        public static string ext;
        public static string path = "";

        static Dictionary<int, string> toDict(Struct person)
        {
            Dictionary<int, string> text = new Dictionary<int, string>();

            text.Add(0, person.name);
            text.Add(1, person.sex);
            text.Add(2, person.age.ToString());
            text.Add(3, person.species);
            text.Add(4, person.address);
            text.Add(5, person.city);
            text.Add(6, person.state);

            return text;
        }

        static Struct toClass(Dictionary<int, string> text)
        {
            Struct result = new Struct();

            result.name = text[0];
            result.sex = text[1];
            result.age = Convert.ToInt32(text[2]);
            result.species = text[3];
            result.address = text[4];
            result.city = text[5];
            result.state = text[6];


            return result;
        }


        static void Main(string[] args)
        {

            Console.WriteLine("Введите путь до файла: ");

            while (true)
            {

                path = Console.ReadLine();
                try
                {


                    ext = Path.GetExtension(path);

                    switch (ext)
                    {
                        case ".txt":

                            Console.Clear();
                            List<Struct> contxt = new List<Struct>();
                            Dictionary<int, string> texttxt = new Dictionary<int, string>();

                            string[] lines = File.ReadAllLines(path);

                            for (int i = 0; i < lines.Length; i++)
                            {
                                Struct str = new Struct();
                                str.name = lines[i];
                                str.sex = lines[i + 1];
                                str.age = Convert.ToInt32(lines[i + 2]);
                                str.species = lines[i + 3];
                                str.address = lines[i + 4];
                                str.city = lines[i + 5];
                                str.state = lines[i + 6];
                                contxt.Add(str);
                                break;
                            }

                            //foreach (var item in contxt)
                            //{
                            //    Console.WriteLine(item.name);
                            //    Console.WriteLine(item.sex);
                            //    Console.WriteLine(item.age);
                            //    Console.WriteLine(item.species);
                            //    Console.WriteLine(item.streetAddress);
                            //    Console.WriteLine(item.city);
                            //    Console.WriteLine(item.state);
                            //}

                            foreach (var sr in contxt)
                            {
                                texttxt = toDict(sr);
                            }

                            var clDicttxt = toClass(ReadLine(texttxt));

                            break;

                        case ".xml":

                            Console.Clear();
                            XmlSerializer conxml = new XmlSerializer(typeof(Struct));
                            Struct stXML = new Struct();
                            using (FileStream fs = new FileStream(path, FileMode.Open))
                            {
                                stXML = (Struct)conxml.Deserialize(fs);
                            }

                            //foreach (var item in stXML)
                            //{
                            //    Console.WriteLine(item.name);
                            //    Console.WriteLine(item.sex);
                            //    Console.WriteLine(item.age);
                            //    Console.WriteLine(item.species);
                            //    Console.WriteLine(item.streetAddress);
                            //    Console.WriteLine(item.city);
                            //    Console.WriteLine(item.state);
                            //}

                            var d1 = ReadLine(toDict(stXML));

                            break;

                        case ".json":

                            Console.Clear();
                            string a = File.ReadAllText(path);

                            List<Struct> con = JsonConvert.DeserializeObject<List<Struct>>(a);
                            Dictionary<int, string> text = new Dictionary<int, string>();

                            //Console.WriteLine(con[0].name);
                            //Console.WriteLine(con[0].sex);
                            //Console.WriteLine(con[0].age);
                            //Console.WriteLine(con[0].species);

                            //Console.WriteLine(con[0].streetAddress);
                            //Console.WriteLine(con[0].city);
                            //Console.WriteLine(con[0].state);


                            foreach (var st in con)
                            {
                                text = toDict(st);
                            }

                            var clDict = toClass(ReadLine(text));

                            break;


                        default:
                            Console.WriteLine("Нужно использовать форматы: txt, json, xml");
                            break;

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Файл не может быть прочитан");
                    Console.WriteLine(e.Message);
                }
            }
        }

        static Dictionary<int, string> ReadLine(Dictionary<int, string> text)
        {

            int goPos = 0;
            int uppos = 0;
            int maxznach = text.Count - 1;

            int goMax = 0;

            foreach (var i in text.Values)
            {
                Console.WriteLine(i);
            }

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (uppos == 0)
                        {
                            uppos = maxznach;
                        }
                        else
                        {
                            uppos = uppos - 1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (uppos == maxznach)
                        {
                            uppos = 0;
                        }
                        else
                        {
                            uppos = uppos + 1;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (goPos == goMax)
                        {
                            goPos = 0;
                        }
                        else
                        {
                            goPos = goPos + 1;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (goPos <= 0)
                        {
                            goPos = 0;
                        }
                        else
                        {
                            goPos = goPos - 1;
                        }
                        break;
                    case ConsoleKey.Backspace:
                        if (text[uppos].Length != 0)
                        {
                            if (goPos != 0)
                            {
                                text[uppos] = text[uppos].Remove(goPos, 1);
                                goPos = goPos - 1;
                            }

                            else
                            {
                                text[uppos] = text[uppos].Remove(goPos, 1);
                            }
                        }
                        break;

                    case ConsoleKey.F1:
                        saveFile(text);
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        Environment.Exit(0);
                        break;
                }
                Console.Clear();

                foreach (var i in text.Values)
                {
                    Console.WriteLine(i);
                }

                Console.SetCursorPosition(goPos, uppos);
                goMax = text[uppos].Length - 1;
            }//конец cтрелочного меню
            return text;
        }
        static void saveFile(Dictionary<int, string> text)
        {
            try
            {

                switch (ext)
                {
                    case ".txt":

                        String txt = "";
                        for (int i = 0; i < text.Count; i++)
                        {
                            if (txt != "")
                            {
                                txt = txt + "\r\n" + text[i];
                            }
                            else
                            {
                                txt = text[i];
                            }
                        }

                        File.WriteAllText(path, txt);

                        break;

                    case ".xml":

                        Struct st = toClass(text);

                        Struct js = new Struct();
                        using (FileStream fs = new FileStream(path, FileMode.Create))
                        {
                            XmlSerializer xml = new XmlSerializer(typeof(Struct)/*, Formatting.indented*/);
                            
                            xml.Serialize(fs, st);
                        }

                        break;

                    case ".json":

                        string json = JsonConvert.SerializeObject(text/*, Formatting.indented*/);
                        File.WriteAllText(path, json);

                        break;


                    default:

                        break;
                        Console.WriteLine("Файл успешно сохранен");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка при записи файла типа " + ext);
                Console.WriteLine(e.Message);
            }
        }
    }
}