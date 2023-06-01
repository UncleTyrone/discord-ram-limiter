using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace DiscordRamLimiter.CControls
{
    public class ToggleButton : CheckBox
    {
        // Fields
        private Color onBackColor = Color.FromArgb(68, 81, 222);
        private Color onToggleColor = Color.GhostWhite;
        private Color offBackColor = Color.FromArgb(48, 61, 202);
        private Color offToggleColor = Color.WhiteSmoke;

        // Constructor
        public ToggleButton()
        {
            this.MinimumSize = new Size(45, 22);
        }

        // Methods
        private GraphicsPath GetFigurePath()
        {
            int arcSize = this.Height - 1;
            Rectangle leftArc = new Rectangle(0, 0, arcSize, arcSize);
            Rectangle rightArc = new Rectangle(this.Width-arcSize-2, 0, arcSize, arcSize);

            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(leftArc, 90, 180);
            path.AddArc(rightArc, 270, 180);
            path.CloseFigure();

            return path;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            int toggleSize = this.Height - 5;
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pevent.Graphics.Clear(this.Parent.BackColor);

            if (this.Checked) // ON
            {
                // Control Surface
                pevent.Graphics.FillPath(new SolidBrush(onBackColor), GetFigurePath());
                // Toggle
                pevent.Graphics.FillEllipse(new SolidBrush(onToggleColor), new Rectangle(this.Width - this.Height + 1, 2, toggleSize, toggleSize));
            }
            else // OFF
            {
                // Control Surface
                pevent.Graphics.FillPath(new SolidBrush(offBackColor), GetFigurePath());
                // Toggle
                pevent.Graphics.FillEllipse(new SolidBrush(offToggleColor), new Rectangle(2, 2, toggleSize, toggleSize));
            }
        }
    }
}
