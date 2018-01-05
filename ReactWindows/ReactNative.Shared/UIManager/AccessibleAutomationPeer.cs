using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactNative.UIManager.Events;
#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
#else
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
#endif

namespace ReactNative.UIManager
{
    /// <summary>
    /// Automation peer for a FrameworkElement that implements IAccessible.
    /// </summary>
    /// <typeparam name="T">Type of owner of the automation peer.</typeparam>
    public sealed class AccessibleAutomationPeer<T> : FrameworkElementAutomationPeer, IInvokeProvider
        where T : FrameworkElement, IAccessible
    {
        /// <summary>
        /// Hides base UIElement Owner to provide stronger-typed T Owner.
        /// </summary>
        private new T Owner => (T)base.Owner;

        /// <inheritdoc />
        public AccessibleAutomationPeer(T owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override IList<AutomationPeer> GetChildrenCore()
        {
            bool shouldHideChildren =
                Owner.ImportantForAccessibility == ImportantForAccessibility.Yes
                || Owner.ImportantForAccessibility == ImportantForAccessibility.NoHideDescendants;
            return shouldHideChildren ? null : base.GetChildrenCore();
        }

        /// <inheritdoc />
        protected override bool IsContentElementCore()
        {
            return IsVisibleToScreenReader() ?? base.IsContentElementCore();
        }

        /// <inheritdoc />
        protected override bool IsControlElementCore()
        {
            return IsVisibleToScreenReader() ?? base.IsControlElementCore();
        }

        /// <summary>
        /// Checks if this peer should be visible to screen reader according to 
        /// owner's <see cref="ImportantForAccessibility"/>.
        /// </summary>
        /// <returns>True if visible, false if not, null if not defined.</returns>
        private bool? IsVisibleToScreenReader()
        {
            switch (Owner.ImportantForAccessibility)
            {
                case ImportantForAccessibility.Yes:
                    return true;
                case ImportantForAccessibility.No:
                case ImportantForAccessibility.NoHideDescendants:
                    return false;
                case ImportantForAccessibility.Auto:
                    return null;
                default:
                    throw new InvalidOperationException(FormattableString.Invariant(
                        $"Unknown {nameof(ImportantForAccessibility)} value {Owner.ImportantForAccessibility}"));
            }
        }

        /// <inheritdoc />
        protected override string GetNameCore()
        {
            string baseName = base.GetNameCore();
            bool hasName = !string.IsNullOrWhiteSpace(baseName);
            bool aggregationIsNotRequired = Owner.ImportantForAccessibility != ImportantForAccessibility.Yes;
            if (hasName || aggregationIsNotRequired)
            {
                return baseName;
            }
            return GetRecursivelyAggregatedName(this);
        }

        private string GetRecursivelyAggregatedName(AutomationPeer peer)
        {
            var sb = new StringBuilder();
            foreach (var child in peer.GetChildren())
            {
                string name = child.GetName();
                if (string.IsNullOrEmpty(name))
                {
                    name = GetRecursivelyAggregatedName(child);
                }
                if (!string.IsNullOrEmpty(name))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(name);
                }
            }
            return sb.ToString();
        }

        /// <inheritdoc />
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            if (Owner.AccessibilityTraits?.Contains(AccessibilityTrait.Button) == true)
            {
                return AutomationControlType.Button;
            }
            return base.GetAutomationControlTypeCore();
        }

        /// <inheritdoc />
        protected override object GetPatternCore(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Invoke
                && Owner.AccessibilityTraits?.Contains(AccessibilityTrait.Button) == true)
            {
                return this;
            }
            return base.GetPatternCore(patternInterface);
        }

        /// <inheritdoc />
        public void Invoke()
        {
            Owner.GetReactContext()
                .GetNativeModule<UIManagerModule>()
                .EventDispatcher
                .DispatchEvent(new AccessibilityTapEvent(Owner.GetTag()));
        }
    }
}
