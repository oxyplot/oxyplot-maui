using OxyPlot.Maui.Skia.Core;

namespace OxyPlot.Maui.Skia
{
    public abstract partial class PlotViewBase : BaseTemplatedView<Grid>, IPlotView
    {
        private int mainThreadId = 1;

        protected override void OnControlInitialized(Grid control)
        {
            this.grid = control;
            mainThreadId = Thread.CurrentThread.ManagedThreadId;
            ApplyTemplate();
            AddTouchEffect();
        }

        /// <summary>
        /// The grid.
        /// </summary>
        protected Grid grid;

        /// <summary>
        /// The plot presenter.
        /// </summary>
        protected View plotPresenter;

        /// <summary>
        /// The render context
        /// </summary>
        protected IRenderContext renderContext;

        /// <summary>
        /// The model lock.
        /// </summary>
        private readonly object modelLock = new object();

        /// <summary>
        /// The current tracker.
        /// </summary>
        private View currentTracker;

        /// <summary>
        /// The current tracker template.
        /// </summary>
        private ControlTemplate currentTrackerTemplate;

        /// <summary>
        /// The default plot controller.
        /// </summary>
        private IPlotController defaultController;

        /// <summary>
        /// Indicates whether the <see cref="PlotViewBase"/> was in the visual tree the last time <see cref="Render"/> was called.
        /// </summary>
        private bool isInVisualTree;

        /// <summary>
        /// The overlays.
        /// </summary>
        private AbsoluteLayout overlays;

        /// <summary>
        /// The zoom control.
        /// </summary>
        private ContentView zoomControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotViewBase" /> class.
        /// </summary>
        protected PlotViewBase()
        {
            this.TrackerDefinitions = new ObservableCollection<TrackerDefinition>();

            DefaultTrackerTemplate = new ControlTemplate(() =>
            {
                var tc = new TrackerControl();
                tc.SetBinding(TrackerControl.PositionProperty, "Position");
                tc.SetBinding(TrackerControl.LineExtentsProperty, "PlotModel.PlotArea");
                tc.Content = TrackerControl.DefaultTrackerTemplateContentProvider();
                return tc;
            });
            this.LayoutChanged += this.OnLayoutUpdated;
        }

        /// <summary>
        /// Gets the actual PlotView controller.
        /// </summary>
        /// <value>The actual PlotView controller.</value>
        public IPlotController ActualController => this.Controller ?? (this.defaultController ??= new OxyPlot.PlotController());

        /// <inheritdoc/>
        IController IView.ActualController => this.ActualController;

        /// <summary>
        /// Gets the actual model.
        /// </summary>
        /// <value>The actual model.</value>
        public PlotModel ActualModel { get; private set; }

        /// <inheritdoc/>
        Model IView.ActualModel => this.ActualModel;

        /// <summary>
        /// Gets the coordinates of the client area of the view.
        /// </summary>
        public OxyRect ClientArea => new OxyRect(0, 0, this.Width, this.Height);

        /// <summary>
        /// Gets the tracker definitions.
        /// </summary>
        /// <value>The tracker definitions.</value>
        public ObservableCollection<TrackerDefinition> TrackerDefinitions { get; }

        /// <summary>
        /// Hides the tracker.
        /// </summary>
        public void HideTracker()
        {
            if (this.currentTracker != null)
            {
                this.overlays.Children.Remove(this.currentTracker);
                this.currentTracker = null;
                this.currentTrackerTemplate = null;
            }
        }

        /// <summary>
        /// Hides the zoom rectangle.
        /// </summary>
        public void HideZoomRectangle()
        {
            this.zoomControl.IsVisible = false;
        }

        /// <summary>
        /// Invalidate the PlotView (not blocking the UI thread)
        /// </summary>
        /// <param name="updateData">The update Data.</param>
        public void InvalidatePlot(bool updateData = true)
        {
            if (this.ActualModel == null)
            {
                return;
            }

            lock (this.ActualModel.SyncRoot)
            {
                ((IPlotModel)this.ActualModel).Update(updateData);
            }

            this.BeginInvoke(this.Render);
        }

        private void ApplyTemplate()
        {
            if (this.grid == null)
            {
                return;
            }

            this.plotPresenter = this.CreatePlotPresenter();
            this.grid.Children.Add(this.plotPresenter);

            this.renderContext = this.CreateRenderContext();

            this.overlays = new AbsoluteLayout();
            this.grid.Children.Add(this.overlays);

            this.zoomControl = new ContentView();
            this.overlays.Children.Add(this.zoomControl);
        }

