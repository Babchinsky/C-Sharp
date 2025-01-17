﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Net.Mime.MediaTypeNames;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;

namespace ToDo_List
{
    public partial class frmMain : Form
    {
        private NotifyIcon notifyIcon;
        string userEmail;
        private int month, year;
        private RadioButton[] radioBtnsDaysInCalendar = new RadioButton[42];


        List<Event> eventsInFile;
        int eventsCount;

        EventFileManager eventFileManager;
        string filepath;

        int eventsOnScreen;



        public frmMain(string email)
        {
            InitializeComponent();
            InitializeNotifyIcon();


            this.userEmail = email;


            filepath = userEmail + ".json";
            eventsOnScreen = 0;

            //eventFileManager.Wr\


            

            eventFileManager = new EventFileManager(filepath);


            if (!File.Exists(filepath))
            {
                eventFileManager.CreateEmptyFile();
                CreateNewEventsFileWithExample();
                eventsCount = 0;
            }
            else
            {
                eventsInFile = eventFileManager.ReadFile();
                eventsCount = eventsInFile.Count;
            }

            
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Загоняю каждую кнопку в массив кнопок
            for (int i = 0; i < 42; i++)
            {
                string rButtonName = "radioButton" + (i + 1);
                RadioButton rButton = Controls.Find(rButtonName, true).FirstOrDefault() as RadioButton;

                if (rButton != null)
                {
                    radioBtnsDaysInCalendar[i] = rButton;
                    //MessageBox.Show(radioBtnsDaysInCalendar[i].Name);
                }
                else
                {
                    // Обработка ошибки, если кнопка не найдена
                }
            }

            labelAccount.Text = userEmail;

            DateTime now = DateTime.Now;
            month = now.Month;
            year = now.Year;

            DisplayDays();
            DisplayEvents(eventsInFile);
        }


        private void InitializeNotifyIcon()
        {
            // Создаем экземпляр NotifyIcon
            notifyIcon = new NotifyIcon();

            // Загружаем иконку для трея (можно указать свою)
            //

            if (File.Exists("C:\\0 My Files\\.NET\\WinForms\\Projects\\ToDo List\\ToDo List\\src\\icon.ico"))
            {
                //MessageBox.Show("Yes");
                notifyIcon.Icon = new System.Drawing.Icon("C:\\0 My Files\\.NET\\WinForms\\Projects\\ToDo List\\ToDo List\\src\\icon.ico");
            }
            //else MessageBox.Show("No");

            // Добавляем контекстное меню
            ContextMenu contextMenu = new ContextMenu();
            MenuItem openMenuItem = new MenuItem("Open");
            openMenuItem.Click += OpenMenuItem_Click;
            MenuItem exitMenuItem = new MenuItem("Close");
            exitMenuItem.Click += ExitMenuItem_Click;
            contextMenu.MenuItems.Add(openMenuItem);
            contextMenu.MenuItems.Add(exitMenuItem);

            // Привязываем контекстное меню к NotifyIcon
            notifyIcon.ContextMenu = contextMenu;

            // Устанавливаем обработчик для двойного щелчка на иконке трея
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            // Развернуть приложение при выборе "Открыть" из контекстного меню
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            // Закрыть приложение при выборе "Выход" из контекстного меню
            this.Close();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            // Развернуть приложение при двойном щелчке на иконке трея
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Скрываем форму и отображаем иконку в трее при закрытии окна
            e.Cancel = true;
            this.Hide();
        }








        public void CreateNewEventsFileWithExample()
        {
            eventsInFile = new List<Event>
            {
                new Event(DateTime.Parse("2023-05-16"), "Meeting", false, true),
                new Event(DateTime.Parse("2023-05-17"), "Birthday Party", true, false),
                new Event(DateTime.Parse("2023-05-18"), "Conference", false, true)
            };

            EventFileManager eventFileManager = new EventFileManager(filepath);
            eventFileManager.WriteToFile(eventsInFile);
        }

        public void ClearDays()
        {
            LayPanDayContainer.Controls.Clear();
        }

