using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IcecastStreamMonitor.Controls;

public class DarkTabControl : TabControl
{
    public DarkTabControl()
    {
        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);

        DrawMode = TabDrawMode.OwnerDrawFixed;
        SizeMode = TabSizeMode.Fixed;
        ItemSize = new Size(185, 34);
        Font = new Font("Segoe UI", 9F, FontStyle.Regular);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        bool isDark = BackColor.R < 100;
        var bgColor = isDark ? Color.FromArgb(24, 24, 27) : Color.FromArgb(245, 246, 250);
        var tabSurface = isDark ? Color.FromArgb(34, 34, 38) : Color.FromArgb(230, 233, 238);
        var tabActive = isDark ? Color.FromArgb(42, 42, 48) : Color.White;
        var borderColor = isDark ? Color.FromArgb(52, 52, 58) : Color.FromArgb(209, 213, 219);
        var textActive = isDark ? Color.FromArgb(244, 244, 245) : Color.FromArgb(17, 24, 39);
        var textInactive = isDark ? Color.FromArgb(161, 161, 170) : Color.FromArgb(107, 114, 128);
        var accentColor = isDark ? Color.FromArgb(224, 108, 84) : Color.FromArgb(194, 65, 12);

        // 1. Fill entire tab header area background
        using (var bgBrush = new SolidBrush(bgColor))
        {
            g.FillRectangle(bgBrush, ClientRectangle);
        }

        if (TabCount == 0) return;

        int bottomY = GetTabRect(0).Bottom;

        // 2. Draw Tab Buttons
        for (int i = 0; i < TabCount; i++)
        {
            var tabRect = GetTabRect(i);
            bool isSelected = (SelectedIndex == i);

            // Tab button background
            using (var tabBrush = new SolidBrush(isSelected ? tabActive : tabSurface))
            {
                g.FillRectangle(tabBrush, tabRect);
            }

            // Tab border outline
            using (var borderPen = new Pen(borderColor, 1))
            {
                g.DrawRectangle(borderPen, tabRect.X, tabRect.Y, tabRect.Width, tabRect.Height);
            }

            // Top accent indicator bar for active tab
            if (isSelected)
            {
                using var accentPen = new Pen(accentColor, 3f);
                g.DrawLine(accentPen, tabRect.Left + 1, tabRect.Top + 1, tabRect.Right - 1, tabRect.Top + 1);
            }

            // Tab label text
            var page = TabPages[i];
            using var font = new Font("Segoe UI", isSelected ? 9.25F : 9F, isSelected ? FontStyle.Bold : FontStyle.Regular);
            using var textBrush = new SolidBrush(isSelected ? textActive : textInactive);
            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(page.Text, font, textBrush, tabRect, sf);
        }

        // 3. Draw dividing horizontal line under the entire tab bar
        using (var linePen = new Pen(borderColor, 1))
        {
            g.DrawLine(linePen, 0, bottomY, Width, bottomY);
        }
    }
}
