using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RUDP.Test.Helper.Windows
{
	/// <summary>
	/// Plotter control with scrolling.
	/// Use the <see cref="Control.ForeColor"/> property to set the plot color.
	/// </summary>
	public partial class Plotter : Control
	{
		/// <summary>
		/// </summary>
		public Plotter()
		{
			InitializeComponent();

			this.Controls.Add(this.plotArea);
			this.Controls.Add(this.scrollBar);

			this.plotArea.Paint += new PaintEventHandler(plotArea_Paint);
			this.scrollBar.Scroll += new ScrollEventHandler(scrollBar_Scroll);
			this.plotArea.Resize += delegate { this.SetScrollBarProperties(); };

			this.LayoutControls();
			this.Resize += delegate { this.LayoutControls(); };

			this.BackColorChanged += delegate { this.plotArea.Invalidate(); };
			this.ForeColorChanged += delegate { this.plotArea.Invalidate(); };
		}

		void LayoutControls()
		{
			//this.plotArea.Dock = DockStyle.Fill;
			//this.scrollBar.Dock = DockStyle.Bottom;

			// I had to lay out the controls manually because docking works slightly incorrectly
			// under certain circumstances.
			this.plotArea.Location = new Point(0, 0);
			this.plotArea.Size = new Size(this.Width, this.scrollBar.Visible ? this.Height - this.scrollBar.Height : this.Height);
			this.scrollBar.Location = new Point(0, this.plotArea.Height);
			this.scrollBar.Size = new Size(this.Width, this.scrollBar.Height);
		}

		void plotArea_Paint(object sender, PaintEventArgs e)
		{
			this.PaintGraph(e.Graphics);
		}

		void scrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.NewValue == e.OldValue) return;

			this.plotArea.Invalidate();
		}

		void SetScrollBarProperties()
		{
			bool scrollBarWasVisible = scrollBar.Visible;

			int largeChange = (int)(this.plotArea.Width / this.DX);

			if (largeChange >= this.values.Count)
			{
				this.scrollBar.Visible = false;
			}
			else
			{
				int shift = this.scrollBar.Maximum - (this.scrollBar.Value + this.scrollBar.LargeChange);
				if (shift < 0) shift = 0;

				this.scrollBar.Visible = true;
				this.scrollBar.Minimum = 0;
				this.scrollBar.Maximum = this.values.Count;
				this.scrollBar.LargeChange = largeChange;
				this.scrollBar.SmallChange = 1;

				if (!scrollBarWasVisible)
				{
					this.scrollBar.Value = this.scrollBar.Maximum - largeChange;
				}
				else
				{
					int value = this.scrollBar.Maximum - shift - this.scrollBar.LargeChange;
					if (value < 0) value = 0;
					this.scrollBar.Value = value;
				}
			}

			if (scrollBarWasVisible != this.scrollBar.Visible)
			{
				// BUG: the RunningGraph control is not laid out until it is resized.
				// Calling PerformLayout does not solve this, and neither does
				// removing/adding the scroll bar.

				this.LayoutControls();

				//if (this.scrollBar.Visible)
				//{
				//    this.Controls.Add (this.scrollBar);
				//}
				//else
				//{
				//    this.Controls.Remove (this.scrollBar);
				//}
				//this.UpdateBounds ();
			}
		}

		readonly PlotArea plotArea = new PlotArea();
		readonly HScrollBar scrollBar = new HScrollBar();

		#region Min

		/// <summary>
		/// Gets or sets the sample value that corresponds to the bottom edge of the plot area.
		/// </summary>
		public float Min
		{
			get { return this.min; }

			set
			{
				this.min = value;
				this.InvalidateAndUpdateScrollBar();
			}
		}

		float min = 0;

		#endregion
		#region Max

		/// <summary>
		/// Gets or sets the sample value that corresponds to the top edge of the plot area.
		/// </summary>
		public float Max
		{
			get { return this.max; }

			set
			{
				this.max = value;
				this.InvalidateAndUpdateScrollBar();
			}
		}

		float max = 100;

		#endregion

		/// <summary>
		/// Add a sample point to the graph.
		/// </summary>
		public void Add(float value)
		{
			this.values.Add(value);
			this.InvalidateAndUpdateScrollBar();
		}

		public void InvalidateAndUpdateScrollBar()
		{
			this.SetScrollBarProperties();
			this.plotArea.Invalidate();
		}

		/// <summary>
		/// Gets the distance between sample points.
		/// TODO: allow to customize this.
		/// </summary>
		public float DX
		{
			get { return 1F; }
		}

		readonly RingBuffer<float> values = new RingBuffer<float>(1000);

		void PaintGraph(Graphics g)
		{
			if (this.values.Count == 0) return;

			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

			float dx = this.DX;

			float x = this.plotArea.Width;

			int i = this.values.Count - 1;
			if (this.scrollBar.Visible)
			{
				i = this.scrollBar.Value + this.scrollBar.LargeChange - 1;
				if (i >= this.values.Count)
				{
					i = this.values.Count - 1;
				}
			}

			PointF p = new PointF(x, ScaleY(this.values[i]));

			for (; ; )
			{
				if (--i < 0) break;
				x -= dx;

				PointF q = new PointF(x, ScaleY(this.values[i]));

				using (Pen pen = new Pen(this.ForeColor))
				{
					g.DrawLine(pen, p, q);
				}

				p = q;
				if (x < 0) break;
			}
		}

		float ScaleY(float y)
		{
			int h = this.plotArea.Height;
			return h - (y - min) / (max - min) * h;
		}
	}

	class PlotArea : Control
	{
		public PlotArea()
		{
			this.SetStyle(
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.ResizeRedraw |
				ControlStyles.AllPaintingInWmPaint,
				true);
		}
	}

	class RingBuffer<T>
	{
		public RingBuffer(int initialCapacity)
		{
			this.buf = new T[initialCapacity];
		}

		public int Capacity
		{
			get { return this.buf.Length; }
		}

		T[] buf;
		int head = 0, tail = 0;
		bool empty = true;

		public int Count
		{
			get
			{
				if (empty) return 0;

				int count = head - tail;
				if (count <= 0) count += buf.Length;
				return count;
			}
		}

		public void Add(T value)
		{
			if (!empty && head == tail)
			{
				tail = (tail + 1) % buf.Length;
			}

			buf[head] = value;

			head = (head + 1) % buf.Length;

			empty = false;
		}

		public T this[int i]
		{
			get { return buf[ShiftIndex(i)]; }
		}

		int ShiftIndex(int i)
		{
			if (i > Count) throw new ArgumentException("Index must be less than Count.");

			return (tail + i) % buf.Length;
		}
	}
}