        public void DisplayDays()
        {
            // Получаем первый день месяца
            CultureInfo culture = new CultureInfo("en-US");
            string monthName = culture.DateTimeFormat.GetMonthName(month);
            labelMonthYear.Text = monthName + " " + year;

            DateTime startOfTheMonth = new DateTime(year, month, 1);
            // Получаем кол-во дней в месяце
            int days = DateTime.DaysInMonth(year, month);
            // Конвертируем начальный день недели в int
            int dayOfTheWeek = Convert.ToInt32(startOfTheMonth.DayOfWeek.ToString("d"));



            #region pre Month, YearOfPreMonth 

            int daysInCalendar = 42;
            int preMonth = month;
            int yearOfPreMonth = year;

            if (month == 1)
            {
                yearOfPreMonth--;
                preMonth = 12;
            }
            else preMonth--;

            int nextMonth = month;
            int yearOfNextMonth = year;

            if (month == 12)
            {
                yearOfNextMonth++;
                nextMonth = 1;
            }
            else nextMonth++;

            #endregion
            int idOfRadButton = 0;

            // Дни прошлого месяца
            for (int i = 1; i < dayOfTheWeek; i++)
            {
                radioBtnsDaysInCalendar[idOfRadButton].Enabled = false;
                int daysInPreMonth = DateTime.DaysInMonth(yearOfPreMonth, preMonth);
                //radioBtnsDaysInCalendar[idOfRadButton].ForeColor = Color.FromArgb(204, 204, 179);
                radioBtnsDaysInCalendar[idOfRadButton].ForeColor = Color.Gainsboro;
                //radioBtnsDaysInCalendar[idOfRadButton].BackColor = Color.FromArgb(39, 39, 58);
                radioBtnsDaysInCalendar[idOfRadButton].BackColor = Color.Gray;
                radioBtnsDaysInCalendar[idOfRadButton++].Text = (daysInPreMonth - dayOfTheWeek + i + 1).ToString();
            }

            // Дни этого месяца
            for (int i = 1; i <= days; i++)
            {
                radioBtnsDaysInCalendar[idOfRadButton].Enabled = true;
                radioBtnsDaysInCalendar[idOfRadButton].Cursor = Cursors.Hand;
                radioBtnsDaysInCalendar[idOfRadButton].Text = i.ToString();
                radioBtnsDaysInCalendar[idOfRadButton].BackColor = Color.Gainsboro;
                radioBtnsDaysInCalendar[idOfRadButton++].ForeColor = Color.FromArgb(51, 51, 76);
            }

            int step = idOfRadButton;

            // Дни следующего месяца
            for (int i = step, j = 1; i < daysInCalendar; i++, j++)
            {
                radioBtnsDaysInCalendar[idOfRadButton].Enabled = false;
                //radioBtnsDaysInCalendar[idOfRadButton].ForeColor = Color.FromArgb(204, 204, 179);
                radioBtnsDaysInCalendar[idOfRadButton].ForeColor = Color.Gainsboro;
                //radioBtnsDaysInCalendar[idOfRadButton].BackColor = Color.FromArgb(39, 39, 58);
                radioBtnsDaysInCalendar[idOfRadButton].BackColor = Color.Gray;
                radioBtnsDaysInCalendar[idOfRadButton++].Text = j.ToString();
            }
        }
        
        public void ClearEvents()
        {
            //panelEvents.Visible = false;
            //foreach (var item in panelEvents.Controls)
            //{
            //    Panel control = item as Panel;
            //    control.Visible = false;
            //}

            // Приостанавливаем обновление макета контролов
            //this.SuspendLayout();

            //// Удаляем элементы управления
            //panelEvents.Dispose();

            panelEvents.Controls.Clear();
            eventsOnScreen = 0;
            //foreach (var item in panelEvents.Controls)
            //{
            //    Panel control = item as Panel;
            //    //messagebox.show(control.name);
            //    //control.dispose();
            //    control.Visible = false;
            //}

            //Thread.Sleep(100);
            //control2.Dispose();
            //control3.Dispose();
            //this.ResumeLayout();

            //// Возобновляем обновление макета контролов
            //this.ResumeLayout();
            
            
        }

        public void DisplayEvents(List<Event> events)
        {
            //panelEvents.Visible = false;
            //Control[] controls = new Control[events.Count];


            //this.SuspendLayout();


            foreach (var item in events)
            {
                AddEventToInterface(item.IsDone, item.Name, item.Date, item.IsFavourite);
            }

            //foreach (var item in panelEvents.Controls)
            //{
            //    Panel control = item as Panel;
            //    //MessageBox.Show(control.Name);
            //    //control.Dispose();
            //    control.Visible = true;
            //}

            //this.ResumeLayout();

            //foreach (var item in panelEvents.Controls)
            //{
            //    Panel control = item as Panel;
            //    control.Visible = true;
            //}

            //Thread.Sleep(250);
            //panelEvents.Visible = true;
            //foreach (var item in panelEvents.Controls)
            //{
            //    Panel control = item as Panel;
            //    control.Visible = true;
            //}



        }

