using Microsoft.Maui.Controls.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using OxyPlot.Maui.Skia.Effects;

namespace OxyPlot.Maui.Skia.Windows.Effects
{
    public class PlatformTouchEffect : PlatformEffect
    {
        FrameworkElement frameworkElement;
        Action<Microsoft.Maui.Controls.Element, TouchActionEventArgs> onTouchAction;

        protected override void OnAttached()
        {
            // Get the Windows FrameworkElement corresponding to the Element that the effect is attached to
            frameworkElement = Control == null ? Container : Control;

            // Get access to the TouchEffect class in the .NET Standard library
            var touchEffect = Element.Effects.OfType<MyTouchEffect>().FirstOrDefault();

            if (touchEffect != null && frameworkElement != null)
            {
                // Save the method to call on touch events
                onTouchAction = touchEffect.OnTouchAction;

                // Set event handlers on FrameworkElement
                frameworkElement.PointerPressed += OnPointerPressed;
                frameworkElement.PointerMoved += OnPointerMoved;
                frameworkElement.PointerReleased += OnPointerReleased;
            }
        }

        protected override void OnDetached()
        {
            if (onTouchAction != null)
            {
                frameworkElement.PointerPressed -= OnPointerPressed;
                frameworkElement.PointerMoved -= OnPointerMoved;
                frameworkElement.PointerReleased -= OnPointerReleased;
            }
        }

        private bool _pressed = false;
        void OnPointerPressed(object sender, PointerRoutedEventArgs args)
        {
            _pressed = true;
            CommonHandler(sender, TouchActionType.Pressed, args);
        }

        void OnPointerMoved(object sender, PointerRoutedEventArgs args)
        {
            if (_pressed)
                CommonHandler(sender, TouchActionType.Moved, args);
        }

        void OnPointerReleased(object sender, PointerRoutedEventArgs args)
        {
            _pressed = false;
            CommonHandler(sender, TouchActionType.Released, args);
        }

        void CommonHandler(object sender, TouchActionType touchActionType, PointerRoutedEventArgs args)
        {
            var pointerPoint = args.GetCurrentPoint(sender as UIElement);
            var windowsPoint = pointerPoint.Position;
            onTouchAction(Element, new TouchActionEventArgs(args.Pointer.PointerId,
                                                            touchActionType,
                                                            new Point[] { new(windowsPoint.X, windowsPoint.Y) },
                                                            args.Pointer.IsInContact));
        }
    }
}