using System.Drawing.Drawing2D;
using System.Windows.Forms;
using IcecastStreamMonitor.RemoteControl.Models;

namespace IcecastStreamMonitor.RemoteControl.Controls;

public enum ChartMetricType
{
    Listeners,
    DataTransferMb,
    LatencyMs
}

public class HistoricalChartControl : Control
{
    private List<RemoteSnapshotLog> _data = new();
    private ChartMetricType _metricType = ChartMetricType.Listeners;
    private string _title = "Historical Metrics (24 Hours)";
    private Point _hoverPoint = Point.Empty;
    private bool _isHovering = false;
    private bool _isDarkMode = false;

    public HistoricalChartControl()
    {
        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);

        BackColor = Color.FromArgb(248, 249, 250);
        Font = new Font("Segoe UI", 9F, FontStyle.Regular);
    }

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public bool IsDarkMode
    {
        get => _isDarkMode;
        set
        {
            _isDarkMode = value;
            BackColor = _isDarkMode ? Color.FromArgb(24, 24, 27) : Color.FromArgb(248, 249, 250);
            Invalidate();
        }
    }

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public ChartMetricType MetricType
    {
        get => _metricType;
        set
        {
            _metricType = value;
            UpdateTitle();
            Invalidate();
        }
    }

    public void SetData(List<RemoteSnapshotLog> data)
    {
        _data = data ?? new List<RemoteSnapshotLog>();
        Invalidate();
    }

    private void UpdateTitle()
    {
        _title = _metricType switch
        {
            ChartMetricType.Listeners => "24-Hour Concurrent Listeners Timeline",
            ChartMetricType.DataTransferMb => "24-Hour Cumulative Bandwidth & Data Transfer (MB)",
            ChartMetricType.LatencyMs => "24-Hour Response Latency & Ping Timeline (ms)",
            _ => "Historical Metrics"
        };
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        _hoverPoint = e.Location;
        _isHovering = true;
        Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _isHovering = false;
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        try
        {
            int padLeft = 65;
            int padRight = 30;
            int padTop = 40;
            int padBottom = 40;

            var chartRect = new Rectangle(
                padLeft,
                padTop,
                Math.Max(10, Width - padLeft - padRight),
                Math.Max(10, Height - padTop - padBottom));

            var bgCardColor = _isDarkMode ? Color.FromArgb(30, 30, 35) : Color.White;
            var borderBoxColor = _isDarkMode ? Color.FromArgb(52, 52, 58) : Color.FromArgb(220, 224, 230);
            var gridLineColor = _isDarkMode ? Color.FromArgb(42, 42, 48) : Color.FromArgb(240, 243, 246);
            var axisTextColor = _isDarkMode ? Color.FromArgb(161, 161, 170) : Color.FromArgb(100, 110, 125);
            var titleTextColor = _isDarkMode ? Color.FromArgb(244, 244, 245) : Color.FromArgb(40, 50, 65);

            // Draw Title
            using (var titleFont = new Font("Segoe UI", 10F, FontStyle.Bold))
            using (var titleBrush = new SolidBrush(titleTextColor))
            {
                g.DrawString(_title, titleFont, titleBrush, padLeft, 12);
            }

            // Draw Chart Background
            using (var bgBrush = new SolidBrush(bgCardColor))
            using (var borderPen = new Pen(borderBoxColor, 1))
            {
                g.FillRectangle(bgBrush, chartRect);
                g.DrawRectangle(borderPen, chartRect);
            }

            if (_data.Count == 0)
            {
                using var brush = new SolidBrush(_isDarkMode ? Color.FromArgb(140, 140, 150) : Color.Gray);
                var sfCenter = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString("No 24-hour log data available yet.", Font, brush, chartRect, sfCenter);
                return;
            }

            // Determine values
            var points = _data.Select(d => new
            {
                d.Timestamp,
                Value = _metricType switch
                {
                    ChartMetricType.Listeners => (double)d.Listeners,
                    ChartMetricType.DataTransferMb => d.MegaBytesTransferred,
                    ChartMetricType.LatencyMs => (double)d.LatencyMs,
                    _ => 0.0
                }
            }).ToList();

            double minVal = 0;
            double maxVal = points.Count > 0 ? points.Max(p => p.Value) : 10;
            if (maxVal <= 0) maxVal = 10;
            maxVal = Math.Ceiling(maxVal * 1.15); // Add 15% headroom
            if (maxVal <= minVal) maxVal = minVal + 10;

            // Draw Y-Axis Grid Lines & Labels
            int yDivisions = 5;
            using var gridPen = new Pen(gridLineColor, 1);
            using var axisFont = new Font("Segoe UI", 8F, FontStyle.Regular);
            using var textBrush = new SolidBrush(axisTextColor);
            var sfRight = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };

            for (int i = 0; i <= yDivisions; i++)
            {
                float y = chartRect.Bottom - (i * (float)chartRect.Height / yDivisions);
                double val = minVal + (i * (maxVal - minVal) / yDivisions);

                if (i > 0 && i < yDivisions)
                {
                    g.DrawLine(gridPen, chartRect.Left, y, chartRect.Right, y);
                }

                string label = _metricType switch
                {
                    ChartMetricType.Listeners => $"{val:N0}",
                    ChartMetricType.DataTransferMb => $"{val:F1} MB",
                    ChartMetricType.LatencyMs => $"{val:N0} ms",
                    _ => $"{val:N0}"
                };

                g.DrawString(label, axisFont, textBrush, new RectangleF(0, y - 10, padLeft - 6, 20), sfRight);
            }

            // Draw X-Axis Time Markers safely
            int xDivisions = Math.Max(1, Math.Min(8, points.Count));
            var sfCenterBottom = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };

            if (points.Count == 1)
            {
                float x = chartRect.Left + chartRect.Width / 2f;
                g.DrawLine(gridPen, x, chartRect.Top, x, chartRect.Bottom);
                g.DrawString(points[0].Timestamp.ToString("HH:mm"), axisFont, textBrush, new RectangleF(x - 30, chartRect.Bottom + 6, 60, 20), sfCenterBottom);
            }
            else
            {
                for (int i = 0; i < xDivisions; i++)
                {
                    int dataIdx = xDivisions > 1 ? (int)(i * (points.Count - 1.0) / (xDivisions - 1)) : 0;
                    float x = chartRect.Left + (float)(dataIdx * (double)chartRect.Width / Math.Max(1.0, points.Count - 1.0));
                    var time = points[dataIdx].Timestamp;

                    g.DrawLine(gridPen, x, chartRect.Top, x, chartRect.Bottom);
                    g.DrawString(time.ToString("HH:mm"), axisFont, textBrush, new RectangleF(x - 30, chartRect.Bottom + 6, 60, 20), sfCenterBottom);
                }
            }

            // Colors based on metric type
            Color mainColor = _metricType switch
            {
                ChartMetricType.Listeners => _isDarkMode ? Color.FromArgb(56, 189, 248) : Color.FromArgb(0, 122, 255),    // Sky Blue / Blue
                ChartMetricType.DataTransferMb => _isDarkMode ? Color.FromArgb(74, 222, 128) : Color.FromArgb(46, 170, 80),// Emerald / Green
                ChartMetricType.LatencyMs => _isDarkMode ? Color.FromArgb(251, 146, 60) : Color.FromArgb(235, 115, 25),    // Coral / Orange
                _ => Color.DodgerBlue
            };

            // Convert data points to screen coordinates safely
            var screenPoints = new List<PointF>();
            for (int i = 0; i < points.Count; i++)
            {
                float x = points.Count > 1
                    ? chartRect.Left + (float)(i * (double)chartRect.Width / (points.Count - 1.0))
                    : chartRect.Left + chartRect.Width / 2f;
                float normalized = (float)((points[i].Value - minVal) / Math.Max(0.0001, maxVal - minVal));
                float y = chartRect.Bottom - (normalized * chartRect.Height);
                screenPoints.Add(new PointF(x, y));
            }

            if (screenPoints.Count >= 2)
            {
                // Draw Gradient Area Fill
                using (var path = new GraphicsPath())
                {
                    path.AddLine(screenPoints[0].X, chartRect.Bottom, screenPoints[0].X, screenPoints[0].Y);
                    for (int i = 1; i < screenPoints.Count; i++)
                    {
                        path.AddLine(screenPoints[i - 1], screenPoints[i]);
                    }
                    path.AddLine(screenPoints[^1].X, screenPoints[^1].Y, screenPoints[^1].X, chartRect.Bottom);
                    path.CloseFigure();

                    using var fillBrush = new LinearGradientBrush(
                        chartRect,
                        Color.FromArgb(_isDarkMode ? 70 : 60, mainColor),
                        Color.FromArgb(_isDarkMode ? 5 : 8, mainColor),
                        LinearGradientMode.Vertical);
                    g.FillPath(fillBrush, path);
                }

                // Draw Curve Line
                using var linePen = new Pen(mainColor, 2f);
                linePen.LineJoin = LineJoin.Round;
                for (int i = 1; i < screenPoints.Count; i++)
                {
                    g.DrawLine(linePen, screenPoints[i - 1], screenPoints[i]);
                }
            }
            else if (screenPoints.Count == 1)
            {
                // Draw single point indicator
                using var dotBrush = new SolidBrush(mainColor);
                g.FillEllipse(dotBrush, screenPoints[0].X - 4, screenPoints[0].Y - 4, 8, 8);
            }

            // Draw Average Line
            double avg = points.Count > 0 ? points.Average(p => p.Value) : 0;
            float avgY = chartRect.Bottom - (float)((avg - minVal) / Math.Max(0.0001, maxVal - minVal) * chartRect.Height);
            using (var avgPen = new Pen(_isDarkMode ? Color.FromArgb(120, 120, 130) : Color.FromArgb(140, 150, 165), 1))
            {
                avgPen.DashStyle = DashStyle.Dash;
                g.DrawLine(avgPen, chartRect.Left, avgY, chartRect.Right, avgY);
            }
            g.DrawString($"Avg: {avg:F1}", axisFont, textBrush, chartRect.Right - 70, avgY - 14);

            // Hover Crosshair & Value Tooltip
            if (_isHovering && chartRect.Contains(_hoverPoint) && screenPoints.Count > 0)
            {
                // Find closest point
                int closestIdx = 0;
                float minDistance = float.MaxValue;
                for (int i = 0; i < screenPoints.Count; i++)
                {
                    float dist = Math.Abs(screenPoints[i].X - _hoverPoint.X);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        closestIdx = i;
                    }
                }

                var cp = screenPoints[closestIdx];
                var rawPoint = points[closestIdx];

                // Vertical Crosshair Line
                using var crossPen = new Pen(_isDarkMode ? Color.FromArgb(180, 180, 190) : Color.FromArgb(80, 90, 110), 1)
                {
                    DashStyle = DashStyle.Dot
                };
                g.DrawLine(crossPen, cp.X, chartRect.Top, cp.X, chartRect.Bottom);

                // Highlight Circle on Point
                using var dotBrush = new SolidBrush(mainColor);
                using var dotPen = new Pen(Color.White, 2f);
                g.FillEllipse(dotBrush, cp.X - 5, cp.Y - 5, 10, 10);
                g.DrawEllipse(dotPen, cp.X - 5, cp.Y - 5, 10, 10);

                // Tooltip Box
                string tipVal = _metricType switch
                {
                    ChartMetricType.Listeners => $"{rawPoint.Value:N0} Listeners",
                    ChartMetricType.DataTransferMb => $"{rawPoint.Value:F2} MB Transfer",
                    ChartMetricType.LatencyMs => $"{rawPoint.Value:N0} ms Latency",
                    _ => $"{rawPoint.Value}"
                };
                string tipTime = rawPoint.Timestamp.ToString("HH:mm:ss");

                var toolTipText = $"{tipTime}\n{tipVal}";
                using var tipFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                var tipSize = g.MeasureString(toolTipText, tipFont);

                float tipX = cp.X + 10;
                float tipY = cp.Y - tipSize.Height - 8;

                if (tipX + tipSize.Width + 12 > chartRect.Right) tipX = cp.X - tipSize.Width - 16;
                if (tipY < chartRect.Top) tipY = cp.Y + 10;

                var tipRect = new RectangleF(tipX, tipY, tipSize.Width + 12, tipSize.Height + 8);

                using var tipBgBrush = new SolidBrush(_isDarkMode ? Color.FromArgb(42, 42, 48) : Color.FromArgb(30, 40, 55));
                using var tipBorderPen = new Pen(_isDarkMode ? Color.FromArgb(80, 80, 90) : Color.FromArgb(60, 70, 85), 1);
                using var tipTextBrush = new SolidBrush(Color.White);

                g.FillRectangle(tipBgBrush, tipRect);
                g.DrawRectangle(tipBorderPen, tipRect.X, tipRect.Y, tipRect.Width, tipRect.Height);
                g.DrawString(toolTipText, tipFont, tipTextBrush, tipX + 6, tipY + 4);
            }
        }
        catch (Exception ex)
        {
            using var errBrush = new SolidBrush(Color.IndianRed);
            g.DrawString($"Chart rendering note: {ex.Message}", Font, errBrush, 10, 10);
        }
    }
}
