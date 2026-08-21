using System.Drawing.Drawing2D;
using IcecastStreamMonitor.Models;

namespace IcecastStreamMonitor.Controls;

public enum ChartMetricType
{
    Listeners,
    DataTransferMb,
    LatencyMs
}

public class HistoricalChartControl : Control
{
    private List<StreamSnapshotLog> _data = new();
    private ChartMetricType _metricType = ChartMetricType.Listeners;
    private Point _hoverPoint = Point.Empty;
    private bool _isHovering = false;

    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)]
    [System.ComponentModel.DefaultValue(ChartMetricType.Listeners)]
    public ChartMetricType MetricType
    {
        get => _metricType;
        set
        {
            _metricType = value;
            Invalidate();
        }
    }

    public HistoricalChartControl()
    {
        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw,
            true);

        BackColor = Color.FromArgb(248, 249, 250);
        Font = new Font("Segoe UI", 8.25F, FontStyle.Regular);
    }

    public void SetData(List<StreamSnapshotLog> data)
    {
        _data = data ?? new List<StreamSnapshotLog>();
        Invalidate();
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

        int padLeft = 60;
        int padRight = 25;
        int padTop = 30;
        int padBottom = 40;

        var chartRect = new Rectangle(padLeft, padTop, Width - padLeft - padRight, Height - padTop - padBottom);
        if (chartRect.Width <= 10 || chartRect.Height <= 10) return;

        // Draw Chart Background
        using (var bgBrush = new SolidBrush(Color.White))
        using (var borderPen = new Pen(Color.FromArgb(220, 224, 230), 1))
        {
            g.FillRectangle(bgBrush, chartRect);
            g.DrawRectangle(borderPen, chartRect);
        }

        if (_data.Count == 0)
        {
            using var brush = new SolidBrush(Color.Gray);
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
                _ => 0
            }
        }).ToList();

        double minVal = 0;
        double maxVal = points.Max(p => p.Value);
        if (maxVal <= 0) maxVal = 10;
        maxVal = Math.Ceiling(maxVal * 1.15); // Add 15% headroom

        // Draw Y-Axis Grid Lines & Labels
        int yDivisions = 5;
        using var gridPen = new Pen(Color.FromArgb(240, 243, 246), 1);
        using var axisFont = new Font("Segoe UI", 8F, FontStyle.Regular);
        using var textBrush = new SolidBrush(Color.FromArgb(100, 110, 125));
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

        // Draw X-Axis Time Markers
        int xDivisions = Math.Min(8, points.Count);
        var sfCenterBottom = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };

        for (int i = 0; i < xDivisions; i++)
        {
            int dataIdx = (int)(i * (points.Count - 1.0) / (xDivisions - 1));
            float x = chartRect.Left + (float)(dataIdx * chartRect.Width / (points.Count - 1.0));
            var time = points[dataIdx].Timestamp;

            g.DrawLine(gridPen, x, chartRect.Top, x, chartRect.Bottom);
            g.DrawString(time.ToString("HH:mm"), axisFont, textBrush, new RectangleF(x - 30, chartRect.Bottom + 6, 60, 20), sfCenterBottom);
        }

        // Colors based on metric type
        Color mainColor = _metricType switch
        {
            ChartMetricType.Listeners => Color.FromArgb(0, 122, 255),       // Blue
            ChartMetricType.DataTransferMb => Color.FromArgb(46, 170, 80),   // Green
            ChartMetricType.LatencyMs => Color.FromArgb(235, 115, 25),      // Orange
            _ => Color.DodgerBlue
        };

        // Convert data points to screen coordinates
        var screenPoints = new List<PointF>();
        for (int i = 0; i < points.Count; i++)
        {
            float x = chartRect.Left + (float)(i * chartRect.Width / (points.Count - 1.0));
            float normalized = (float)((points[i].Value - minVal) / (maxVal - minVal));
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
                    Color.FromArgb(60, mainColor),
                    Color.FromArgb(8, mainColor),
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

        // Draw Average Line
        double avg = points.Average(p => p.Value);
        float avgY = chartRect.Bottom - (float)((avg - minVal) / (maxVal - minVal) * chartRect.Height);
        using (var avgPen = new Pen(Color.FromArgb(140, 150, 165), 1))
        {
            avgPen.DashStyle = DashStyle.Dash;
            g.DrawLine(avgPen, chartRect.Left, avgY, chartRect.Right, avgY);
        }
        g.DrawString($"Avg: {avg:F1}", axisFont, textBrush, chartRect.Right - 70, avgY - 14);

        // Hover Crosshair & Value Tooltip
        if (_isHovering && chartRect.Contains(_hoverPoint) && screenPoints.Count > 1)
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

            var pt = screenPoints[closestIdx];
            var item = points[closestIdx];

            // Draw vertical dashed line
            using (var crosshairPen = new Pen(Color.FromArgb(120, 130, 145), 1))
            {
                crosshairPen.DashStyle = DashStyle.Dot;
                g.DrawLine(crosshairPen, pt.X, chartRect.Top, pt.X, chartRect.Bottom);
            }

            // Draw highlighted point circle
            using (var dotBrush = new SolidBrush(Color.White))
            using (var dotPen = new Pen(mainColor, 3f))
            {
                g.FillEllipse(dotBrush, pt.X - 5, pt.Y - 5, 10, 10);
                g.DrawEllipse(dotPen, pt.X - 5, pt.Y - 5, 10, 10);
            }

            // Tooltip Box
            string valText = _metricType switch
            {
                ChartMetricType.Listeners => $"{item.Value:N0} Listeners",
                ChartMetricType.DataTransferMb => $"{item.Value:F2} MB Transfer",
                ChartMetricType.LatencyMs => $"{item.Value:N0} ms Latency",
                _ => $"{item.Value:N0}"
            };
            string tooltipText = $"{item.Timestamp:yyyy-MM-dd HH:mm}\n{valText}";

            var textSize = g.MeasureString(tooltipText, Font);
            float boxW = textSize.Width + 16;
            float boxH = textSize.Height + 10;
            float boxX = pt.X + 10;
            float boxY = pt.Y - boxH - 5;

            if (boxX + boxW > chartRect.Right) boxX = pt.X - boxW - 10;
            if (boxY < chartRect.Top) boxY = pt.Y + 10;

            var tipRect = new RectangleF(boxX, boxY, boxW, boxH);
            using var tipBg = new SolidBrush(Color.FromArgb(235, 33, 37, 41));
            using var tipBorder = new Pen(Color.FromArgb(200, 50, 55, 60), 1);
            using var tipTextBrush = new SolidBrush(Color.White);

            g.FillRectangle(tipBg, tipRect);
            g.DrawRectangle(tipBorder, tipRect.X, tipRect.Y, tipRect.Width, tipRect.Height);
            g.DrawString(tooltipText, Font, tipTextBrush, tipRect.X + 8, tipRect.Y + 5);
        }

        // Top Title
        string title = _metricType switch
        {
            ChartMetricType.Listeners => "24-Hour Concurrent Listeners Timeline",
            ChartMetricType.DataTransferMb => "24-Hour Estimated Bandwidth & Data Transfer (MB)",
            ChartMetricType.LatencyMs => "24-Hour Server Response Latency (Ping in ms)",
            _ => "Historical Metrics"
        };
        using var titleFont = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        using var titleBrush = new SolidBrush(Color.FromArgb(40, 50, 65));
        g.DrawString(title, titleFont, titleBrush, padLeft, 6);
    }
}
