using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
//Rahave Suresan
//ImageProcessing Application Student Version
//Created by Mr. Iannotta for ICS4U
//Dec.09.2020

namespace ImageProcessing
{
    public partial class frmMain : Form
    {
        private Color[,] original; //this is the original picture - never change the values stored in this array
        private Color[,] transformedPic;  //transformed picture that is displayed

        public frmMain()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //this method draws the transformed picture
            //what ever is stored in transformedPic array will
            //be displayed on the form

            base.OnPaint(e);

            Graphics g = e.Graphics;

            //only draw if picture is transformed
            if (transformedPic != null)
            {
                //get height and width of the transfrormedPic array
                int height = transformedPic.GetUpperBound(0)+1;
                int width = transformedPic.GetUpperBound(1) + 1;

                //create a new Bitmap to be dispalyed on the form
                Bitmap newBmp = new Bitmap(width, height);
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        //loop through each element transformedPic and set the 
                        //colour of each pixel in the bitmalp
                        newBmp.SetPixel(j, i, transformedPic[i, j]);
                    }

                }
                //call DrawImage to draw the bitmap
                g.DrawImage(newBmp, 0, 20, width, height);
            }
            
        }


        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            //this method reads in a picture file and stores it in an array

            //try catch should handle any errors for invalid picture files
            try
            {

                //open the file dialog to select a picture file

                OpenFileDialog fd = new OpenFileDialog();

                //create a bitmap to store the file in
                Bitmap bmp;

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    //store the selected file into a bitmap
                    bmp = new Bitmap(fd.FileName);

                    //create the arrays that store the colours for the image
                    //the size of the arrays is based on the height and width of the bitmap
                    //initially both the original and transformedPic arrays will be identical
                    original = new Color[bmp.Height, bmp.Width];
                    transformedPic = new Color[bmp.Height, bmp.Width];

                    //load each color into a color array
                    for (int i = 0; i < bmp.Height; i++)//each row
                    {
                        for (int j = 0; j < bmp.Width; j++)//each column
                        {
                            //assign the colour in the bitmap to the array
                            original[i, j] = bmp.GetPixel(j, i);
                            transformedPic[i, j] = original[i, j];
                        }
                    }
                    //this will cause the form to be redrawn and OnPaint() will be called
                    this.Refresh();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Picture File. \n" + ex.Message);
            }
            
        }

        private void mnuProcessDarken_Click(object sender, EventArgs e)
        {
            //code to Darken
            if (transformedPic != null)
            {
                int Red, Green, Blue;
                for ( int i = 0; i < transformedPic.GetLength (0); i++)
                {
                    for ( int j = 0; j < transformedPic.GetLength (1); j++)
                    {
                        //Color Pixel = transformedPic[i, j];
                        Red = transformedPic[i, j].R - 10;
                        //Red = Pixel.R - 10;
                        if ( Red < 0) Red = 0;
                        Green = transformedPic[i, j].G - 10;
                        //Green = Pixel.G - 10;
                        if (Green < 0) Green = 0;
                        Blue = transformedPic[i, j].B - 10;
                        //Blue = Pixel.B - 10;
                        if (Blue < 0) Blue = 0;
                        transformedPic[i, j] = Color.FromArgb(Red, Green, Blue);

                    }
                }
            }
            this.Refresh();
        }

        private void mnuProcessInvert_Click(object sender, EventArgs e)
        {
            //code to Invert
            if ( transformedPic != null)
            {
                int Red, Green, Blue;
                for ( int i = 0; i < transformedPic.GetLength (0); i++)
                {
                    for (int j = 0; j < transformedPic.GetLength (1); j++)
                    {
                        Color Pixel = transformedPic[i, j];
                        Red = 255 - Pixel.R;
                        Green = 255 - Pixel.G;
                        Blue = 255 - Pixel.B;

                        transformedPic[i, j] = Color.FromArgb(Red, Green, Blue);
                        
                    }
                }
            }
            this.Refresh();
        }

        private void mnuProcessWhiten_Click(object sender, EventArgs e)
        {
            //code to Whiten
            if (transformedPic != null)
            {
                int Red, Green, Blue;
                for (int i = 0; i < transformedPic.GetLength (0); i++)
                {
                    for ( int j = 0; j < transformedPic.GetLength (1); j++)
                    {
                        Color Pixel = transformedPic[i, j];
                        Red = Pixel.R + 10;
                        if (Red > 255) Red = 255;
                        Green = Pixel.G + 10;
                        if (Green > 255) Green = 255;
                        Blue = Pixel.B + 10;
                        if (Blue > 255) Blue = 255;

                        transformedPic[i, j] = Color.FromArgb(Red, Green, Blue);
                    }
                }
            }
            this.Refresh();

        }

        private void mnuProcessRotate_Click(object sender, EventArgs e)
        {
            //code to Rotate
            if (transformedPic != null)
            {
                //create Temp array
                Color[,] Temp = new Color[transformedPic.GetLength(0), transformedPic.GetLength(1)];

                //copy transformedPic
                for (int i = 0; i < transformedPic.GetLength(0); i++)
                {
                    for ( int j =0; j < transformedPic.GetLength(1); j++)
                    {
                        Temp[i, j] = transformedPic[i, j];
                    }
                }

                //resize transformedPic array (Col to Row, Row to Col)
                transformedPic = new Color[transformedPic.GetLength(1), transformedPic.GetLength(0)];

                int Count = Temp.GetLength(1);
                //Rotate
                for ( int i = 0; i < transformedPic.GetLength(0); i++)
                {
                    Count--;
                    for ( int j = 0; j < transformedPic.GetLength(1); j++)
                    {
                        if (Count == -1)
                            Count = original.GetLength(0) - 1;
                        transformedPic[i, j] = Temp[j, Count];
                        
                    }
                }
            }

            this.Refresh();
        }

        private void mnuProcessReset_Click(object sender, EventArgs e)
        {
            //code to Reset
            if (transformedPic != null)
            {
                transformedPic = new Color[original.GetLength(0), original.GetLength(1)];

                for ( int i = 0; i < original.GetLength (0); i++)
                {
                    for ( int j = 0; j < original.GetLength (1); j++ )
                    {
                        transformedPic[i, j] = original[i, j];
                    }
                }
            }
            this.Refresh();
        }

        private void mnuProcessFlipX_Click(object sender, EventArgs e)
        {
            //code to FlipX
            if (transformedPic != null)
            {
                int Coloumn = transformedPic.GetLength(1) / 2; // number of coloumns it will loop by
                for (int i = 0; i < transformedPic.GetLength (0); i++)
                {
                    for (int j = 0; j < Coloumn; j++)
                    {
                        //swap
                        Color Pixel = transformedPic[i, j]; //keep tracks of original value
                        int IndexColoumn =  transformedPic.GetLength(1) - 1;
                        transformedPic[i, j] = transformedPic [i, IndexColoumn-j];
                        transformedPic[i, IndexColoumn-j] = Pixel;
                    }
                }
            }
            this.Refresh();
        }

        private void mnuProcessFlipY_Click(object sender, EventArgs e)
        {
            //code to FlipY
            if (transformedPic != null)
            {
                int Row = transformedPic.GetLength(0) / 2; // number rows it would loop by
                for ( int i = 0; i < Row; i++)
                {
                    for ( int j = 0; j < transformedPic.GetLength (1); j++)
                    {
                        //swap
                        Color Pixel = transformedPic[i, j]; // track originial value 
                        int IndexRow = transformedPic.GetLength(0) - 1; 
                        transformedPic[i, j] = transformedPic[IndexRow - i, j];
                        transformedPic[IndexRow - i, j] = Pixel;
                    }
                }
            }
            this.Refresh();
        }

        private void mnuProcessMirrorH_Click(object sender, EventArgs e)
        {
            //code to Mirror Horizontally
            if (transformedPic != null)
            {
                //create temp 
                Color[,] Temp = new Color[transformedPic.GetLength(0), transformedPic.GetLength(1)];

                //copy transformedPic into temp array
                for ( int i = 0; i < transformedPic.GetLength (0); i++)
                {
                    for ( int j = 0; j < transformedPic.GetLength (1); j++)
                    {
                        Temp[i, j] = transformedPic[i, j];
                    }
                }

                //resize transformedPic
                 transformedPic = new Color[transformedPic.GetLength(0), transformedPic.GetLength(1) * 2];

                //Copy temp into first half of transformedPic
                //int Index = original.GetLength(1);
                int Index = transformedPic.GetLength (1)/ 2;
                for (int i = 0; i < transformedPic.GetLength(0); i++)
                {
                    for (int j = 0; j < transformedPic.GetLength(1); j++)
                    {
                        if (j < Index)
                            transformedPic[i, j] = Temp[i, j];
                    }

                }

                //Flip Temp horizontally
                int Coloumn = Temp.GetLength(1) / 2; // number of coloumns it will loop by
                for (int i = 0; i < transformedPic.GetLength(0); i++)
                {
                    for (int j = 0; j < Coloumn; j++)
                    {
                        //swap
                        Color Pixel = Temp [i, j]; //keep tracks of original value
                        int IndexColoumn = Temp.GetLength(1) - 1;
                        Temp[i, j] = Temp[i, IndexColoumn - j];
                        Temp[i, IndexColoumn - j] = Pixel;
                    }
                    
                }
                

                //// Copy Temp into second half of transformed Pic
                int Count = 0;
                for (int i = 0; i < transformedPic.GetLength(0); i++)
                {
                    for (int j = 0; j < transformedPic.GetLength(1); j++)
                    {
                        if (j >= Index)
                        {
                            if (Count >= Index)
                                Count = 0;
                            transformedPic[i, j] = Temp[i, Count];
                            Count++;

                        }

                    }
                }

            }
            this.Refresh();
        }

        private void mnuProcessMirrorV_Click(object sender, EventArgs e)
        {
            //code to Mirror Vertically
            if (transformedPic != null)
            {
                //Create Temp Array
                Color[,] Temp = new Color [transformedPic.GetLength(0), transformedPic.GetLength(1)];

                //Copy transformedPic into Temp
                for ( int i = 0; i < transformedPic.GetLength (0); i++)
                {
                    for ( int j = 0; j < transformedPic.GetLength(1); j++)
                    {
                        Temp[i, j] = transformedPic[i, j];
                    }
                }

                //resize transformed pic (double rows)
                transformedPic = new Color[transformedPic.GetLength(0) * 2, transformedPic.GetLength(1)];

                //copy temp array into first half of transformedPic
                int Index = transformedPic.GetLength(0) /2;
                for ( int i = 0; i < transformedPic.GetLength (0); i++)
                {
                    if (i < Index)
                    {
                        for (int j = 0; j < transformedPic.GetLength(1); j++)
                        {
                           transformedPic[i, j] = Temp[i, j];
                        }
                    }
                }

                ////flip Temp vertically
                int Row = Temp.GetLength(0) / 2; // number rows it would loop by
                for (int i = 0; i < Row; i++)
                {
                    for (int j = 0; j < transformedPic.GetLength(1); j++)
                    {
                        //swap
                        Color Pixel = Temp[i, j]; // track originial value 
                        int IndexRow = Temp.GetLength(0) - 1;
                        Temp[i, j] = Temp[IndexRow - i, j];
                        Temp[IndexRow - i, j] = Pixel;

                    }
                }

                ////Copy Temp into 2nd half of transformedPic
                int Count = Temp.GetLength(0) - 1;
                for (int i = 0; i < transformedPic.GetLength(0); i++)
                {
                    if (i >= Index)
                    {
                        Count++;
                        for (int j = 0; j < transformedPic.GetLength(1); j++)
                        {
                            if (Count == Index)
                                Count = 0;
                            transformedPic[i, j] = Temp[Count, j];
                  
                        }
                       
                    }
                    
                }
            }
            this.Refresh();
        }

        private void mnuProcessScale50_Click(object sender, EventArgs e)
        {
            //code to Scale 50%
            if (transformedPic != null)
            {
                //create a Temp array
                Color[,] Temp = new Color[transformedPic.GetLength(0), transformedPic.GetLength(1)];

                //copy transformedPic into Temp
                for (int i = 0; i < transformedPic.GetLength (0); i++)
                {
                    for ( int j = 0; j <transformedPic.GetLength (1); j++)
                    {
                        Temp[i, j] = transformedPic[i, j];
                    }
                }

                //resize transformedPic
                transformedPic = new Color[transformedPic.GetLength(0) / 2, transformedPic.GetLength(1) / 2];

                //copy every other coloumn and every other row
                int Count = 0; 
                int Count2 = 0;
               // int IndexCol = transformedPic.GetLength(1);
                for ( int i = 0; i < Temp.GetLength(0); i++)
                {

                    if(Count == transformedPic.GetLength(0))
                                    Count = 0;
                    if (i % 2 == 0)
                    {
                        for (int j = 0; j < Temp.GetLength(1); j++)
                        {
                            if (Count2 == transformedPic.GetLength(1))
                                Count2 = 0;
                            if ( j % 2 == 0)
                                transformedPic[Count, Count2] = Temp[i, j];
                            else
                                Count2++;
                        }
                    }
                    else
                    {
                        Count++;
                    }

                }
            }
            this.Refresh();
        }

        private void mnuProcessScale200_Click(object sender, EventArgs e)
        {
            //code to Scale 200%
            if (transformedPic != null)
            {
                //create Temp array
                Color[,] Temp = new Color[transformedPic.GetLength(0), transformedPic.GetLength(1)];
                
                //Copy transformedPic into Temp
                for (int i = 0; i < transformedPic.GetLength(0); i++)
                {
                    for (int j = 0; j < transformedPic.GetLength(1); j++)
                    {
                        Temp[i, j] = transformedPic[i, j];
                    }
                }

                //resize transformedPic
                transformedPic = new Color[transformedPic.GetLength(0) * 2, transformedPic.GetLength(1) * 2];

                //Scale 200
                int Count2 = 0;
                for ( int i = 0; i < transformedPic.GetLength(0); i++)
                {
                    int Count = 0;

                    if (i % 2 == 0 & i != 0)
                        Count2++;
                    for ( int j =0; j < transformedPic.GetLength(1); j++)
                    {
                        if (Count2 == Temp.GetLength(0))
                            Count2 = 0;
                        transformedPic[i, j] = Temp[Count2, j - Count];
                        if (j % 2 == 0)
                            Count++;

                    }
                }
              

            }
            this.Refresh();
        }

        private void mnuProcessBlur_Click(object sender, EventArgs e)
        {
            //code to Blur
            if ( transformedPic != null)
            {
                //create Temp
                Color[,] Temp = new Color[transformedPic.GetLength(0), transformedPic.GetLength(1)];

                //copy transformedPic
                for ( int i = 0; i < transformedPic.GetLength(0); i++)
                {
                    for ( int j = 0; j < transformedPic.GetLength(1); j++)
                    {
                        Temp[i, j] = transformedPic[i, j];
                    }
                }

                //9 Cases of Blur
                int Red, Blue, Green;
                int Row = transformedPic.GetLength(0) - 1;
                int Col = transformedPic.GetLength(1) - 1;
                for ( int i = 0; i < transformedPic.GetLength (0); i++)
                {
                    for (int j = 0; j < transformedPic.GetLength(1); j++)
                    {
                        Color Pixel = transformedPic[i, j];
                        //case 1 (middle section)
                        if (i != 0 && j != 0 && i != Row && j != Col)
                        {
                            //Blue
                            Blue = (Temp[i - 1, j].B + Temp[i - 1, j].B + Temp[i - 1, j + 1].B
                                   + Temp[i, j - 1].B + Temp[i, j].B + Temp[i, j + 1].B
                                   + Temp[i + 1, j + 1].B + Temp[i + 1, j - 1].B + Temp[i + 1, j + 1].B) / 9;
                            //Red
                            Red = (Temp[i - 1, j].R + Temp[i - 1, j].R + Temp[i - 1, j + 1].R
                                   + Temp[i, j - 1].R + Temp[i, j].R + Temp[i, j + 1].R
                                   + Temp[i + 1, j + 1].R + Temp[i + 1, j - 1].R + Temp[i + 1, j + 1].R) / 9;
                            //Green
                            Green = (Temp[i - 1, j].G + Temp[i - 1, j].G + Temp[i - 1, j + 1].G
                                   + Temp[i, j - 1].G + Temp[i, j].G + Temp[i, j + 1].G
                                   + Temp[i + 1, j + 1].G + Temp[i + 1, j - 1].G + Temp[i + 1, j + 1].G) / 9;

                             transformedPic [i,j] = Color.FromArgb(Red, Green, Blue);

                        }
                        else if (Pixel != Temp[0, 0] && Pixel != Temp[Row, 0] && Pixel != Temp[0,Col] && Pixel != Temp[Row,Col])
                        {
                            //first row
                            if (i == 0)
                            {
                                Blue = (Temp[i, j - 1].B + Temp[i, j].B + Temp[i, j + 1].B
                                     + Temp[i + 1, j - 1].B + Temp[i + 1, j ].B + Temp[i + 1, j + 1].B) / 6;

                                Green = (Temp[i, j - 1].G + Temp[i, j].G + Temp[i, j + 1].G
                                     + Temp[i + 1, j - 1].G + Temp[i + 1, j].G + Temp[i + 1, j + 1].G) / 6;

                                Red = (Temp[i, j - 1].R + Temp[i, j].R + Temp[i, j + 1].R
                                     + Temp[i + 1, j - 1].R + Temp[i + 1, j].R + Temp[i + 1, j + 1].R) / 6;

                                transformedPic [i,j] = Color.FromArgb(Red, Green, Blue);
                            }
                            // last row
                            if (i == Row)
                            {
                                Blue = (Temp[i - 1, j - 1].B + Temp[i - 1, j].B + Temp[i - 1, j + 1].B +
                                       Temp[i, j - 1].B + Temp[i, j].B + Temp[i, j + 1].B) / 6;

                                Red = (Temp[i - 1, j - 1].R + Temp[i - 1, j].R + Temp[i - 1, j + 1].R +
                                       Temp[i, j - 1].R + Temp[i, j].R + Temp[i, j + 1].R) / 6;

                                Green = (Temp[i - 1, j - 1].G + Temp[i - 1, j].G + Temp[i - 1, j + 1].G +
                                       Temp[i, j - 1].G + Temp[i, j].G + Temp[i, j + 1].G) / 6;

                                transformedPic[i, j] = Color.FromArgb(Red, Green, Blue);

                            }

                            //leftside
                            if (j == 0)
                            {
                                Red = (Temp[i - 1, j].R + Temp[i - 1, j + 1].R +
                                      Temp[i, j].R + Temp[i, j + 1].R +
                                      Temp[i + 1, j].R + Temp[i + 1, j + 1].R)/6;

                                Green = (Temp[i - 1, j].G + Temp[i - 1, j + 1].G +
                                      Temp[i, j].G + Temp[i, j + 1].G +
                                      Temp[i + 1, j].G + Temp[i + 1, j + 1].G) / 6;

                                Blue = (Temp[i - 1, j].B + Temp[i - 1, j + 1].B +
                                      Temp[i, j].B + Temp[i, j + 1].B +
                                      Temp[i + 1, j].B + Temp[i + 1, j + 1].B) / 6;

                                transformedPic [i,j] = Color.FromArgb(Red, Green, Blue);
                            }

                            //Right side
                            if ( j == Col)
                            {
                                Red = (Temp[i - 1, j - 1].R + Temp[i - 1, j].R +
                                      Temp[i, j - 1].R + Temp[i, j].R +
                                      Temp[i + 1, j - 1].R + Temp[i + 1, j].R)/6;

                                Green = (Temp[i - 1, j - 1].G + Temp[i - 1, j].G +
                                      Temp[i, j - 1].G + Temp[i, j].G +
                                      Temp[i + 1, j - 1].G + Temp[i + 1, j].G) / 6;

                                Blue = (Temp[i - 1, j - 1].B + Temp[i - 1, j].B +
                                      Temp[i, j - 1].B + Temp[i, j].B +
                                      Temp[i + 1, j - 1].B + Temp[i + 1, j].B) / 6;

                                transformedPic[i, j] = Color.FromArgb(Red, Green, Blue);
                            }
                        }
                        else
                        {
                            //Top Left Corner (0,0)
                            if (j == 0 && i ==0)
                            {
                                Red = (Temp[i, j].R + Temp[i, j + 1].R + Temp[i + 1, j].R + Temp[i + 1, j + 1].R)/4;

                                Green = (Temp[i, j].G + Temp[i, j + 1].G + Temp[i + 1, j].G + Temp[i + 1, j + 1].G) / 4;

                                Blue = (Temp[i, j].B + Temp[i, j + 1].B + Temp[i + 1, j].B + Temp[i + 1, j + 1].B) / 4;

                                transformedPic[i, j] = Color.FromArgb(Red, Green, Blue);
                            }

                            // Top Right Corner (0, Col)
                            if (i == 0 && j == Col)
                            {
                                Red = (Temp[i, j - 1].R + Temp[i, j].R + Temp[i + 1, j - 1].R + Temp [i + 1, j].R)/4;

                                Green = (Temp[i, j - 1].G + Temp[i, j].G + Temp[i + 1, j - 1].G + Temp[i + 1, j].G) / 4;

                                Blue = (Temp[i, j - 1].B + Temp[i, j].B + Temp[i + 1, j - 1].B + Temp[i + 1, j].B) / 4;

                                transformedPic[i, j] = Color.FromArgb(Red, Green, Blue);
                            }

                            //Bottom Left Corner (0,Row)
                            if (i == Row && j == 0)
                            {
                                Red = (Temp[i - 1, j].R + Temp[i - 1, j + 1].R + Temp[i, j].R + Temp[i, j + 1].R) /4;

                                Green = (Temp[i - 1, j].G + Temp[i - 1, j + 1].G + Temp[i, j].G + Temp[i, j + 1].G) / 4;

                                Blue = (Temp[i - 1, j].B + Temp[i - 1, j + 1].B + Temp[i, j].B + Temp[i, j + 1].B) / 4;

                                transformedPic[i, j] = Color.FromArgb(Red, Green, Blue);
                            }

                            //Bottom Right Corner (Row,Col)
                            if (i == Row && j == Col)
                            {
                                Red = (Temp[i - 1, j - 1].R + Temp[i - 1, j].R + Temp[i, j - 1].R + Temp[i, j].R) / 4;

                                Green = (Temp[i - 1, j - 1].G + Temp[i - 1, j].G + Temp[i, j - 1].G + Temp[i, j].G) / 4;

                                Blue = (Temp[i - 1, j - 1].B + Temp[i - 1, j].B + Temp[i, j - 1].B + Temp[i, j].B) / 4;

                                transformedPic[i, j] = Color.FromArgb(Red, Green, Blue);
                            }
                        }
                    }    
                }
            }
            this.Refresh();
        }


    }
}
