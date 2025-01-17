﻿using System;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;

namespace Gallery.Model
{
    public class Photo
    {
        private string _path;
        private Uri _source;
        private BitmapFrame _image;

        public Photo(string path)
        {
            _path = path;
            _source = new Uri(path);
            _image = BitmapFrame.Create(_source);
        }

        public Photo(BitmapFrame image)
        {
            _path = Environment.CurrentDirectory + "\\" + DateTime.Now.ToString(new CultureInfo("en-US")) + ".jpg";
            _source = new Uri(_path);
            _image = image;
        }

        public override string ToString()
        {
            return _source.ToString();
        }

        public string Source { get { return _path; } }

        public BitmapFrame Image { get { return _image; } set { _image = value; } }

        public string FileName
        {
            get { return System.IO.Path.GetFileName(_path); }
        }

        public long SizeInBytes
        {
            get { return new FileInfo(_path).Length; }
        }

        public string Author
        {
            get { return "Unknown"; } // Замените на соответствующую логику получения информации об авторе
        }
    }


}
