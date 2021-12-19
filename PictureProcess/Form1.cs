using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureProcess
{
    public partial class Form1 : Form
    {
        Bitmap nowBitmap;
        int[] countPixel = new int[256];

        public Form1()
        {
            InitializeComponent();
        }
                   //加法运算
        private void 图像旋转ToolStripMenuItem_Click(object sender, EventArgs e)
        {       
            //实际是加法运算，因为之前这个框的名字是图像旋转，改不过来了
            //打开第二张图片
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "图位(*.bmp;*.jpg)|*.bmp;*.jpg";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            Bitmap bm2 = new Bitmap(dlg.FileName, true);
            pictureBox2.Image = bm2;                                            //显示第二张图片
            Bitmap bm3 = new Bitmap(bm2);                                       //结果图像

            for (int i = 0; i < nowBitmap.Width; i++)
            {
                for (int j = 0; j < nowBitmap.Height; j++)
                {
                    Color col1 = nowBitmap.GetPixel(i, j);                      //获取第一张图片的像素
                    Color col2 = bm2.GetPixel(i, j);                            //获取第二张图片的像素
                    int r = (int)((col1.R + col2.R) / 2);                       //R、G、B分相加取平均值
                    int g = (int)((col1.G + col2.G) / 2);
                    int b = (int)((col1.B + col2.B) / 2);
                    bm3.SetPixel(i, j, Color.FromArgb(r, g, b));                //保存相加结果

                }
            }
            pictureBox3.Image = bm3;
        }

        private void 旋转ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 几何运算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //这个没用，废弃
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() != DialogResult.OK)
                return;
            nowBitmap = new Bitmap(file.FileName, true);
            pictureBox1.Image = nowBitmap;
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 阈值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap tempBitmap = new Bitmap(nowBitmap.Width, nowBitmap.Height);
            for (int i = 0; i < nowBitmap.Width; i++)
            {
                for (int j = 0; j < nowBitmap.Height; j++)
                {
                    Color col = nowBitmap.GetPixel(i, j);
                    if (col.R > 128)
                        tempBitmap.SetPixel(i, j, Color.White);
                    else
                        tempBitmap.SetPixel(i, j, Color.Black);
                }

            }
            pictureBox2.Image = tempBitmap;
        }

        private void 灰度化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap tempBitmap = new Bitmap(nowBitmap.Width, nowBitmap.Height);
            for (int i = 0; i < nowBitmap.Width; i++)
            {
                for (int j = 0; j < nowBitmap.Height; j++)                         //跑一边所有的像素点
                {
                    Color col = nowBitmap.GetPixel(i, j);
                    int ret;
                    ret = (int)(col.R * 0.299 + col.G * 0.587 + col.B * 0.114);    //灰度值公式计算
                    tempBitmap.SetPixel(i, j, Color.FromArgb(ret, ret, ret));
                }

            }
            pictureBox2.Image = tempBitmap;
        }

        private void 镜像旋转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp = nowBitmap;
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            pictureBox2.Image = bmp;
        }

        private void 中心旋转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graphics graphics = this.CreateGraphics();
            graphics.Clear(Color.White);

            //装入图片
            Bitmap image = nowBitmap;

            //获取当前窗口的中心点
            Rectangle rect = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            PointF center = new PointF(rect.Width / 2, rect.Height / 2);

            float offsetX = 0;
            float offsetY = 0;
            offsetX = center.X - image.Width / 2;
            offsetY = center.Y - image.Height / 2;
            //构造图片显示区域:让图片的中心点与窗口的中心点一致
            RectangleF picRect = new RectangleF(offsetX, offsetY, image.Width, image.Height);
            PointF Pcenter = new PointF(picRect.X + picRect.Width / 2,
             picRect.Y + picRect.Height / 2);

            //让图片绕中心旋转一周
            for (int i = 0; i < 361; i += 10)
            {
                // 绘图平面以图片的中心点旋转
                graphics.TranslateTransform(Pcenter.X, Pcenter.Y);
                graphics.RotateTransform(i);
                //恢复绘图平面在水平和垂直方向的平移
                graphics.TranslateTransform(-Pcenter.X, -Pcenter.Y);
                //绘制图片并延时
                graphics.DrawImage(image, picRect);
                //Thread.Sleep(100);
                //重置绘图平面的所有变换
                graphics.ResetTransform();
            }
        }

        private void 度旋转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp = nowBitmap;
            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox2.Image = bmp;
        }

        private void 图像平移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //获得平移量
            int x = 50;
            int y = 50;
            //创建新图像，并赋值白色
            Bitmap tmp = new Bitmap(nowBitmap.Width, nowBitmap.Height);
            //遍历新图像，求对应的原图像位置，如果在范围内就赋值过来
            for (int i = 0; i < nowBitmap.Width; i++)
            {
                for (int j = 0; j < nowBitmap.Height; j++)
                {
                    if ((i - x) > 0 && (j - y) > 0 && (i - x) < nowBitmap.Width && (j - y) < nowBitmap.Height)
                    {
                        tmp.SetPixel(i, j, nowBitmap.GetPixel(i - x, j - y));
                    }
                    else
                        tmp.SetPixel(i, j, Color.White);
                }
            }
            //显示新图像
            pictureBox2.Image = tmp;
        }

        private void 水平镜像ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Bitmap tmp = new Bitmap(nowBitmap.Width, nowBitmap.Height);
            //遍历新图像，求对应的原图像位置，如果在范围内就赋值过来
            for (int i = 0; i < nowBitmap.Width; i++)
            {
                for (int j = 0; j < nowBitmap.Height; j++)
                {
                    tmp.SetPixel(i, j, nowBitmap.GetPixel(nowBitmap.Width - i - 1, j));
                }
            }
            //显示新图像
            pictureBox2.Image = tmp;
        }

        private void 垂直镜像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap tmp = new Bitmap(nowBitmap.Width, nowBitmap.Height);
            //遍历新图像，求对应的原图像位置，如果在范围内就赋值过来
            for (int i = 0; i < nowBitmap.Width; i++)
            {
                for (int j = 0; j < nowBitmap.Height; j++)
                {
                    tmp.SetPixel(i, j, nowBitmap.GetPixel(i, nowBitmap.Height - j - 1));
                }
            }
            //显示新图像
            pictureBox2.Image = tmp;
        }

        private void 普通旋转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f2 = new Form2();
            f2.ShowDialog();
            int x, y;
            int angle = 30;
            double radian = angle * Math.PI / 180.0;          //注意此处要用弧度，c#不支持角度运算
            //创建新画布，大小和原图一样
            Bitmap tmp = new Bitmap(nowBitmap.Width, nowBitmap.Height);
            for (int i = 0; i < nowBitmap.Width; i++)
            {
                for (int j = 0; j < nowBitmap.Height; j++)
                {
                    //x，y是旋转后的坐标
                    x = (int)(i * Math.Cos(radian) - j * Math.Sin(radian));
                    y = (int)(i * Math.Sin(radian) + j * Math.Cos(radian));
                    //筛选那些旋转后还留在框内的像素点
                    if (x > 0 && y > 0 && x < nowBitmap.Width && y < nowBitmap.Height)
                    {
                        //要注意是新画布推旧画布还是旧画布推新画布
                        tmp.SetPixel(i, j, nowBitmap.GetPixel(x, y));
                    }
                    else            //其余的赋予白色
                    {
                        tmp.SetPixel(i, j, Color.White);
                    }
                }
            }
            pictureBox2.Image = tmp;
        }

        private void 缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //获得缩小量
            double x = 0.5;
            double y = 0.5;
            //创建新图像
            Bitmap tmp = new Bitmap((int)(nowBitmap.Width * x), (int)(nowBitmap.Height * y));
            //遍历新图像，求对应的原图像位置，如果在范围内就赋值过来
            for (int i = 0; i < nowBitmap.Width; i++)
            {
                for (int j = 0; j < nowBitmap.Height; j++)
                {
                    tmp.SetPixel(i, j, nowBitmap.GetPixel((int)(i / x), (int)(j / y)));
                }
            }
            //显示新图像
            pictureBox2.Image = tmp;
        }

        private void 放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //获得放大量
            double x = 1.5;
            double y = 1.5;
            //创建新图像
            Bitmap tmp = new Bitmap((int)(nowBitmap.Width * x), (int)(nowBitmap.Height * y));
            //遍历新图像，求对应的原图像位置，如果在范围内就赋值过来
            for (int i = 0; i < nowBitmap.Width; i++)
            {
                for (int j = 0; j < nowBitmap.Height; j++)
                {
                    tmp.SetPixel(i, j, nowBitmap.GetPixel((int)(i / x), (int)(j / y)));
                }
            }
            //显示新图像
            pictureBox2.Image = tmp;
        }

        private void 按比例灰度拉伸ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //打开原图，创建新图像
            Bitmap tmp = new Bitmap(nowBitmap.Width, nowBitmap.Height);
            //获得拉伸后的灰度范围
            int x2 = 0;
            int y2 = 255;
            //统计原图的灰度范围
            int x1 = 255;
            int y1 = 0;
            for (int i = 0; i < nowBitmap.Width; i++)
            {
                for (int j = 0; j < nowBitmap.Height; j++)
                {
                    Color col = nowBitmap.GetPixel(i, j);
                    if(col.R < x1) x1 = col.R;
                    if(col.R > y1) y1 = col.R;
                }
            }
            //遍历图像，计算斜率，按线性公式逐点计算每个像素点拉伸后的值
            for (int i = 0; i < nowBitmap.Width; i++)
            {
                for (int j = 0; j < nowBitmap.Height; j++)
                {
                    Color col_2 = nowBitmap.GetPixel(i, j);
                    int ret = (y2 - x2) / (y1 - x1) * (col_2.R - x1) + x2;
                    tmp.SetPixel(i, j, Color.FromArgb(ret, ret, ret));
                }
            }
            pictureBox2.Image = tmp;
        }

        private void 直方图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nowBitmap == null)
                return;
            //创建新的图像对象
            Bitmap tempBitmap = new Bitmap(nowBitmap.Width, nowBitmap.Height);
            //统计原图像灰度等级的像素数
            int[] histogram = new int[256];//灰度等级数组
            int[] sum = new int[256];//累积求和的灰度等级
            int[] lut = new int[256];//新的灰度映射等级，映射表
            for (int i = 0; i < nowBitmap.Width; i++)
            {
                for (int j = 0; j < nowBitmap.Height; j++)
                {
                    histogram[nowBitmap.GetPixel(i, j).R]++;
                }
            }
            //计算累积求和函数
            for (int i = 0; i < 256; i++)
            {
                if (i == 0)
                    sum[0] = histogram[0];
                else
                    sum[i] = sum[i - 1] + histogram[i];
            }
            //将累积求和函数映射为新的灰度等级，映射表
            for (int i = 0; i < 256; i++)
            {
                lut[i] = (int)(255.0 * sum[i] / (nowBitmap.Height * nowBitmap.Width));
            }
            //遍历新图像，根据原像素值逐点通过映射表计算新的灰度值
            for (int i = 0; i < tempBitmap.Height; i++)
            {
                for (int j = 0; j < tempBitmap.Width; j++)
                {
                    Color tempColor = nowBitmap.GetPixel(i, j);
                    tempBitmap.SetPixel(i, j, Color.FromArgb(lut[tempColor.R], lut[tempColor.R], lut[tempColor.R]));
                }
            }
            //显示均衡化的结果图像
            pictureBox2.Image = tempBitmap;
        }

        private void 产生ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //椒盐噪声
            //创建新图像
            Bitmap tmp = new Bitmap(nowBitmap);
            Random r = new Random();

            //遍历新图像，求对应的原图像位置，如果在范围内就赋值过来
            for (int i = 0; i < nowBitmap.Width; i++)
            {
                for (int j = 0; j < nowBitmap.Height; j++)
                {
                    double s = r.NextDouble();
                    if(s<0.05)
                    {
                        tmp.SetPixel(i, j, Color.White);
                    }
                    if(s>0.95)
                    {
                        tmp.SetPixel(i, j, Color.Black);
                    }
                }
            }
            //显示新图像
            pictureBox2.Image = tmp;
        }

        private void 产生ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //高斯噪声
            //创建新图像
            Bitmap tmp = new Bitmap(nowBitmap);
            Random r = new Random();
            int xmean = 0;          //期望
            int sigma = 50;         //方差

            //遍历新图像，求对应的原图像位置，如果在范围内就赋值过来
            for (int i = 0; i < nowBitmap.Width; i++)
            {
                for (int j = 0; j < nowBitmap.Height; j++)
                {
                    double s = r.NextDouble() * 2 - 1;
                    int ret = (int)(nowBitmap.GetPixel(i, j).R + xmean + sigma * s);
                    if (ret > 255)
                    {
                        ret = 255;
                    }
                    if (ret < 0)
                    {
                        ret = 0;
                    }
                    tmp.SetPixel(i, j, Color.FromArgb(ret, ret, ret));
                }
            }
            //显示新图像
            pictureBox2.Image = tmp;

        }

        private void 领域平均法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //创建新图像
            Bitmap tmp = new Bitmap(nowBitmap);

            //遍历新图像，求对应原图像的位置坐标
            for(int i = 1; i < tmp.Width - 1 ; i++)
            {
                for(int j = 1; j < tmp.Height - 1 ; j++)
                {
                    int ret = (int)((nowBitmap.GetPixel(i, j).R + nowBitmap.GetPixel(i-1, j).R + nowBitmap.GetPixel(i, j-1).R+
                                     nowBitmap.GetPixel(i-1, j-1).R + nowBitmap.GetPixel(i, j+1).R + nowBitmap.GetPixel(i+1, j).R+
                                     nowBitmap.GetPixel(i+1, j+1).R + nowBitmap.GetPixel(i+1, j-1).R + nowBitmap.GetPixel(i-1, j+1).R)/9);
                    tmp.SetPixel(i, j, Color.FromArgb(ret, ret, ret));
                }
            }
            pictureBox2.Image = tmp;
        }

        private void 超限领域平均法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //创建新图像
            Bitmap tmp = new Bitmap(nowBitmap);
            //遍历新图像，求对应原图像的位置坐标
            for (int i = 1; i < tmp.Width - 1; i++)
            {
                for (int j = 1; j < tmp.Height - 1; j++)
                {
                    int ret = (int)((nowBitmap.GetPixel(i, j).R + nowBitmap.GetPixel(i - 1, j).R + nowBitmap.GetPixel(i, j - 1).R +
                                     nowBitmap.GetPixel(i - 1, j - 1).R + nowBitmap.GetPixel(i, j + 1).R + nowBitmap.GetPixel(i + 1, j).R +
                                     nowBitmap.GetPixel(i + 1, j + 1).R + nowBitmap.GetPixel(i + 1, j - 1).R + nowBitmap.GetPixel(i - 1, j + 1).R) / 9);
                    if(Math.Abs(ret - nowBitmap.GetPixel(i,j).R)>50)
                        tmp.SetPixel(i, j, Color.FromArgb(ret, ret, ret));
                }
            }
            pictureBox2.Image = tmp;
        }

        private void 中值滤波ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //创建新图像
            Bitmap tmp = new Bitmap(nowBitmap);

            //遍历新图像，求对应原图像的位置坐标
            for (int i = 1; i < tmp.Width - 1; i++)
            {
                for (int j = 1; j < tmp.Height - 1; j++)
                {
                    int[] a = new int[9];
                    a[0] = nowBitmap.GetPixel(i, j).R;
                    a[1] = nowBitmap.GetPixel(i - 1, j).R;
                    a[2] = nowBitmap.GetPixel(i, j - 1).R;
                    a[3] = nowBitmap.GetPixel(i - 1, j - 1).R;
                    a[4] = nowBitmap.GetPixel(i, j + 1).R;
                    a[5] = nowBitmap.GetPixel(i + 1, j).R;
                    a[6] = nowBitmap.GetPixel(i + 1, j + 1).R;
                    a[7] = nowBitmap.GetPixel(i + 1, j - 1).R;
                    a[8] = nowBitmap.GetPixel(i - 1, j + 1).R;
                    Array.Sort(a);
                    tmp.SetPixel(i, j, Color.FromArgb(a[4], a[4], a[4]));     //这里的a[4]是因为：排序后，a[4]为nowBitmap.GetPixel(i, j).R;
                }


            }
            pictureBox2.Image = tmp;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void robert算子ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //废弃此单元
        }

        private void robert算子ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //创建新图像
            Bitmap tmp = new Bitmap(nowBitmap);

            //遍历新图像，求对应原图像的位置坐标
            for (int i = 0; i < tmp.Width - 1; i++)
            {
                for (int j = 0; j < tmp.Height - 1; j++)
                {
                    int ret_x = nowBitmap.GetPixel(i + 1, j + 1).R - nowBitmap.GetPixel(i, j).R;        //默认左上角的是（i，j），右下角的是（i+1，j+1）
                        ret_x = Math.Abs(ret_x);

                    int ret_y = nowBitmap.GetPixel(i + 1, j).R - nowBitmap.GetPixel(i, j + 1).R;
                        ret_y = Math.Abs(ret_y);

                    int mag = ret_x + ret_y;

                    if(mag > 255) mag = 255;
                    if(mag < 0)   mag = 0;

                    tmp.SetPixel(i, j, Color.FromArgb(mag, mag, mag));
                }
            }
            pictureBox2.Image = tmp;
        }

        private void prewitt算子ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //创建新图像
            Bitmap tmp = new Bitmap(nowBitmap);

            //遍历新图像，求对应原图像的位置坐标
            for (int i = 1; i < tmp.Width - 1; i++)
            {
                for (int j = 1; j < tmp.Height - 1; j++)
                {
                    int ret_x = (nowBitmap.GetPixel(i - 1, j + 1).R + nowBitmap.GetPixel(i, j + 1).R + nowBitmap.GetPixel(i + 1, j + 1).R) -
                                (nowBitmap.GetPixel(i - 1, j - 1).R + nowBitmap.GetPixel(i, j - 1).R + nowBitmap.GetPixel(i + 1, j - 1).R); 
                    ret_x = Math.Abs(ret_x);

                    int ret_y = (nowBitmap.GetPixel(i + 1, j - 1).R + nowBitmap.GetPixel(i + 1, j).R + nowBitmap.GetPixel(i + 1, j + 1).R) -
                                (nowBitmap.GetPixel(i - 1, j - 1).R + nowBitmap.GetPixel(i - 1, j).R + nowBitmap.GetPixel(i - 1, j + 1).R);
                    ret_y = Math.Abs(ret_y);

                    int mag = ret_x + ret_y;

                    if (mag > 255) mag = 255;
                    if (mag < 0) mag = 0;

                    tmp.SetPixel(i, j, Color.FromArgb(mag, mag, mag));
                }
            }
            pictureBox3.Image = tmp;
        }

        private void sobel算子ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //创建新图像
            Bitmap tmp = new Bitmap(nowBitmap);

            //遍历新图像，求对应原图像的位置坐标
            for (int i = 1; i < tmp.Width - 1; i++)
            {
                for (int j = 1; j < tmp.Height - 1; j++)
                {
                    int ret_x = (nowBitmap.GetPixel(i - 1, j + 1).R + nowBitmap.GetPixel(i, j + 1).R * 2 + nowBitmap.GetPixel(i + 1, j + 1).R) -
                                (nowBitmap.GetPixel(i - 1, j - 1).R + nowBitmap.GetPixel(i, j - 1).R * 2+ nowBitmap.GetPixel(i + 1, j - 1).R);
                    ret_x = Math.Abs(ret_x);

                    int ret_y = (nowBitmap.GetPixel(i + 1, j - 1).R + nowBitmap.GetPixel(i + 1, j).R * 2 + nowBitmap.GetPixel(i + 1, j + 1).R) -
                                (nowBitmap.GetPixel(i - 1, j - 1).R + nowBitmap.GetPixel(i - 1, j).R * 2 + nowBitmap.GetPixel(i - 1, j + 1).R);
                    ret_y = Math.Abs(ret_y);

                    int mag = ret_x + ret_y;

                    if (mag > 255) mag = 255;
                    if (mag < 0) mag = 0;

                    tmp.SetPixel(i, j, Color.FromArgb(mag, mag, mag));
                }
            }
            pictureBox4.Image = tmp;
        }

        private void 四邻域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //创建新图像
            Bitmap tmp = new Bitmap(nowBitmap);

            //遍历新图像，求对应原图像的位置坐标
            for (int i = 1; i < tmp.Width - 1; i++)
            {
                for (int j = 1; j < tmp.Height - 1; j++)
                {
                    int ret = (nowBitmap.GetPixel(i, j - 1).R + nowBitmap.GetPixel(i, j + 1).R + nowBitmap.GetPixel(i + 1, j).R +
                                 nowBitmap.GetPixel(i - 1, j).R - nowBitmap.GetPixel(i ,j).R * 4);
                    int mag = Math.Abs(ret);

                    if (mag > 255) mag = 255;
                    if (mag < 0) mag = 0;

                    tmp.SetPixel(i, j, Color.FromArgb(mag, mag, mag));
                }
            }
            pictureBox2.Image = tmp;
        }

        private void 八邻域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //创建新图像
            Bitmap tmp = new Bitmap(nowBitmap);

            //遍历新图像，求对应原图像的位置坐标
            for (int i = 1; i < tmp.Width - 1; i++)
            {
                for (int j = 1; j < tmp.Height - 1; j++)
                {
                    int ret = (nowBitmap.GetPixel(i - 1, j - 1).R + nowBitmap.GetPixel(i, j - 1).R + nowBitmap.GetPixel(i + 1, j - 1).R +
                               nowBitmap.GetPixel(i - 1, j).R + nowBitmap.GetPixel(i + 1, j).R + nowBitmap.GetPixel(i - 1, j + 1).R +
                               nowBitmap.GetPixel(i, j + 1).R + nowBitmap.GetPixel(i + 1, j + 1).R - nowBitmap.GetPixel(i, j).R * 8);
                    int mag = Math.Abs(ret);

                    if (mag > 255) mag = 255;
                    if (mag < 0) mag = 0;

                    tmp.SetPixel(i, j, Color.FromArgb(mag, mag, mag));
                }
            }
            pictureBox3.Image = tmp;
        }

        private void ostu算法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //创建新图像
            Bitmap tmp = new Bitmap(nowBitmap);
            //设置初始阈值T与sigma为0
            int thre = 0;
            double sigma = 0.0;
            double sigmaMax = 0.0;
            //统计原图像的灰度直方图
            int[] countPixel = new int[256];
            for(int i = 0;i < nowBitmap.Width;i++)
            {
                for(int j = 0;j < nowBitmap.Height;j++)
                {
                    countPixel[nowBitmap.GetPixel(i, j).R]++;
                }
            }
            //循环计算类间方差；利用阈值T把图像分割成两部分区域，计算两部分的类间方差
            //和sigma比较，保存最大的类间方差所对应的阈值
            for(int i = 0;i < 255; i++)
            {
                double count_1 = 0;
                double count_2 = 0;
                double sumGray_1 = 0;
                double sumGray_2 = 0;
                for(int j = 0;j < i; j++)
                {
                    sumGray_1 = sumGray_1 + j * countPixel[j];
                    count_1 = count_1 + countPixel[j];
                }
                for(int j=i;j<255;j++)
                {
                    sumGray_2 = sumGray_2 + j * countPixel[j];
                    count_2 = count_2 + countPixel[j];
                }

                if (count_1 == 0 || count_2 == 0)
                    continue;

                sigma = 1.0 * count_1 * count_2 * Math.Pow(sumGray_1/count_1 - sumGray_2/count_2 , 2);
                if (sigma > sigmaMax)
                {
                    sigmaMax = sigma;
                    thre = i;
                }
            }
            //使用最大类间方差的阈值T来二值化原图像，并显示新图像
            for(int i = 0;i < nowBitmap.Width; i++)
            {
                for(int j = 0;j < nowBitmap.Height; j++)
                {
                    Color col = nowBitmap.GetPixel(i, j);
                    if (col.R > thre)
                        tmp.SetPixel(i, j, Color.White);
                    else
                        tmp.SetPixel(i, j, Color.Black);
                }
            }
            pictureBox2.Image = tmp;
            label1.Text = Convert.ToString(thre);
        }

        private void sobel算子ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //创建新图像
            Bitmap tmp = new Bitmap(nowBitmap);

            //遍历新图像，求对应原图像的位置坐标
            for (int i = 1; i < tmp.Width - 1; i++)
            {
                for (int j = 1; j < tmp.Height - 1; j++)
                {
                    int ret_x = (nowBitmap.GetPixel(i - 1, j + 1).R + nowBitmap.GetPixel(i, j + 1).R * 2 + nowBitmap.GetPixel(i + 1, j + 1).R) -
                                (nowBitmap.GetPixel(i - 1, j - 1).R + nowBitmap.GetPixel(i, j - 1).R * 2 + nowBitmap.GetPixel(i + 1, j - 1).R);
                    ret_x = Math.Abs(ret_x);

                    int ret_y = (nowBitmap.GetPixel(i + 1, j - 1).R + nowBitmap.GetPixel(i + 1, j).R * 2 + nowBitmap.GetPixel(i + 1, j + 1).R) -
                                (nowBitmap.GetPixel(i - 1, j - 1).R + nowBitmap.GetPixel(i - 1, j).R * 2 + nowBitmap.GetPixel(i - 1, j + 1).R);
                    ret_y = Math.Abs(ret_y);

                    int mag = ret_x + ret_y;

                    if (mag > 100)
                        tmp.SetPixel(i, j, Color.White);
                    else
                        tmp.SetPixel(i, j, Color.Black);
                }
            }
            pictureBox2.Image = tmp;
        }

        private void 减法运算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "图位(*.bmp;*.jpg)|*.bmp;*.jpg";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            Bitmap bm2 = new Bitmap(dlg.FileName, true);
            pictureBox2.Image = bm2;                                            //显示第二张图片
            Bitmap bm3 = new Bitmap(bm2);                                       //结果图像

            for (int i = 0; i < nowBitmap.Width; i++)
            {
                for (int j = 0; j < nowBitmap.Height; j++)
                {
                    Color col1 = nowBitmap.GetPixel(i, j);                      //获取第一张图片的像素
                    Color col2 = bm2.GetPixel(i, j);                            //获取第二张图片的像素
                    int r = (int)((col1.R - col2.R) / 2);                       //R、G、B分相加取平均值
                    int g = (int)((col1.G - col2.G) / 2);
                    int b = (int)((col1.B - col2.B) / 2);
                    bm3.SetPixel(i, j, Color.FromArgb(r, g, b));                //保存相减结果

                }
            }
            pictureBox3.Image = bm3;
        }
    }
}
