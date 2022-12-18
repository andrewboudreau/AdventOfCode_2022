//vusing System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Day00.Examples
//{
//    using System;
//    using System.Drawing;
//    using System.Windows.Forms;

//    using OpenTK;
//    using OpenTK.Graphics;
//    using OpenTK.Graphics.OpenGL;
//    using static System.Net.Mime.MediaTypeNames;

//    namespace RandomPixelTexture
//    {
//        public class MainWindow : Form
//        {
//            private int texture;

//            public MainWindow()
//            {
//                // Set the size and title of the window.
//                Width = 640;
//                Height = 480;
//                Text = "Random Pixel Texture";

//                // Create a new OpenGL control and add it to the window.
//                GLControl glControl = new GLControl();
//                glControl.Paint += new PaintEventHandler(OnPaint);
//                glControl.Dock = DockStyle.Fill;
//                Controls.Add(glControl);
//            }

//            private void OnPaint(object sender, PaintEventArgs e)
//            {
//                // Set the viewport and clear the screen.
//                GL.Viewport(0, 0, Width, Height);
//                GL.Clear(ClearBufferMask.ColorBufferBit);

//                // Bind the texture and draw a textured quad.
//                GL.BindTexture(TextureTarget.Texture2D, texture);
//                GL.Begin(PrimitiveType.Quads);
//                GL.TexCoord2(0, 0);
//                GL.Vertex2(-1, -1);
//                GL.TexCoord2(1, 0);
//                GL.Vertex2(1, -1);
//                GL.TexCoord2(1, 1);
//                GL.Vertex2(1, 1);
//                GL.TexCoord2(0, 1);
//                GL.Vertex2(-1, 1);
//                GL.End();

//            }
