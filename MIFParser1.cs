using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace MiniGIS
{
    public class MIFParser1
    {
        public int Version { get; set; }
        public string Charset { get; set; }
        public char Delimiter { get; set; }
        public List<int> Unique { get; set; }
        public List<int> Index { get; set; }
        public List<int> Transform { get; set; }

        public class Column
        {
            public string Name { get; set; }
            public string Type { get; set; }

            public Column(string name, string type)
            {
                Name = name;
                Type = type;
            }
        }

        public int ColumnsN { get; set; }
        public List<Column> Columns { get; set; }
        public List<MapObject> Data { get; set; }

        public MIFParser1(string layerFilename)
        {
            Version = 300;
            Charset = "";
            Delimiter = '\t';
            Unique = new List<int>();
            Index = new List<int>();
            Transform = new List<int>(4);
            ColumnsN = 0;
            Data = new List<MapObject>();
            string tmpline;

            using (var sr = new StreamReader(layerFilename))
            {
                // Версия
                try { Version = Convert.ToInt32(sr.ReadLine().Split(' ')[1]); }
                catch { Version = 0; }

                // Кодировка
                string charset = sr.ReadLine().Split(' ')[1];
                Charset = charset.Substring(1, charset.Length - 2);

                string[] line = sr.ReadLine().Split(' ');

                // Разделитель
                if (line[0] == "Delimiter")
                {
                    Delimiter = line[1].Substring(1, line[1].Length - 2)[0];
                    line = sr.ReadLine().Split(' ');
                }

                // Уникальная колонка
                if (line[0] == "Unique")
                {
                    string[] uniqueStr = line[1].Split(',');
                    for (int i = 0; i < uniqueStr.Length; ++i)
                    {
                        Unique[i] = Convert.ToInt32(uniqueStr[i]);
                    }
                    line = sr.ReadLine().Split(' ');
                }

                // Индекс
                if (line[0] == "Index")
                {
                    string[] indexStr = line[1].Split(',');
                    for (int i = 0; i < indexStr.Length; ++i)
                    {
                        Index.Add(Convert.ToInt32(indexStr[i]));
                    }
                    line = sr.ReadLine().Split(' ');
                }

                // Преобразование
                if (line[0] == "Transform")
                {
                    string[] transformStr = line[1].Split(',');
                    for (int i = 0; i < transformStr.Length; ++i)
                    {
                        Transform[i] = Convert.ToInt32(transformStr[i]);
                    }
                    line = sr.ReadLine().Split(' ');
                }

                // Колонки
                if (line[0] == "Columns")
                {
                    ColumnsN = Convert.ToInt32(line[1]);
                    Columns = new List<Column>(ColumnsN);
                    for (int i = 0; i < ColumnsN; ++i)
                    {
                        line = sr.ReadLine().Split(' ');
                        var column = new Column(line[0], line[1]);
                    }
                }

                // Слово Data
                sr.ReadLine();

                // Данные
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine().Split(' ');
                    if (line[0] == "Point")
                    {
                        // Координаты точки
                        double x = double.Parse(line[1], CultureInfo.InvariantCulture);
                        double y = double.Parse(line[2], CultureInfo.InvariantCulture);

                        // Symbol
                        //byte symbolByte;
                        int symbolByte;
                        Color color;
                        int symbolSize;
                        string fontFamily = "MapInfo Symbols";
                        if (line.Length == 3)
                        {
                            tmpline = sr.ReadLine().Trim();
                            line = tmpline.Split(',');
                            //symbolByte = Convert.ToByte(line[0].Substring(8, line[0].Length - 8))+Convert.ToByte(1);
                            symbolByte = Convert.ToInt32(line[0].Substring(8, line[0].Length - 8))+1;
                            color = IntToColor(Convert.ToInt32(line[1].Substring(0, line[1].Length)));
                            symbolSize = Convert.ToInt32(line[2].Substring(0, line[2].Length - 1));
                        }
                        else
                        {
                            symbolByte = Convert.ToByte(line[4].Substring(1, line[4].Length - 2));
                            color = IntToColor(Convert.ToInt32(line[5].Substring(0, line[5].Length - 1)));
                            symbolSize = Convert.ToInt32(line[6].Substring(0, line[6].Length - 1));
                            fontFamily = line[7].Substring(1, line[7].Length - 3);
                        }

                        // Добавление точки в основной список
                        var point = new Point(new Vertex(x, y));
                        point.Symbol.Font = new Font(fontFamily, symbolSize);
                        point.Symbol.Number = symbolByte;
                        point.Brush = new SolidBrush(color);
                        point.UseOwnStyle = true;
                        Data.Add(point);
                    }

                    else if (line[0] == "Line")
                    {
                        // Координаты начала и конца линии
                        double x1 = double.Parse(line[1], CultureInfo.InvariantCulture);
                        double y1 = double.Parse(line[2], CultureInfo.InvariantCulture);
                        double x2 = double.Parse(line[3], CultureInfo.InvariantCulture);
                        double y2 = double.Parse(line[4], CultureInfo.InvariantCulture);

                        // Pen
                        line = sr.ReadLine().Replace(" ", string.Empty).Split(',');
                        Pen pen = GetPen(line[0], line[1], line[2]);

                        // Добавление линии в основной список
                        var mapLine = new Line(new Vertex(x1, y1), new Vertex(x2, y2));
                        mapLine.Pen = pen;
                        mapLine.UseOwnStyle = true;
                        Data.Add(mapLine);
                    }

                    else if (line[0] == "Pline")
                    {
                        // Количество полилиний
                        int n;
                        try
                        {
                            n = Convert.ToInt32(line[1]);
                        }
                        catch
                        {
                            n = Convert.ToInt32(sr.ReadLine().Split(' ')[0]);
                        }

                        // Вершины полилинии
                        var polyline = new Polyline();
                        for (int i = 0; i < n; ++i)
                        {
                            line = sr.ReadLine().Split(' ');
                            double x = double.Parse(line[0], CultureInfo.InvariantCulture);
                            double y = double.Parse(line[1], CultureInfo.InvariantCulture);
                            var point = new Vertex(x, y);
                            polyline.AddNode(point);
                        }

                        // Pen
                        line = sr.ReadLine().Replace(" ", string.Empty).Split(',');
                        //float width = float.Parse(line[0].Substring(4, line[0].Length - 4));
                        //Color color = IntToColor(Convert.ToInt32(line[2].Substring(0, line[2].Length - 1)));
                        Pen pen = GetPen(line[0], line[1], line[2]);

                        // Добавление полилинии в основной список
                        polyline.Pen = pen;
                        polyline.UseOwnStyle = true;
                        Data.Add(polyline);
                    }

                    else if (line[0] == "Region")
                    {
                        // Количество регионов
                        int regionsNumb;
                        try { regionsNumb = Convert.ToInt32(line[1]); }
                        catch { regionsNumb = Convert.ToInt32(line[2]); }

                        // Вершины полигонов
                        var polygonsList = new List<Polygon>();
                        for (int i = 0; i < regionsNumb; ++i)
                        {
                            line = sr.ReadLine().Trim().Split(' ');
                            int pointsNumb = Convert.ToInt32(line[0]);
                            var polygon = new Polygon();
                            for (int j = 0; j < pointsNumb; ++j)
                            {
                                line = sr.ReadLine().Split(' ');
                                double x = double.Parse(line[0], CultureInfo.InvariantCulture);
                                double y = double.Parse(line[1], CultureInfo.InvariantCulture);
                                var point = new Vertex(x, y);
                                polygon.AddNode(point);
                            }

                            // Добавление полигонов во временный список
                            polygonsList.Add(polygon);
                        }

                        // Pen
                        line = sr.ReadLine().Replace(" ", string.Empty).Split(',');
                        //float width = float.Parse(line[0].Substring(4, line[0].Length - 4));
                        //Color penColor = IntToColor(Convert.ToInt32(line[2].Substring(0, line[2].Length - 1)));
                        Pen pen = GetPen(line[0], line[1], line[2]);

                        // Brush
                        line = sr.ReadLine().Replace(" ", string.Empty).Split(',');
                        //Color brushColor = IntToColor(Convert.ToInt32(line[2].Substring(0, line[2].Length - 1)));
                        //if (Convert.ToInt32(line[0].Substring(6, line[0].Length - 6)) == 1)
                        //    brushColor = Color.FromArgb(0, brushColor);
                        Brush brush = GetBrush(line[0], line[1], line[2]);

                        // Добавление полигонов в основной список и задание стилей 
                        foreach (var polygon in polygonsList)
                        {
                            polygon.Pen = pen;
                            polygon.Brush = brush;
                            polygon.UseOwnStyle = true;
                            Data.Add(polygon);
                        }
                    }
                }
            }
        }

        private Color IntToColor(int dec)
        {
            byte red = (byte)((dec >> 16) & 0xff);
            byte green = (byte)((dec >> 8) & 0xff);
            byte blue = (byte)(dec & 0xff);
            return (Color.FromArgb(red, green, blue));
        }
        private Color IntToColor(int alpha, int dec)
        {
            byte red = (byte)((dec >> 16) & 0xff);
            byte green = (byte)((dec >> 8) & 0xff);
            byte blue = (byte)(dec & 0xff);
            return (Color.FromArgb(alpha, red, green, blue));
        }

        private Pen GetPen(string w, string p, string c)
        {
            float width = float.Parse(w.Substring(4, w.Length - 4));
            int pattern = int.Parse(p);
            Color color = IntToColor(Convert.ToInt32(c.Substring(0, c.Length - 1)));
            Pen pen = new Pen(color, width);
            switch (pattern)
            {
                case 1: pen.Color = Color.FromArgb(0, color); break;
                case 2: pen.DashStyle = DashStyle.Solid; break;
                case 3: case 4: case 5: case 10: pen.DashStyle = DashStyle.Dot; break;
                case 6: case 9: pen.DashStyle = DashStyle.Dash; pen.DashPattern = new float[] { 2, 1}; break;
                case 7: pen.DashStyle = DashStyle.Dash; pen.DashPattern = new float[] { 3, 1 }; break;
                case 8: pen.DashStyle = DashStyle.Dash; pen.DashPattern = new float[] { 4, 1 }; break;
                case 11: pen.DashStyle = DashStyle.Dash; pen.DashPattern = new float[] { 1, 2 }; break;
                case 12: pen.DashStyle = DashStyle.Dash; pen.DashPattern = new float[] { 3, 3 }; break;
                case 13: pen.DashStyle = DashStyle.Dash; pen.DashPattern = new float[] { 5, 5 }; break;
                case 14: pen.DashStyle = DashStyle.DashDot; pen.DashPattern = new float[] { 4, 2, 1, 2}; break;
                case 15: pen.DashStyle = DashStyle.DashDot; pen.DashPattern = new float[] { 6, 2, 1, 2 }; break;
                case 16: pen.DashStyle = DashStyle.DashDot; pen.DashPattern = new float[] { 7, 2, 2, 2 }; break;
                case 17: pen.DashStyle = DashStyle.DashDot; pen.DashPattern = new float[] { 8, 2, 2, 2 }; break;
                case 18: case 19: pen.DashStyle = DashStyle.DashDotDot; pen.DashPattern = new float[] { 8, 2, 2, 2, 2, 2 }; break;
                case 20: pen.DashStyle = DashStyle.DashDotDot; pen.DashPattern = new float[] { 4, 2, 1, 2, 1, 2 }; break;
                case 21: case 22: case 23: case 24: case 25: pen.DashStyle = DashStyle.DashDotDot; pen.DashPattern = new float[] { 6, 2, 1, 2, 1, 2 }; break;
                default: break;
            }
            return pen;
        }

        private Brush GetBrush(string p, string fc, string bc = "")
        {
            Color foreColor = IntToColor(Convert.ToInt32(fc));
            Color backColor = IntToColor(Convert.ToInt32(bc.Substring(0, bc.Length - 1)));
            int pattern = Convert.ToInt32(p.Substring(6, p.Length - 6));
            Brush brush;
            switch (pattern)
            {
                case 1: brush = new SolidBrush(Color.FromArgb(0, 0, 0, 0)); break;
                case 2: brush = new SolidBrush(foreColor); break;
                case 3: case 19: case 20: case 21: case 22: case 23: brush = new HatchBrush(HatchStyle.Horizontal, foreColor, backColor); break;
                case 4: case 24: case 25: case 26: case 27: case 28: brush = new HatchBrush(HatchStyle.Vertical, foreColor, backColor); break;
                case 5: case 29: case 30: case 31: case 32: case 33: brush = new HatchBrush(HatchStyle.BackwardDiagonal, foreColor, backColor); break;
                case 6: case 34: case 35: case 36: case 37: case 38: brush = new HatchBrush(HatchStyle.ForwardDiagonal, foreColor, backColor); break;
                case 7: case 39: case 40: case 41: case 42: case 43: brush = new HatchBrush(HatchStyle.Cross, foreColor, backColor); break;
                case 8: case 44: case 45: case 46: case 47: brush = new HatchBrush(HatchStyle.DiagonalCross, foreColor, backColor); break;
                case 12: case 13: case 14: case 15: brush = new HatchBrush(HatchStyle.Percent90, foreColor, backColor); break;
                case 16: case 17: case 18: case 49: case 50: case 51: case 52: case 53: brush = new HatchBrush(HatchStyle.Percent10, foreColor, backColor); break;
                default: brush = new HatchBrush(HatchStyle.Cross, foreColor, backColor); break;
            }
            return brush;
        }
    }
}
