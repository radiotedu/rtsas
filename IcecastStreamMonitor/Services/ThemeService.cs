using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IcecastStreamMonitor.Services;

public static class ThemeService
{
    // Anthropic / Claude Warm Dark Palette
    public static readonly Color DarkBg = Color.FromArgb(24, 24, 27);         // #18181B
    public static readonly Color DarkSurface = Color.FromArgb(39, 39, 42);    // #27272A
    public static readonly Color DarkCard = Color.FromArgb(45, 45, 50);       // #2D2D32
    public static readonly Color DarkInput = Color.FromArgb(30, 30, 34);      // #1E1E22
    public static readonly Color DarkBorder = Color.FromArgb(63, 63, 70);     // #3F3F46
    public static readonly Color DarkText = Color.FromArgb(244, 244, 245);    // #F4F4F5
    public static readonly Color DarkTextMuted = Color.FromArgb(161, 161, 170);// #A1A1AA
    public static readonly Color DarkAccent = Color.FromArgb(224, 108, 84);   // #E06C54 (Terracotta)
    public static readonly Color DarkAccentHover = Color.FromArgb(235, 125, 103);
    public static readonly Color DarkGridAlt = Color.FromArgb(30, 30, 35);    // Alternating grid row
    public static readonly Color DarkGridSelect = Color.FromArgb(58, 62, 78); // Grid selection
    public static readonly Color DarkGridLines = Color.FromArgb(48, 48, 56);

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

        ApplyToControls(form.Controls, isDark);
    }

    private static void ApplyToControls(Control.ControlCollection controls, bool isDark)
    {
        foreach (Control c in controls)
        {
            switch (c)
            {
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
                    foreach (ToolStripItem item in status.Items)
                    {
                        item.ForeColor = isDark ? DarkTextMuted : LightTextMuted;
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
                    tc.BackColor = isDark ? DarkSurface : LightSurface;
                    tc.ForeColor = isDark ? DarkText : LightText;
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
                    break;

                case NumericUpDown num:
                    num.BackColor = isDark ? DarkInput : LightInput;
                    num.ForeColor = isDark ? DarkText : LightText;
                    break;

                case ComboBox cb:
                    cb.BackColor = isDark ? DarkInput : LightInput;
                    cb.ForeColor = isDark ? DarkText : LightText;
                    cb.FlatStyle = FlatStyle.Flat;
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

        // Header style
        grid.ColumnHeadersDefaultCellStyle.BackColor = isDark ? DarkSurface : Color.FromArgb(243, 244, 246);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = isDark ? DarkText : LightText;
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = isDark ? DarkSurface : Color.FromArgb(243, 244, 246);
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = isDark ? DarkText : LightText;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

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
    public override Color ButtonSelectedHighlight => ThemeService.DarkCard;
    public override Color ButtonSelectedHighlightBorder => ThemeService.DarkBorder;
    public override Color ButtonPressedHighlight => ThemeService.DarkSurface;
    public override Color ButtonPressedHighlightBorder => ThemeService.DarkBorder;
    public override Color CheckBackground => ThemeService.DarkCard;
    public override Color CheckSelectedBackground => ThemeService.DarkCard;
}
