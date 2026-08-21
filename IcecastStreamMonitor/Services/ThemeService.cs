using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using IcecastStreamMonitor.Controls;

namespace IcecastStreamMonitor.Services;

public static class ThemeService
{
    // Win32 DWM API for Native Dark Title Bar
    [DllImport("dwmapi.dll", PreserveSig = true)]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
    private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

    // Anthropic / Claude Warm Dark Palette
    public static readonly Color DarkBg = Color.FromArgb(24, 24, 27);          // #18181B (Warm Zinc)
    public static readonly Color DarkSurface = Color.FromArgb(34, 34, 38);     // #222226
    public static readonly Color DarkCard = Color.FromArgb(42, 42, 48);        // #2A2A30
    public static readonly Color DarkInput = Color.FromArgb(28, 28, 32);       // #1C1C20
    public static readonly Color DarkBorder = Color.FromArgb(52, 52, 58);      // #34343A
    public static readonly Color DarkText = Color.FromArgb(244, 244, 245);     // #F4F4F5
    public static readonly Color DarkTextMuted = Color.FromArgb(161, 161, 170);// #A1A1AA
    public static readonly Color DarkAccent = Color.FromArgb(224, 108, 84);    // #E06C54 (Terracotta)
    public static readonly Color DarkAccentHover = Color.FromArgb(235, 125, 103);
    public static readonly Color DarkGridAlt = Color.FromArgb(28, 28, 32);     // Alternating grid row
    public static readonly Color DarkGridSelect = Color.FromArgb(52, 58, 74);  // Grid selection
    public static readonly Color DarkGridLines = Color.FromArgb(40, 40, 46);

    // Clean Light Palette
    public static readonly Color LightBg = Color.FromArgb(245, 246, 250);
    public static readonly Color LightSurface = Color.White;
    public static readonly Color LightCard = Color.FromArgb(240, 242, 245);
    public static readonly Color LightInput = Color.White;
    public static readonly Color LightBorder = Color.FromArgb(209, 213, 219);
    public static readonly Color LightText = Color.FromArgb(17, 24, 39);
    public static readonly Color LightTextMuted = Color.FromArgb(107, 114, 128);
    public static readonly Color LightAccent = Color.FromArgb(194, 65, 12);
    public static readonly Color LightGridAlt = Color.FromArgb(249, 250, 251);
    public static readonly Color LightGridSelect = Color.FromArgb(219, 234, 254);
    public static readonly Color LightGridLines = Color.FromArgb(229, 231, 235);

    public static void ApplyTheme(Form form, bool isDark)
    {
        form.BackColor = isDark ? DarkBg : LightBg;
        form.ForeColor = isDark ? DarkText : LightText;

        // Apply native Windows 10/11 Dark Title Bar & Window Border
        SetWindowDarkMode(form.Handle, isDark);

        ApplyToControls(form.Controls, isDark);
    }

