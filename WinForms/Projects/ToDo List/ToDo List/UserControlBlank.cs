﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToDo_List
{
    public partial class UserControlBlank : UserControl
    {
        public UserControlBlank()
        {
            InitializeComponent();
        }

        private void UserControlBlank_Load(object sender, EventArgs e)
        {
            
        }

        public void Days(int dayNum)
        {
            lDays.Text = dayNum.ToString();
        }
    }
}
