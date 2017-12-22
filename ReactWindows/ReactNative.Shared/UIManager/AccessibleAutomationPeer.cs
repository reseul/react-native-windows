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
            return Owner.ImportantForAccessibility == ImportantForAccessibility.Yes;
        }

        /// <inheritdoc />
        protected override bool IsControlElementCore()
        {
            return Owner.ImportantForAccessibility == ImportantForAccessibility.Yes;
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
        protected override string GetNameCore()
        {
            string baseName = base.GetNameCore();
            if (!string.IsNullOrWhiteSpace(baseName)
                || Owner.ImportantForAccessibility != ImportantForAccessibility.Yes)
            {
                return baseName;
            }

            var sb = new StringBuilder();
            foreach (var peer in base.GetChildrenCore())
            {
                sb.Append(peer.GetName()).Append(" ");
            }
            return sb.ToString();
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
                .DispatchEvent(new InvokeEvent(Owner.GetTag()));
        }
    }
}
