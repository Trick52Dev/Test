using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Media;
using System.Drawing;
using static MyApp;

public class MyApp
{
    // Windows API kullanarak mouse pozisyonunu alır
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    static void Main()
    {
        // Soru kutusu
        DialogResult dialogResult = MessageBox.Show("Programı başlatmak ister misiniz?", "Başlangıç", MessageBoxButtons.YesNo);
        if (dialogResult == DialogResult.No)
        {
            Application.Exit();
            return;
        }

        // Programı arka planda çalıştıran thread
        Thread backgroundThread = new Thread(new ThreadStart(BackgroundTask));
        backgroundThread.Start();

        // İmleci takip eden hata simgesi
        Application.Run(new CustomCursorForm());
    }

    static void BackgroundTask()
    {
        Random random = new Random();
        string[] websites = { "https://www.google.com/search?q=How+to+send+a+virus+to+my+friend%3F&oq=How+to+send+a+virus+to+my+friend%3F&gs_lcrp=EgZjaHJvbWUyBggAEEUYOTIGCAEQLhhA0gEJMTE0NTlqMGoxqAIAsAIA&sourceid=chrome&ie=UTF-8", "https://www.google.com/search?q=How+2+make+a+virus%3F&sca_esv=12afcb07d421cd6c&sxsrf=ADLYWIIXIDy7yiZOqMsNP7317g6ZFCQ9RQ%3A1725565132785&ei=zAjaZoLML5iL7NYP88njyQk&ved=0ahUKEwiC4pqGx6yIAxWYBdsEHfPkOJkQ4dUDCBA&uact=5&oq=How+2+make+a+virus%3F&gs_lp=Egxnd3Mtd2l6LXNlcnAiE0hvdyAyIG1ha2UgYSB2aXJ1cz8yBhAAGAgYHjIGEAAYCBgeSPVCUABY4UFwAHgBkAEAmAGFAaABuhWqAQQwLjIzuAEDyAEA-AEBmAIWoAKGFcICBBAjGCfCAgoQIxiABBgnGIoFwgIKEAAYgAQYQxiKBcICBBAAGAPCAgsQABiABBixAxiDAcICBRAAGIAEwgIREC4YgAQYsQMY0QMYgwEYxwHCAgUQLhiABMICDBAjGIAEGBMYJxiKBcICCxAuGIAEGLEDGIMBwgIOEC4YgAQYsQMYgwEY1ALCAggQABiABBixA8ICCBAAGIAEGMsBwgIIEC4YgAQYywHCAggQABgTGAcYHsICChAAGBMYBxgKGB6YAwCSBwQwLjIyoAffhwI&sclient=gws-wiz-serp", "https://www.google.com/search?q=How+to+hack+internet%3F&sca_esv=12afcb07d421cd6c&sxsrf=ADLYWIKBJjTPGTe-W2FaPIKuYS9aYoOobA%3A1725565176642&ei=-AjaZofjJuyL7NYP1qXCuQ0&ved=0ahUKEwiHv4-bx6yIAxXsBdsEHdaSMNcQ4dUDCBA&uact=5&oq=How+to+hack+internet%3F&gs_lp=Egxnd3Mtd2l6LXNlcnAiFUhvdyB0byBoYWNrIGludGVybmV0P0jUdlDvTliZdHAEeAGQAQCYAZgBoAGPFqoBBDAuMjO4AQPIAQD4AQGYAhKgAroOwgIKEAAYsAMY1gQYR8ICBBAjGCfCAgoQIxiABBgnGIoFwgIEEAAYA8ICCxAAGIAEGLEDGIMBwgIFEAAYgATCAhEQLhiABBixAxjRAxiDARjHAcICChAAGIAEGEMYigXCAgUQLhiABMICDBAjGIAEGBMYJxiKBcICCxAuGIAEGLEDGIMBwgIIEAAYgAQYywHCAgYQABgWGB6YAwCIBgGQBgeSBwQzLjE1oAeLew&sclient=gws-wiz-serp" };

        while (true)
        {
            // Rastgele bir süre bekle
            Thread.Sleep(random.Next(5000, 15000));

            // Rastgele web sitesini aç
            Process.Start(new ProcessStartInfo
            {
                FileName = websites[random.Next(websites.Length)],
                UseShellExecute = true
            });

            // Rastgele bir zamanda hata sesi çal
            if (random.Next(0, 10) > 7)
            {
                SystemSounds.Hand.Play();
            }
        }
    }
}

public class CustomCursorForm : Form
{
    private System.Windows.Forms.Timer timer;
    private Point lastPosition = new Point(0, 0);

    public CustomCursorForm()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.WindowState = FormWindowState.Maximized;
        this.TopMost = true;
        this.BackColor = Color.Transparent;
        this.TransparencyKey = Color.Transparent;

        timer = new System.Windows.Forms.Timer();
        timer.Interval = 1000; // 100ms'de bir güncelle
        timer.Tick += new EventHandler(UpdateCursorEffect);
        timer.Start();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.FillEllipse(Brushes.Red, lastPosition.X - 15, lastPosition.Y - 15, 30, 30); // İmlecin arkasında iz bırak
    }

    private void UpdateCursorEffect(object sender, EventArgs e)
    {
        POINT p;
        GetCursorPos(out p);
        lastPosition = new Point(p.X, p.Y);
        this.Invalidate(); // Formu yeniden çiz
    }
}