        public void AddEventToInterface(bool isDone, string name, DateTime dateAndTime, bool isFavourite)
        {
            #region reg
            Panel panel = new Panel();
            //panel.Visible = false;



            panel.Width = 810;
            panel.Height = 40;
            panel.Location = new Point(10, eventsOnScreen * 50);
            panel.Margin = new Padding(0, 10, 0, 0);
            panel.BackColor = Color.Gainsboro;

            

            panelEvents.Controls.Add(panel);
            //panel.Dock = DockStyle.Top;

            CheckBox chkDone = new CheckBox();
            TextBox txtName = new TextBox();
            TextBox txtDate = new TextBox();
            TextBox txtTime = new TextBox();
            Button btnRemove =  new Button();
            CheckBox chkFavourite = new CheckBox();

            if (isDone) chkDone.Checked = true;
            else chkFavourite.Checked = false;
            chkDone.Location = new Point(13, 15);
            chkDone.Width = 15;
            chkDone.Height = 14;
            panel.Controls.Add(chkDone);


            
            txtName.Text = name;
            txtName.BackColor = SystemColors.Window;
            txtName.ForeColor = Color.Black;
            txtName.Location = new Point(34, 13);
            txtName.Width = 369;
            txtName.Height = 25;
            txtName.BorderStyle = BorderStyle.None;
            txtName.Multiline = true;
            txtName.BackColor = Color.Gainsboro;
            txtName.ReadOnly = true;
            //txtName.TextAlign = Tex
            panel.Controls.Add(txtName);

            txtDate.Text = dateAndTime.ToString("dd-MM-yyyy");
            txtDate.BackColor = SystemColors.Window;
            txtDate.ForeColor = Color.Black;
            txtDate.Location = new Point(409, 13);
            txtDate.Width = 134;
            txtDate.Height = 25;
            txtDate.BorderStyle = BorderStyle.None;
            txtDate.Multiline = true;
            txtDate.BackColor = Color.Gainsboro;
            panel.Controls.Add(txtDate);

            txtTime.Text = dateAndTime.ToString("t"); ;
            txtTime.BackColor = SystemColors.Window;
            txtTime.ForeColor = Color.Black;
            txtTime.Location = new Point(549, 13);
            txtTime.Width = 134;
            txtTime.Height = 25;
            txtTime.BorderStyle = BorderStyle.None;
            txtTime.Multiline = true;
            txtTime.BackColor = Color.Gainsboro;
            panel.Controls.Add(txtTime);


            btnRemove.Text = "Remove";
            btnRemove.Location = new Point(689, 9);
            btnRemove.Width = 75;
            btnRemove.Height = 25;
            //btnRemove.Cursor = Cursor.CureHand;
            panel.Controls.Add(btnRemove);


            if (isFavourite) chkFavourite.Checked = true;
            else chkFavourite.Checked = false;
            chkFavourite.Location = new Point(770, 10);
            chkFavourite.Width = 25;
            chkFavourite.Height = 25;
            chkFavourite.BackColor = Color.Gainsboro;
            
            chkFavourite.FlatAppearance.BorderSize = 0;
            //chkFavourite.Enabled = false;
            chkFavourite.Appearance = Appearance.Button;
            chkFavourite.FlatStyle = FlatStyle.Standard;
            chkFavourite.BackgroundImageLayout = ImageLayout.Stretch;

            //string imagePath = Path.Combine(System.Windows.Forms.Application.StartupPath, "src", "star_unchecked.png"); // Путь к изображению
            //string imagePath = @"src\star_unchecked.png"; // Путь к изображению относительно текущего расположения исполняемого файла

            string imagePath;
            if (isFavourite) imagePath = @"C:\0 My Files\.NET\WinForms\Projects\ToDo List\ToDo List\src\star_checked.png";
            else imagePath = @"C:\0 My Files\.NET\WinForms\Projects\ToDo List\ToDo List\src\star_unchecked.png";

            // Проверяем, существует ли файл изображения
            if (File.Exists(imagePath))
            {
                // Загружаем изображение как Bitmap
                Bitmap backgroundImage = new Bitmap(imagePath);

                // Устанавливаем фоновое изображение для checkBox1
                chkFavourite.BackgroundImage = backgroundImage;
            }
            else
            {
                // Если файл изображения не найден, можно предпринять соответствующие действия
                MessageBox.Show("Изображение не найдено!");
            }


            panel.Controls.Add(chkFavourite);


            btnRemove.Click += btnRemoveEvent_Click;
            chkDone.CheckedChanged += chkDone_CheckedChanged;
            chkFavourite.CheckedChanged += chkFavourite_CheckedChanged;

            //txtName.TextChanged += txtDateAndTime_TextChanged;
            txtDate.TextChanged += txtDateAndTime_TextChanged;
            txtTime.TextChanged += txtDateAndTime_TextChanged;
            //eventsCount++;
            eventsOnScreen++;


            
            #endregion
        }