    public static void SetWindowDarkMode(IntPtr handle, bool isDark)
    {
        try
        {
            if (Environment.OSVersion.Version.Major >= 10 && handle != IntPtr.Zero)
            {
                int useDarkMode = isDark ? 1 : 0;
                int res = DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDarkMode, sizeof(int));
                if (res != 0)
                {
                    DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, ref useDarkMode, sizeof(int));
                }
            }
        }
        catch { }
    }

    public static void ApplyToControls(Control.ControlCollection controls, bool isDark)
    {
        foreach (Control c in controls)
        {
            switch (c)
            {
                case HistoricalChartControl chart:
                    chart.IsDarkMode = isDark;
                    break;

                case MenuStrip menu:
                    menu.Renderer = isDark ? new DarkMenuRenderer() : new ToolStripProfessionalRenderer();
                    menu.BackColor = isDark ? DarkSurface : SystemColors.Control;
                    menu.ForeColor = isDark ? DarkText : LightText;
                    foreach (ToolStripItem item in menu.Items)
                    {
                        ApplyToToolStripItem(item, isDark);
                    }
                    break;

                case StatusStrip status:
                    status.Renderer = isDark ? new DarkMenuRenderer() : new ToolStripProfessionalRenderer();
                    status.BackColor = isDark ? DarkInput : SystemColors.Control;
                    status.ForeColor = isDark ? DarkTextMuted : LightTextMuted;
                    status.SizingGrip = false;
                    foreach (ToolStripItem item in status.Items)
                    {
                        item.ForeColor = isDark ? DarkTextMuted : LightTextMuted;
                        if (item is ToolStripStatusLabel lbl)
                        {
                            lbl.BorderStyle = Border3DStyle.Flat;
                            lbl.BorderSides = ToolStripStatusLabelBorderSides.None;
                        }
                    }
                    break;

                case ToolStrip tool:
                    tool.Renderer = isDark ? new DarkMenuRenderer() : new ToolStripProfessionalRenderer();
                    tool.BackColor = isDark ? DarkSurface : SystemColors.Control;
                    tool.ForeColor = isDark ? DarkText : LightText;
                    foreach (ToolStripItem item in tool.Items)
                    {
                        ApplyToToolStripItem(item, isDark);
                    }
                    break;

                case SplitContainer sc:
                    sc.BackColor = isDark ? DarkBorder : LightBorder;
                    sc.Panel1.BackColor = isDark ? DarkBg : LightBg;
                    sc.Panel2.BackColor = isDark ? DarkBg : LightBg;
                    ApplyToControls(sc.Panel1.Controls, isDark);
                    ApplyToControls(sc.Panel2.Controls, isDark);
                    break;

                case DataGridView grid:
                    ApplyToGrid(grid, isDark);
                    break;

                case GroupBox gb:
                    gb.BackColor = isDark ? DarkSurface : LightSurface;
                    gb.ForeColor = isDark ? DarkText : LightText;
                    ApplyToControls(gb.Controls, isDark);
                    break;

                case Panel p:
                    p.BackColor = isDark ? (p.Tag?.ToString() == "card" ? DarkCard : DarkSurface) : (p.Tag?.ToString() == "card" ? LightCard : LightSurface);
                    p.ForeColor = isDark ? DarkText : LightText;
                    ApplyToControls(p.Controls, isDark);
                    break;

                case TabControl tc:
                    tc.DrawMode = TabDrawMode.OwnerDrawFixed;
                    tc.ItemSize = new Size(Math.Max(120, tc.ItemSize.Width), 28);
                    tc.SizeMode = TabSizeMode.Normal;
                    tc.BackColor = isDark ? DarkBg : LightBg;
                    tc.ForeColor = isDark ? DarkText : LightText;
                    tc.DrawItem -= TabControl_DrawItem;
                    tc.DrawItem += TabControl_DrawItem;
                    foreach (TabPage tp in tc.TabPages)
                    {
                        tp.BackColor = isDark ? DarkBg : LightBg;
                        tp.ForeColor = isDark ? DarkText : LightText;
                        ApplyToControls(tp.Controls, isDark);
                    }
                    break;

                case Button btn:
                    if (btn.Tag?.ToString() == "primary")
                    {
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.BorderSize = 0;
                        btn.BackColor = isDark ? DarkAccent : LightAccent;
                        btn.ForeColor = Color.White;
                    }
                    else
                    {
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.BorderColor = isDark ? DarkBorder : LightBorder;
                        btn.BackColor = isDark ? DarkCard : LightSurface;
                        btn.ForeColor = isDark ? DarkText : LightText;
                    }
                    break;

                case TextBox tb:
                    tb.BackColor = isDark ? DarkInput : LightInput;
                    tb.ForeColor = isDark ? DarkText : LightText;
                    tb.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case NumericUpDown num:
                    num.BackColor = isDark ? DarkInput : LightInput;
                    num.ForeColor = isDark ? DarkText : LightText;
                    num.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case ComboBox cb:
                    cb.BackColor = isDark ? DarkInput : LightInput;
                    cb.ForeColor = isDark ? DarkText : LightText;
                    cb.FlatStyle = FlatStyle.Flat;
                    SetupDarkComboBox(cb, isDark);
                    break;

                case CheckBox chk:
                    chk.ForeColor = isDark ? DarkText : LightText;
                    break;

                case RadioButton rb:
                    rb.ForeColor = isDark ? DarkText : LightText;
                    break;

                case Label lbl:
                    if (lbl.Tag?.ToString() == "muted")
                    {
                        lbl.ForeColor = isDark ? DarkTextMuted : LightTextMuted;
                    }
                    else if (lbl.Tag?.ToString() == "accent")
                    {
                        lbl.ForeColor = isDark ? DarkAccent : LightAccent;
                    }
                    else
                    {
                        lbl.ForeColor = isDark ? DarkText : LightText;
                    }
                    break;

                default:
                    if (c.HasChildren)
                    {
                        ApplyToControls(c.Controls, isDark);
                    }
                    break;
            }
        }
    }

    private static void TabControl_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (sender is not TabControl tc || e.Index < 0 || e.Index >= tc.TabPages.Count) return;

        var page = tc.TabPages[e.Index];
        bool isSelected = (e.State & DrawItemState.Selected) != 0;
        bool isDark = tc.BackColor.R < 100;

        var tabRect = tc.GetTabRect(e.Index);
        var bg = isSelected
            ? (isDark ? DarkCard : LightSurface)
            : (isDark ? DarkSurface : Color.FromArgb(235, 238, 242));
        var fg = isSelected
            ? (isDark ? DarkText : LightText)
            : (isDark ? DarkTextMuted : LightTextMuted);

        using (var brush = new SolidBrush(bg))
        {
            e.Graphics.FillRectangle(brush, tabRect);
        }

        // Draw top active accent line for selected tab
        if (isSelected)
        {
            using var accentPen = new Pen(isDark ? DarkAccent : LightAccent, 2f);
            e.Graphics.DrawLine(accentPen, tabRect.Left, tabRect.Top, tabRect.Right, tabRect.Top);
        }

        // Draw subtle outline
        using (var borderPen = new Pen(isDark ? DarkBorder : LightBorder, 1))
        {
            e.Graphics.DrawRectangle(borderPen, tabRect);
        }

        // Draw text
        using var font = new Font(tc.Font, isSelected ? FontStyle.Bold : FontStyle.Regular);
        using var textBrush = new SolidBrush(fg);
        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        e.Graphics.DrawString(page.Text, font, textBrush, tabRect, sf);
    }

    private static void SetupDarkComboBox(ComboBox cb, bool isDark)
    {
        cb.DrawMode = DrawMode.OwnerDrawFixed;
        cb.DrawItem -= ComboBox_DrawItem;
        cb.DrawItem += ComboBox_DrawItem;
    }

    private static void ComboBox_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0 || sender is not ComboBox cb) return;

        bool isDark = cb.BackColor.R < 100;
        var bg = (e.State & DrawItemState.Selected) != 0
            ? (isDark ? DarkGridSelect : LightGridSelect)
            : (isDark ? DarkInput : LightInput);
        var fg = (e.State & DrawItemState.Selected) != 0
            ? Color.White
            : (isDark ? DarkText : LightText);

        using var brush = new SolidBrush(bg);
        e.Graphics.FillRectangle(brush, e.Bounds);

        var text = cb.Items[e.Index]?.ToString() ?? "";
        using var fontBrush = new SolidBrush(fg);
        var sf = new StringFormat { LineAlignment = StringAlignment.Center };
        var textBounds = new Rectangle(e.Bounds.X + 4, e.Bounds.Y, e.Bounds.Width - 6, e.Bounds.Height);
        e.Graphics.DrawString(text, cb.Font, fontBrush, textBounds, sf);

        e.DrawFocusRectangle();
    }

    private static void ApplyToToolStripItem(ToolStripItem item, bool isDark)
    {
        item.ForeColor = isDark ? DarkText : LightText;
        if (item is ToolStripDropDownItem dropDown)
        {
            dropDown.DropDown.BackColor = isDark ? DarkSurface : LightSurface;
            dropDown.DropDown.ForeColor = isDark ? DarkText : LightText;
            foreach (ToolStripItem subItem in dropDown.DropDownItems)
            {
                ApplyToToolStripItem(subItem, isDark);
            }
        }
    }

    private static void ApplyToGrid(DataGridView grid, bool isDark)
    {
        grid.EnableHeadersVisualStyles = false;
        grid.BackgroundColor = isDark ? DarkBg : LightBg;
        grid.GridColor = isDark ? DarkGridLines : LightGridLines;
        grid.BorderStyle = BorderStyle.None;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

        // Header style
        grid.ColumnHeadersDefaultCellStyle.BackColor = isDark ? DarkSurface : Color.FromArgb(243, 244, 246);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = isDark ? DarkText : LightText;
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = isDark ? DarkSurface : Color.FromArgb(243, 244, 246);
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = isDark ? DarkText : LightText;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

        // Rows style
        grid.DefaultCellStyle.BackColor = isDark ? DarkBg : LightSurface;
        grid.DefaultCellStyle.ForeColor = isDark ? DarkText : LightText;
        grid.DefaultCellStyle.SelectionBackColor = isDark ? DarkGridSelect : LightGridSelect;
        grid.DefaultCellStyle.SelectionForeColor = isDark ? Color.White : Color.Black;

        // Alternating Rows
        grid.AlternatingRowsDefaultCellStyle.BackColor = isDark ? DarkGridAlt : LightGridAlt;
        grid.AlternatingRowsDefaultCellStyle.ForeColor = isDark ? DarkText : LightText;
        grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = isDark ? DarkGridSelect : LightGridSelect;
        grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = isDark ? Color.White : Color.Black;

        // Row headers
        grid.RowHeadersDefaultCellStyle.BackColor = isDark ? DarkSurface : Color.FromArgb(243, 244, 246);
        grid.RowHeadersDefaultCellStyle.ForeColor = isDark ? DarkTextMuted : LightTextMuted;
    }
}

