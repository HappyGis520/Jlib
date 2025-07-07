/*******************************************************************
 * * 功   能：  日志输出接口
 * * 作   者：  Jack
 * * 编程语言： C# 
 * *******************************************************************/
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Jlib
{
    /// <summary>
    /// 在图片上绘制矩形
    /// </summary>
    public static class ImageDrawHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cx">中心坐标X</param>
        /// <param name="cy">中心坐标Y</param>
        /// <param name="lx">长半轴</param>
        /// <param name="ly">短半轴</param>
        /// <param name="ra">旋转角度（度）</param>
        /// <returns></returns>
        private static System.Windows.Point[] CalculateRectangleVertices(double cx,double cy,double lx,double ly,double ra)
        {
            double _cx = cx ;
            double _cy = cy;
            double halfLx = lx / 2;  // 半长轴
            double halfLy = ly / 2;  // 半短轴
            double rotation = ra;    // 旋转角度（度）

            // 将角度转换为弧度
            double radians = rotation * Math.PI / 180.0;
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            // 计算未旋转的四个顶点（相对中心点）
            System.Windows.Point[] unrotatedPoints = new System.Windows.Point[]
            {
                new System.Windows.Point(-halfLx, -halfLy),  // 左上
                new System.Windows.Point(halfLx, -halfLy),   // 右上
                new System.Windows.Point(halfLx, halfLy),    // 右下
                new System.Windows.Point(-halfLx, halfLy)    // 左下
            };

            // 旋转并平移顶点
            System.Windows.Point[] rotatedPoints = new System.Windows.Point[4];
            for (int i = 0; i < 4; i++)
            {
                double x = unrotatedPoints[i].X;
                double y = unrotatedPoints[i].Y;

                // 应用旋转矩阵
                double rotatedX = x * cos - y * sin;
                double rotatedY = x * sin + y * cos;

                // 平移到中心点
                rotatedPoints[i] = new System.Windows.Point(rotatedX + _cx, rotatedY + _cy);
            }

            return rotatedPoints;
        }
        public static BitmapSource DrawRectOnBitmap(BitmapSource source, double cx, double cy, double lx, double ly, double ra)
        {
            var myrect = new System.Windows.Rect(0, 0, source.PixelWidth, source.PixelHeight);
            System.Windows.Point[] vertices = CalculateRectangleVertices(cx,cy,lx,ly,ra);
            // 创建路径几何
            PathGeometry polygon = new PathGeometry();
            PathFigure figure = new PathFigure
            {
                StartPoint = vertices[0],
                IsClosed = true,
                IsFilled = false
            };

            // 添加线段（连接所有顶点）
            for (int i = 1; i < vertices.Length; i++)
            {
                figure.Segments.Add(new LineSegment(vertices[i], true));
            }
            figure.Segments.Add(new LineSegment(vertices[0], true)); // 闭合路径
            polygon.Figures.Add(figure);


            // 创建绘图视觉对象
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                // 绘制原始图像
                drawingContext.DrawImage(source, myrect);
                // 绘制绿色边框
                System.Windows.Media.Pen greenPen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.LimeGreen, 3); // 3像素宽的绿色边框
                drawingContext.DrawGeometry(null, greenPen, polygon);
            }
            // 渲染为新的位图
            RenderTargetBitmap rtb = new RenderTargetBitmap(
                source.PixelWidth,
                source.PixelHeight,
                source.DpiX,
                source.DpiY,
                PixelFormats.Pbgra32);
            rtb.Render(drawingVisual);
            return rtb;
        }
    }
}
