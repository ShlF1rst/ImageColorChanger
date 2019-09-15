using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;






namespace ShadesOfGray
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // кнопка Открыть
        private void openButton_Click(object sender, EventArgs e)
        {
            // диалог для выбора файла
            OpenFileDialog ofd = new OpenFileDialog();
            // фильтр форматов файлов
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            // если в диалоге была нажата кнопка ОК
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // загружаем изображение
                    pictureBox1.Image = new Bitmap(ofd.FileName);
                }
                catch // в случае ошибки выводим MessageBox
                {
                    MessageBox.Show("Невозможно открыть выбранный файл", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

       

// кнопка Ч/Б
private void grayButton_Click(object sender, EventArgs e)
{
    if (pictureBox1.Image != null) // если изображение в pictureBox1 имеется
    {
        RGBHSV.lastBut = 'g';
        // создаём Bitmap из изображения, находящегося в pictureBox1
        Bitmap input = new Bitmap(pictureBox1.Image);
        
        Bitmap output1 = new Bitmap(input.Width, input.Height);
        Bitmap output2 = new Bitmap(input.Width, input.Height);
        Bitmap output3 = new Bitmap(input.Width, input.Height);
        Bitmap output4 = new Bitmap(512, pictureBox5.Height);

        int[] gist = new int[256];

        // перебираем в циклах все пиксели исходного изображения
        for (int j = 0; j < input.Height; j++)
            for (int i = 0; i < input.Width; i++)
            {
                // получаем (i, j) пиксель
                UInt32 pixel = (UInt32)(input.GetPixel(i, j).ToArgb());
                // получаем компоненты цветов пикселя
                float R = (float)((pixel & 0x00FF0000) >> 16); // красный
                float G = (float)((pixel & 0x0000FF00) >> 8); // зеленый
                float B = (float)(pixel & 0x000000FF); // синий
                
                float R1, R2, G1, G2, B1, B2;
                R1 = G1 = B1 = (R + G + B) / 3.0f;
                R2 = G2 = B2 = 0.2126f * R + 0.7152f * G + 0.0722f * B;
                // собираем новый пиксель по частям (по каналам)
                UInt32 newPixel1 = 0xFF000000 | ((UInt32)R1 << 16) | ((UInt32)G1 << 8) | ((UInt32)B1);
                // добавляем его в Bitmap нового изображения
                output1.SetPixel(i, j, Color.FromArgb((int)newPixel1));

                UInt32 newPixel2 = 0xFF000000 | ((UInt32)R2 << 16) | ((UInt32)G2 << 8) | ((UInt32)B2);
                
                output2.SetPixel(i, j, Color.FromArgb((int)newPixel2));

                gist[(int)R2]++;

                output3.SetPixel(i, j, Color.FromArgb((int)(newPixel1-newPixel2)));
            }
                
            label2.Text = "По формуле с равными весами";
            label3.Text = "По формуле с разными вкладами R,G,B";
            label4.Text = "Разность полученных изображений";

            pictureBox2.Image = output1;
            pictureBox3.Image = output2;
            pictureBox4.Image = output3;
    
        
            double point= (double)gist.Max() / pictureBox5.Height;
            for (int i=0; i<512;i+=2)
            {
                for (int j = pictureBox5.Height - 1; (j > pictureBox5.Height - (gist[i/2] / point)) && (j >= 0); --j)
                {
                        output4.SetPixel(i, j, Color.Gray);
                        output4.SetPixel(i+1, j, Color.Gray);
                }
            }

            label5.Text = "Гистограмма интенсивности";
            pictureBox5.Image = output4;
        

     }
}

        private void RGB_button_Click(object sender, EventArgs e)
        {
            RGBHSV.lastBut = 'g';
            if (pictureBox1.Image != null) // если изображение в pictureBox1 имеется
            {
                
                Bitmap input = new Bitmap(pictureBox1.Image);
                
                Bitmap output1 = new Bitmap(input.Width, input.Height);
                Bitmap output2 = new Bitmap(input.Width, input.Height);
                Bitmap output3 = new Bitmap(input.Width, input.Height);
                Bitmap output4 = new Bitmap(512, pictureBox5.Height);


                int[] Rg = new int[256];
                int[] Gg = new int[256];
                int[] Bg = new int[256];

                for (int j = 0; j < input.Height; j++)
                    for (int i = 0; i < input.Width; i++)
                    {
                        // получаем (i, j) пиксель
                        UInt32 pixel = (UInt32)(input.GetPixel(i, j).ToArgb());
                        // получаем компоненты цветов пикселя
                        float R = (float)((pixel & 0x00FF0000) >> 16); // красный
                        float G = (float)((pixel & 0x0000FF00) >> 8); // зеленый
                        float B = (float)(pixel & 0x000000FF); // синий
                                                               
                        float R1, R2, R3, G1, G2, G3, B1, B2, B3;
                        R1 = R;
                        G1 = B1 = 0f;
                        G2 = G;
                        R2 = B2 = 0f;
                        B3 = B;
                        R3 = G3 = 0f;

                        ++Rg[(int)R];
                        ++Gg[(int)G];
                        ++Bg[(int)B];

                        // собираем новый пиксель по частям (по каналам)
                        UInt32 newPixel1 = 0xFF000000 | ((UInt32)R1 << 16) | ((UInt32)G1 << 8) | ((UInt32)B1);
                        // добавляем его в Bitmap нового изображения
                        output1.SetPixel(i, j, Color.FromArgb((int)newPixel1));

                        UInt32 newPixel2 = 0xFF000000 | ((UInt32)R2 << 16) | ((UInt32)G2 << 8) | ((UInt32)B2);
                        
                        output2.SetPixel(i, j, Color.FromArgb((int)newPixel2));

                        UInt32 newPixel3 = 0xFF000000 | ((UInt32)R3 << 16) | ((UInt32)G3 << 8) | ((UInt32)B3);
                        
                        output3.SetPixel(i, j, Color.FromArgb((int)newPixel3));

                    }

                label2.Text = "Красный канал";
                label3.Text = "Зеленый канал";
                label4.Text = "Синий канал";

                pictureBox2.Image = output1;
                pictureBox3.Image = output2;
                pictureBox4.Image = output3;



                Color gistColor = Color.Blue;
                int[] Fg = new int[256];
                if (radioButtonRed.Checked)
                {
                    Fg = Rg;
                    gistColor = Color.Red;
                }
                else if (radioButtonGreen.Checked)
                {
                    Fg = Gg;
                    gistColor = Color.Green;
                }
                else
                    Fg = Bg;

                double point = Fg.Max()/ pictureBox5.Height;
                for (int i = 0; i < 512; i += 2)
                {
                    for (int j = (pictureBox5.Height - 1); (j > pictureBox5.Height - (Fg[i / 2] / (point))) && (j>=0); --j)
                    {
                        output4.SetPixel(i, j, gistColor);
                        output4.SetPixel(i + 1, j, gistColor);
                    }
                }

                label5.Text = "Гистограмма по цвету";
                pictureBox5.Image = output4;

            }
        }

        private void ButtonHSV_Click(object sender, EventArgs e)
        {
            RGBHSV.lastBut = 'h';
            if (pictureBox1.Image != null) // если изображение в pictureBox1 имеется
            {
                Bitmap input = new Bitmap(pictureBox1.Image);
               
                Bitmap output1 = new Bitmap(input.Width, input.Height);
                Bitmap output2 = new Bitmap(input.Width, input.Height);
                Bitmap output3 = new Bitmap(input.Width, input.Height);
                Bitmap output4 = new Bitmap(input.Width, input.Height);


                for (int j = 0; j < input.Height; j++)
                    for (int i = 0; i < input.Width; i++)
                    {

                        Color imgColor = input.GetPixel(i, j);
                        double hue = 0;
                        double saturation = 0;
                        double value = 0;

                        RGBHSV.ColorToHSV(imgColor, out hue, out saturation, out value);


                        imgColor = RGBHSV.HsvToRgb(360, saturation, value);
                        output2.SetPixel(i, j, imgColor);

                        imgColor = RGBHSV.HsvToRgb(hue, 1, value);
                        output3.SetPixel(i, j, imgColor);

                        imgColor = RGBHSV.HsvToRgb(hue, saturation, 1);
                        output4.SetPixel(i, j, imgColor);

                        hue += (double)trackBarH.Value;
                        saturation += (double)trackBarS.Value/100;
                        value += (double)trackBarV.Value/100;

                        if (hue > 360)
                            hue = 360;
                        if (hue < 0)
                            hue = 0;

                     


                        if (saturation > 1)
                            saturation = 1;
                        if (saturation < 0)
                            saturation = 0;

                        if (value > 1)
                            value = 1;
                        if (value < 0)
                            value = 0;

                        imgColor = RGBHSV.HsvToRgb(hue, saturation, value);
                        output1.SetPixel(i, j, imgColor);

                   

                    }
                label2.Text = "Изображение с максимальным тоном";
                label3.Text = "Изображение с максимальной насыщенностью";
                label4.Text = "Изображение с максимальной яркостью";
                label5.Text = "Полученное изображение";

                pictureBox5.Image = output1;
                pictureBox2.Image = output2;
                pictureBox3.Image = output3;
                pictureBox4.Image = output4;

            }
        }

        private void HSVReset_Click(object sender, EventArgs e)
        {
            trackBarH.Value = 0;
            trackBarS.Value = 0;
            trackBarV.Value = 0;
        }

        // кнопка Сохранить
        private void SaveButton_Click(object sender, EventArgs e)
        {
            PictureBox savePictureBox=pictureBox2;
            if (RGBHSV.lastBut=='h')
                savePictureBox = pictureBox5;
            if (savePictureBox.Image != null) // если изображение в pictureBox имеется
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Сохранить картинку как...";
                sfd.OverwritePrompt = true; // показывать ли "Перезаписать файл" если пользователь указывает имя файла, который уже существует
                sfd.CheckPathExists = true; // отображает ли диалоговое окно предупреждение, если пользователь указывает путь, который не существует
                // фильтр форматов файлов
                sfd.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                sfd.ShowHelp = true; // отображается ли кнопка Справка в диалоговом окне
                // если в диалоге была нажата кнопка ОК
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // сохраняем изображение
                        savePictureBox.Image.Save(sfd.FileName);
                    }
                    catch // в случае ошибки выводим MessageBox
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }

    class RGBHSV
    {
        static public char lastBut = 'g';
        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }


        public static Color HsvToRgb(double h, double S, double V)
        {
          
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Красный цвет преобладает

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Зеленый цвет преобладает

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Синий цвет преобладает

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Красный цвет преобладает

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // При ошибки в округлениях

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Если цвет не распознан вернем черный

                    default:
                        
                        R = G = B = V; 
                        break;
                }
            }
            var r = Clamp((int)(R * 255.0));
            var g = Clamp((int)(G * 255.0));
            var b = Clamp((int)(B * 255.0));

            return Color.FromArgb(255, r, g, b);
        }


        private static int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }



        //2я реализация перевода HSV в RGB для проверки идентичности пролученных изображений.
        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }


    }

}