        private void txtDateAndTime_TextChanged(object sender, EventArgs e)
        {
            TextBox selected = (TextBox)sender;
            Control parent = selected.Parent;
            Control[] controlsInPanel = new Control[6];
            

            

            for (int i = 0; i < controlsInPanel.Length; i++)
            {
                controlsInPanel[i] = parent.Controls[i];
            }

            TextBox txtDate = controlsInPanel[2] as TextBox;
            TextBox txtTime = controlsInPanel[3] as TextBox;

            //MessageBox.Show(evnt.Name.ToString());

            TimeSpan time;
            DateTime date;

            //&&
            //(txtDate.Text, "yyyy-mm--dd", CultureInfo.InvariantCulture, out time2))

            if (TimeSpan.TryParseExact(txtTime.Text, "hh\\:mm", CultureInfo.InvariantCulture, out time)
                &&
                DateTime.TryParseExact(txtDate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
                ) 
            {
                // Строка времени соответствует формату
                // Выполнение нужных операций



                // Меняю evnt в eventsInFile
                Event evnt = GetSelectedEventFromDisplay(selected);
                for (int i = 0; i < eventsInFile.Count; i++)
                {
                    if (eventsInFile[i].Name == evnt.Name)
                    {

                        eventsInFile[i].Date = DateTime.Parse(txtDate.Text + " " + txtTime.Text); ;
                    }

                }

                if (rbtnSelectedDay.Checked)
                {
                    rbtnSelectedDay_CheckedChanged(null, null);
                }



            }



            

        }

        private void chkDone_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox selected = (CheckBox)sender;
            Event evnt = GetSelectedEventFromDisplay(selected);
            //MessageBox.Show(evnt.Name.ToString());

            // Меняю evnt в eventsInFile
            for (int i = 0; i < eventsInFile.Count; i++)
            {
                if (eventsInFile[i].Name == evnt.Name && eventsInFile[i].Date == evnt.Date) eventsInFile[i].IsDone = !(eventsInFile[i].IsDone);
            }

            //ClearEvents();
            //DisplayEvents(eventsInFile);

            if (rbtnDone.Checked)
            {
                rbtnDone_CheckedChanged(null, null);
            }

        }

        private void chkFavourite_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox selected = (CheckBox)sender;
            Event evnt = GetSelectedEventFromDisplay(selected);
            //MessageBox.Show(evnt.Name.ToString());


            // Меняю evnt в eventsInFile
            for (int i = 0; i < eventsInFile.Count; i++)
            {
                if (eventsInFile[i].Name == evnt.Name && eventsInFile[i].Date == evnt.Date) eventsInFile[i].IsFavourite = !(eventsInFile[i].IsFavourite);
                
            }



            if (rbtnFavourite.Checked)
            {
                rbtnFavourite_CheckedChanged(null, null);
            }
            if (rbtnAll.Checked)
            {
                ClearEvents();
                DisplayEvents(eventsInFile);
            }
            if (rbtnSelectedDay.Checked)
            {
                rbtnSelectedDay_CheckedChanged(null, null);
            }
            if (rbtnDone.Checked)
            {
                rbtnDone_CheckedChanged(null, null);
            }
            if (rbtnPending.Checked)
            {
                rbtnPending_CheckedChanged(null, null);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            //LayPanDayContainer.Controls.Clear();

            if (month == 1)
            {
                year--;
                month = 12;
            }
            else month--;


            DisplayDays();

            //ClearEvents();

            //MessageBox.Show(string.Format(month.ToString() + " " + year.ToString()));
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //DisplayEvents(eventsInFile);

            //LayPanDayContainer.Controls.Clear();
            if (month == 12)
            {
                year++;
                month = 1;
            }
            else month++;


            DisplayDays();
            //MessageBox.Show(string.Format(month.ToString() + " " + year.ToString()));
        }