        /// <summary>
        /// Pans all axes.
        /// </summary>
        public void PanAllAxes(double deltaX, double deltaY)
        {
            if (this.ActualModel != null)
            {
                this.ActualModel.PanAllAxes(deltaX, deltaY);
            }

            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Resets all axes.
        /// </summary>
        public void ResetAllAxes()
        {
            if (this.ActualModel != null)
            {
                this.ActualModel.ResetAllAxes();
            }

            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Stores text on the clipboard.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetClipboardText(string text)
        {
            Clipboard.SetTextAsync(text);
        }

        /// <summary>
        /// Sets the cursor type.
        /// </summary>
        /// <param name="cursorType">The cursor type.</param>
        public void SetCursorType(CursorType cursorType)
        {
        }

        /// <summary>
        /// Shows the tracker.
        /// </summary>
        /// <param name="trackerHitResult">The tracker data.</param>
        public void ShowTracker(TrackerHitResult trackerHitResult)
        {
            if (trackerHitResult == null)
            {
                this.HideTracker();
                return;
            }

            var trackerTemplate = this.DefaultTrackerTemplate;
            if (trackerHitResult.Series != null && !string.IsNullOrEmpty(trackerHitResult.Series.TrackerKey))
            {
                var match = this.TrackerDefinitions.FirstOrDefault(t => t.TrackerKey == trackerHitResult.Series.TrackerKey);
                if (match != null)
                {
                    trackerTemplate = match.TrackerTemplate;
                }
            }

            if (trackerTemplate == null)
            {
                this.HideTracker();
                return;
            }

            if (!ReferenceEquals(trackerTemplate, this.currentTrackerTemplate))
            {
                this.HideTracker();

                var tracker = (ContentView)trackerTemplate.CreateContent();
                this.overlays.Children.Add(tracker);
                AbsoluteLayout.SetLayoutBounds(tracker, new Rect(0, 0, 0, 0));
                this.currentTracker = tracker;
                this.currentTrackerTemplate = trackerTemplate;
            }

            if (this.currentTracker != null)
            {
                this.currentTracker.BindingContext = trackerHitResult;
            }
        }

        /// <summary>
        /// Shows the zoom rectangle.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        public void ShowZoomRectangle(OxyRect r)
        {
            this.zoomControl.WidthRequest = r.Width;
            this.zoomControl.HeightRequest = r.Height;

            AbsoluteLayout.SetLayoutBounds(zoomControl,
                new Rect(r.Left, r.Top, r.Width, r.Height));

            this.zoomControl.ControlTemplate = this.ZoomRectangleTemplate;
            this.zoomControl.IsVisible = true;
        }

        /// <summary>
        /// Zooms all axes.
        /// </summary>
        /// <param name="factor">The zoom factor.</param>
        public void ZoomAllAxes(double factor)
        {
            if (this.ActualModel != null)
            {
                this.ActualModel.ZoomAllAxes(factor);
            }

            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Clears the background of the plot presenter.
        /// </summary>
        protected abstract void ClearBackground();

        /// <summary>
        /// Creates the plot presenter.
        /// </summary>
        /// <returns>The plot presenter.</returns>
        protected abstract View CreatePlotPresenter();

        /// <summary>
        /// Creates the render context.
        /// </summary>
        /// <returns>The render context.</returns>
        protected abstract IRenderContext CreateRenderContext();

        /// <summary>
        /// Called when the model is changed.
        /// </summary>
        protected void OnModelChanged()
        {
            lock (this.modelLock)
            {
                if (this.ActualModel != null)
                {
                    ((IPlotModel)this.ActualModel).AttachPlotView(null);
                    this.ActualModel = null;
                }

                if (this.Model != null)
                {
                    IPlotModel plotModel = this.Model;
                    var oldPlotView = this.Model.PlotView;
                    if (!ReferenceEquals(oldPlotView, null) &&
                        !ReferenceEquals(oldPlotView, this))
                    {
                        // This PlotModel is already in use by some other PlotView control.
                        plotModel.AttachPlotView(null);
                    }

                    plotModel.AttachPlotView(this);
                    this.ActualModel = this.Model;
                }
            }

            this.InvalidatePlot();
        }

        /// <summary>
        /// Renders the plot model to the plot presenter.
        /// </summary>
        protected void Render()
        {
            if (this.plotPresenter == null || this.renderContext == null || !(this.isInVisualTree = this.IsInVisualTree()))
            {
                return;
            }

            this.RenderOverride();
        }

        /// <summary>
        /// Renders the plot model to the plot presenter.
        /// </summary>
        protected virtual void RenderOverride()
        {
            var dpiScale = this.UpdateDpi();
            this.ClearBackground();

            this.HideTracker();

            if (this.ActualModel != null)
            {
                // round width and height to full device pixels
                var width = (int)(this.plotPresenter.Width * dpiScale) / dpiScale;
                var height = (int)(this.plotPresenter.Height * dpiScale) / dpiScale;

                lock (this.ActualModel.SyncRoot)
                {
                    ((IPlotModel)this.ActualModel).Render(this.renderContext, new OxyRect(0, 0, width, height));
                }
            }
        }

        /// <summary>
        /// Updates the DPI scale of the render context.
        /// </summary>
        /// <returns>The DPI scale.</returns>
        protected virtual double UpdateDpi()
        {
            return DeviceDisplay.MainDisplayInfo.Density;
        }

        /// <summary>
        /// Called when the model is changed.
        /// </summary>
        //event data.</param>
        private static void ModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((PlotViewBase)bindable).OnModelChanged();
        }

        /// <summary>
        /// Invokes the specified action on the dispatcher, if necessary.
        /// </summary>
        /// <param name="action">The action.</param>
        private void BeginInvoke(Action action)
        {
            if (!CheckAccess())
            {
                this.Dispatcher.Dispatch(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Determines whether the calling thread is the main thread 
        /// </summary>
        /// <returns></returns>
        private bool CheckAccess()
        {
            return Thread.CurrentThread.ManagedThreadId == mainThreadId;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="PlotViewBase"/> is connected to the visual tree.
        /// </summary>
        /// <returns><c>true</c> if the PlotViewBase is connected to the visual tree; <c>false</c> otherwise.</returns>
        private bool IsInVisualTree()
        {
            Microsoft.Maui.Controls.Element dpObject = this;
            while ((dpObject = dpObject.Parent) != null)
            {
                if (dpObject is Page)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This event fires every time Layout updates the layout of the trees associated with current Dispatcher.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            // if we were not in the visual tree the last time we tried to render but are now, we have to render
            if (!this.isInVisualTree && this.IsInVisualTree())
            {
                this.Render();
            }
        }
    }
}