public class DarkMenuRenderer : ToolStripProfessionalRenderer
{
    public DarkMenuRenderer() : base(new DarkColorTable()) { }

    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
    {
        e.TextColor = e.Item.Selected ? Color.White : ThemeService.DarkText;
        base.OnRenderItemText(e);
    }

    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    {
        // Suppress default 3D / white borders
        using var pen = new Pen(ThemeService.DarkBorder, 1);
        if (e.ToolStrip is StatusStrip)
        {
            e.Graphics.DrawLine(pen, 0, 0, e.ToolStrip.Width, 0);
        }
        else if (e.ToolStrip is MenuStrip || e.ToolStrip is ToolStrip)
        {
            e.Graphics.DrawLine(pen, 0, e.ToolStrip.Height - 1, e.ToolStrip.Width, e.ToolStrip.Height - 1);
        }
        else
        {
            e.Graphics.DrawRectangle(pen, 0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
        }
    }

    protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
    {
        var bounds = new Rectangle(Point.Empty, e.Item.Size);
        if (e.Vertical)
        {
            int midX = bounds.Width / 2;
            using var pen = new Pen(ThemeService.DarkBorder);
            e.Graphics.DrawLine(pen, midX, 4, midX, bounds.Height - 4);
        }
        else
        {
            int midY = bounds.Height / 2;
            using var pen = new Pen(ThemeService.DarkBorder);
            e.Graphics.DrawLine(pen, 4, midY, bounds.Width - 4, midY);
        }
    }

    protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
    {
        // Suppress grip to maintain clean modern look
    }
}

public class DarkColorTable : ProfessionalColorTable
{
    public override Color MenuBorder => ThemeService.DarkBorder;
    public override Color MenuItemBorder => ThemeService.DarkBorder;
    public override Color MenuItemSelected => ThemeService.DarkCard;
    public override Color MenuItemSelectedGradientBegin => ThemeService.DarkCard;
    public override Color MenuItemSelectedGradientEnd => ThemeService.DarkCard;
    public override Color MenuItemPressedGradientBegin => ThemeService.DarkSurface;
    public override Color MenuItemPressedGradientEnd => ThemeService.DarkSurface;
    public override Color MenuStripGradientBegin => ThemeService.DarkSurface;
    public override Color MenuStripGradientEnd => ThemeService.DarkSurface;
    public override Color ToolStripDropDownBackground => ThemeService.DarkSurface;
    public override Color ImageMarginGradientBegin => ThemeService.DarkSurface;
    public override Color ImageMarginGradientMiddle => ThemeService.DarkSurface;
    public override Color ImageMarginGradientEnd => ThemeService.DarkSurface;
    public override Color SeparatorDark => ThemeService.DarkBorder;
    public override Color SeparatorLight => ThemeService.DarkSurface;
    public override Color StatusStripGradientBegin => ThemeService.DarkInput;
    public override Color StatusStripGradientEnd => ThemeService.DarkInput;
    public override Color ToolStripGradientBegin => ThemeService.DarkSurface;
    public override Color ToolStripGradientMiddle => ThemeService.DarkSurface;
    public override Color ToolStripGradientEnd => ThemeService.DarkSurface;
    public override Color ToolStripBorder => ThemeService.DarkBorder;
    public override Color ButtonSelectedHighlight => ThemeService.DarkCard;
    public override Color ButtonSelectedHighlightBorder => ThemeService.DarkBorder;
    public override Color ButtonPressedHighlight => ThemeService.DarkSurface;
    public override Color ButtonPressedHighlightBorder => ThemeService.DarkBorder;
    public override Color ButtonCheckedHighlight => ThemeService.DarkCard;
    public override Color ButtonCheckedHighlightBorder => ThemeService.DarkAccent;
    public override Color CheckBackground => ThemeService.DarkCard;
    public override Color CheckSelectedBackground => ThemeService.DarkCard;
}