        private void rBtns_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbtnMenus_Enter(object sender, EventArgs e)
        {
            //RadioButton radioButton = (RadioButton)sender;

            //// Сохраняем исходный цвет фона
            //Color originalBackColor = radioButton.BackColor;

            //// Устанавливаем пользовательский цвет фона, чтобы предотвратить изменение при наведении
            //radioButton.BackColor = originalBackColor;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //btnAdd.Enabled = true;

            if (!String.IsNullOrEmpty(txtBox.Text)) btnAdd.Enabled = true;

            for (int i = 0; i < radioBtnsDaysInCalendar.Length; i++)
            {
                radioBtnsDaysInCalendar[i].ForeColor = Color.FromArgb(39, 39, 58);
            }
            ((RadioButton)sender as RadioButton).ForeColor = Color.Maroon;

            if (rbtnSelectedDay.Checked == true)
            {
                rbtnSelectedDay_CheckedChanged(sender,e);
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            int day = 1;


            foreach (RadioButton radio in radioBtnsDaysInCalendar)
            {
                if (radio.Checked) day = Convert.ToInt32(radio.Text);
            }


            DateTime dateTimeToAdd = new DateTime(year, month, day);

            bool newDone = false;
            bool newFav = false;

            if (rbtnFavourite.Checked) newFav = true;
            else if (rbtnDone.Checked) newDone = true;

            Event eventToAdd = new Event(dateTimeToAdd, txtBox.Text, newDone, newFav);
            
            eventsInFile.Add(eventToAdd);

            if (rbtnAll.Checked)
            {
                //AddEventToInterface(false, txtBox.Text, dateTimeToAdd, false);
                ClearEvents();
                DisplayEvents(eventsInFile);
            }
            else if (rbtnSelectedDay.Checked)
            {
                rbtnSelectedDay_CheckedChanged(null, null);
                //AddEventToInterface(false, txtBox.Text, dateTimeToAdd, false);
            }
            else if (rbtnFavourite.Checked)
            {
                rbtnFavourite_CheckedChanged(null, null);
                //AddEventToInterface(false, txtBox.Text, dateTimeToAdd, true);
            }
            else if (rbtnDone.Checked)
            {
                rbtnDone_CheckedChanged(null, null);
                //AddEventToInterface(true, txtBox.Text, dateTimeToAdd, false);
            }
            else if (rbtnPending.Checked)
            {
                rbtnPending_CheckedChanged(null, null);
                //AddEventToInterface(false, txtBox.Text, dateTimeToAdd, false);
            }

            
            txtBox.Text = string.Empty;
            btnAdd.Enabled = true;

        }

        private void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnAll.Checked == true)
            {
                ClearEvents();
                DisplayEvents(eventsInFile);
            }
        }

        private void rbtnSelectedDay_CheckedChanged(object sender, EventArgs e)
        {
            ClearEvents();

            int day = 1;// по умолчанию
            RadioButton selected;

            foreach (RadioButton radio in radioBtnsDaysInCalendar)
            {
                if (radio.Checked)
                {
                    day = Convert.ToInt32(radio.Text);
                    selected = radio;
                }
            }

            DateTime dateTime = new DateTime(year, month, day);

            List<Event> eventsPerSDay = new List<Event>();

            foreach (var item in eventsInFile)
            {
                if (item.Date.Year == dateTime.Year && item.Date.Month == dateTime.Month && item.Date.Day == dateTime.Day) eventsPerSDay.Add(item);
            }



            foreach (RadioButton radio in radioBtnsDaysInCalendar)
            {
                if (radio.Checked)
                {
                    if (rbtnSelectedDay.Checked == true)
                    {
                        DisplayEvents(eventsPerSDay);
                    }
                }
            }            
        }

        private void rbtnFavourite_CheckedChanged(object sender, EventArgs e)
        {
            ClearEvents();

            List<Event> favouriteEvents = new List<Event>();
            foreach (var item in eventsInFile)
            {
                if (item.IsFavourite == true) favouriteEvents.Add(item);
            }

            DisplayEvents(favouriteEvents);
        }

        private void rbtnDone_CheckedChanged(object sender, EventArgs e)
        {
            ClearEvents();

            List<Event> doneEvents = new List<Event>();
            foreach (var item in eventsInFile)
            {
                if (item.IsDone == true) doneEvents.Add(item);
            }

            DisplayEvents(doneEvents);
        }

