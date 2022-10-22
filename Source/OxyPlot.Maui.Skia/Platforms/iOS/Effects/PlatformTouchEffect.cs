using CoreGraphics;
using Foundation;
using Microsoft.Maui.Controls.Platform;
using OxyPlot.Maui.Skia.Effects;
using UIKit;

namespace OxyPlot.Maui.Skia.ios.Effects
{
    public class PlatformTouchEffect : PlatformEffect
    {
        UIView view;
        TouchRecognizer touchRecognizer;

        protected override void OnAttached()
        {
            // Get the iOS UIView corresponding to the Element that the effect is attached to
            view = Control == null ? Container : Control;

            // Uncomment this line if the UIView does not have touch enabled by default
            //view.UserInteractionEnabled = true;

            // Get access to the TouchEffect class in the .NET Standard library
            var touchEffect = Element.Effects.OfType<MyTouchEffect>().FirstOrDefault();

            if (touchEffect != null && view != null)
            {
                // Create a TouchRecognizer for this UIView
                touchRecognizer = new TouchRecognizer(Element, view, touchEffect);
                view.AddGestureRecognizer(touchRecognizer);
            }
        }

        protected override void OnDetached()
        {
            if (touchRecognizer != null)
            {
                // Clean up the TouchRecognizer object
                touchRecognizer.Detach();

                // Remove the TouchRecognizer from the UIView
                view.RemoveGestureRecognizer(touchRecognizer);
            }
        }
    }

    class TouchRecognizer : UIGestureRecognizer
    {
        Microsoft.Maui.Controls.Element element;        // Forms element for firing events
        UIView view;            // iOS UIView 
        MyTouchEffect touchPlatformEffect;

        static Dictionary<UIView, TouchRecognizer> viewDictionary = new();

        static Dictionary<long, TouchRecognizer> idToTouchDictionary = new();

        public TouchRecognizer(Microsoft.Maui.Controls.Element element, UIView view, MyTouchEffect touchPlatformEffect)
        {
            this.element = element;
            this.view = view;
            this.touchPlatformEffect = touchPlatformEffect;

            viewDictionary.Add(view, this);
        }

        public void Detach()
        {
            viewDictionary.Remove(view);
        }

        // touches = touches of interest; evt = all touches of type UITouch
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = ((IntPtr)touch.Handle).ToInt64();
                FireEvent(this, id, TouchActionType.Pressed, touch, true);

                if (!idToTouchDictionary.ContainsKey(id))
                {
                    idToTouchDictionary.Add(id, this);
                }
            }
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = ((IntPtr)touch.Handle).ToInt64();
                CheckForBoundaryHop(touch);
                if (idToTouchDictionary[id] != null)
                {
                    FireEvent(idToTouchDictionary[id], id, TouchActionType.Moved, touch, true);
                }
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = ((IntPtr)touch.Handle).ToInt64();
                CheckForBoundaryHop(touch);
                if (idToTouchDictionary[id] != null)
                {
                    FireEvent(idToTouchDictionary[id], id, TouchActionType.Released, touch, false);
                }

                idToTouchDictionary.Remove(id);
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = ((IntPtr)touch.Handle).ToInt64();
                idToTouchDictionary.Remove(id);
            }
        }

        void CheckForBoundaryHop(UITouch touch)
        {
            long id = ((IntPtr)touch.Handle).ToInt64();

            // TODO: Might require converting to a List for multiple hits
            TouchRecognizer recognizerHit = null;

            foreach (UIView view in viewDictionary.Keys)
            {
                CGPoint location = touch.LocationInView(view);

                if (new CGRect(new CGPoint(), view.Frame.Size).Contains(location))
                {
                    recognizerHit = viewDictionary[view];
                }
            }
            if (recognizerHit != idToTouchDictionary[id])
            {
                if (idToTouchDictionary[id] != null)
                {
                    FireEvent(idToTouchDictionary[id], id, TouchActionType.Pressed, touch, true);
                }
                if (recognizerHit != null)
                {
                    FireEvent(recognizerHit, id, TouchActionType.Released, touch, true);
                }
                idToTouchDictionary[id] = recognizerHit;
            }
        }

        void FireEvent(TouchRecognizer recognizer, long id, TouchActionType actionType, UITouch touch, bool isInContact)
        {
            // Convert touch location to Xamarin.Forms Point value
            CGPoint cgPoint = touch.LocationInView(recognizer.View);
            Point xfPoint = new Point(cgPoint.X, cgPoint.Y);

            // Get the method to call for firing events
            var onTouchAction = recognizer.touchPlatformEffect.OnTouchAction;

            // Call that method
            onTouchAction(recognizer.element,
                new TouchActionEventArgs(id, actionType, new[] { xfPoint }, isInContact));
        }
    }
}