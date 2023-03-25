// Copyright (c) 2023 Dewnith Fernando @github.com/Pahasara.
// Licensed under the MIT license.

using System;
using System.Windows.Forms;

namespace Zero
{
    public partial class Guide : Form
    {
        public Guide()
        {
            InitializeComponent();
        }

        int slide = 0, numberOfSlides = 5;

        private void Guide_Load(object sender, EventArgs e)
        {
            changeSlide();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            slide++;
            changeSlide();
        }

        private void changeSlide()
        {
            if (slide <= numberOfSlides)
            {
                show(btnBack);
                show(btnNext);
                btnNext.Image = Properties.Resources.btnGuideNext;
                if (slide == 0)
                {
                    BackgroundImage = Properties.Resources.guide_00;
                    hide(btnBack);
                }
                else if (slide == 1)
                {
                    BackgroundImage = Properties.Resources.guide_01;
                    show(btnClose);
                }
                else if (slide == 2)
                {
                    BackgroundImage = Properties.Resources.guide_02;
                }
                else if (slide == 3)
                {
                    BackgroundImage = Properties.Resources.guide_03;
                }
                else if (slide == 4)
                {
                    BackgroundImage = Properties.Resources.guide_04;
                }
                else
                {
                    BackgroundImage = Properties.Resources.guide_05;
                    hide(btnNext);
                }
            }
            else
            {
                Close();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            slide--;
            changeSlide();
        }

        private void show(Control component)
        {
            component.Visible = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void hide(Control component)
        {
            component.Visible = false;
        }
    }
}