        private void rbtnPending_CheckedChanged(object sender, EventArgs e)
        {
            ClearEvents();

            List<Event> pendingEvents = new List<Event>();
            foreach (var item in eventsInFile)
            {
                if (item.Date > DateTime.Now) pendingEvents.Add(item);
            }

            DisplayEvents(pendingEvents);
        }

        private Event GetSelectedEventFromDisplay(Control selected)
        {
            Control parent = selected.Parent;

            // Панель является родительской панелью
            Panel myParentPanel = (Panel)parent;
            // Выполнение нужных операций с родительской панелью
            // ...

            Control[] controlsInPanel = new Control[6];

            for (int i = 0; i < controlsInPanel.Length; i++)
            {
                controlsInPanel[i] = myParentPanel.Controls[i];
            }

            CheckBox chkDone = controlsInPanel[0] as CheckBox;
            TextBox txtName = controlsInPanel[1] as TextBox;
            TextBox txtDate = controlsInPanel[2] as TextBox;
            TextBox txtTime = controlsInPanel[3] as TextBox;
            CheckBox chkFavourite = controlsInPanel[5] as CheckBox;

            //if (chkDone.Checked == false) chkDone.Checked = true;
            //else chkDone.Checked = false;


            //string dateString = "2023-05-14";
            //string timeString = "13:14";

            //string format = "yyyy-MM-dd HH:mm";
            //DateTime dateTime = DateTime.ParseExact(txtDate.Text + " " + txtTime.Text, format, CultureInfo.InvariantCulture);
            DateTime dateTime = DateTime.Parse(txtDate.Text + " " + txtTime.Text);

            return new Event(dateTime, txtName.Text, chkDone.Checked, chkFavourite.Checked);
        }

        private void btnEditAndSave_Click(object sender, EventArgs e)
        {
            eventFileManager.WriteToFile(eventsInFile);
            MessageBox.Show("File is succesfully saved");
        }

        private void btnAcExit_Click(object sender, EventArgs e)
        {
            new frmRegister().Show();
            this.Hide();
        }

        private void txtBox_TextChanged(object sender, EventArgs e)
        {
            bool isCheck = false;
            foreach (var item in radioBtnsDaysInCalendar)
            {
                if (item.Checked) isCheck = true;
            }

            if (!String.IsNullOrEmpty(txtBox.Text) && isCheck == true )
            {
                btnAdd.Enabled = true;
            }
            else btnAdd.Enabled = false;
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimizeWindow_Click(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Minimized;
        }

        private void btnTray_Click(object sender, EventArgs e)
        {
            this.Hide(); // Скрываем форму

            // Показываем иконку в трее
            notifyIcon.Visible = true;
        }

        private void panelEvents_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnRemoveEvent_Click(object sender, EventArgs e)
        {
            Button selected = (Button)sender;
            Event evnt = GetSelectedEventFromDisplay(selected);

            //MessageBox.Show(string.Format(evnt.Date.ToString() + " " + evnt.Name + " " + evnt.IsDone.ToString() + " " + evnt.IsFavourite.ToString()));

            for (int i = 0; i < eventsInFile.Count; i++)
            {
                if (eventsInFile[i].Name == evnt.Name && eventsInFile[i].Date == evnt.Date) eventsInFile.Remove(eventsInFile[i]);
            }

            //foreach (var item in eventsInFile)
            //{
            //    if (item.Name == evnt.Name && item.Date = evnt.Date)
            //}


            //eventsInFile.Remove(evnt);

            eventsOnScreen--;

            Control parent = selected.Parent;

            // Панель является родительской панелью
            Panel myParentPanel = (Panel)parent;

            panelEvents.Controls.Remove(myParentPanel);
            

            if (rbtnAll.Checked)
            {
                ClearEvents();
                DisplayEvents(eventsInFile);
            }
            if (rbtnSelectedDay.Checked)
            {
                rbtnSelectedDay_CheckedChanged(null, null);
            }
            if (rbtnFavourite.Checked)
            {
                rbtnFavourite_CheckedChanged(null, null);
            }
            if (rbtnDone.Checked)
            {
                rbtnDone_CheckedChanged(null, null);
            }
            if (rbtnPending.Checked)
            {
                rbtnPending_CheckedChanged(null, null);
            }

        }
    }
}
